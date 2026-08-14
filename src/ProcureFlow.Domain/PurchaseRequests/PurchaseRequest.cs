    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using ProcureFlow.Domain.Common.Entities;
    using ProcureFlow.Domain.Common.Exceptions;
    using ProcureFlow.Domain.Common.Guards;
    using ProcureFlow.Domain.Common.ValueObjects;
    using ProcureFlow.Domain.Approvals;
    using System.Net.WebSockets;
using ProcureFlow.Domain.PurchaseRequests.Events;

namespace ProcureFlow.Domain.PurchaseRequests
    {
        public class PurchaseRequest : AuditableEntity
        {
            private readonly List<PurchaseRequestItem> _items = [];
            private readonly List<Approval> _approvals = [];

            public string RequestNumber { get; private set; } = null!;
        
            public Guid RequesterId { get; private set; }

            public Guid DepartmentId { get; private set; }

            public string? Currency { get; private set; } 

            public string Justification { get; private set; } = null!;

            public PurchaseRequestStatus Status { get; private set; }

            public IReadOnlyCollection<PurchaseRequestItem> Items => _items.AsReadOnly();
            public IReadOnlyCollection<Approval> Approvals => _approvals.AsReadOnly();

            private PurchaseRequest()
            {
            }

            public PurchaseRequest(string requestNumber, Guid requesterId, Guid departmentId, string justification)
            {
                RequestNumber = DomainGuard.Required(requestNumber, "Purchase request number is required.");
                if(requesterId == Guid.Empty)
                {
                    throw new DomainException("Requester ID is required.");
                }
                RequesterId = requesterId;
                if(departmentId == Guid.Empty)
                {
                    throw new DomainException("Department ID is required.");
                }
                DepartmentId = departmentId;
                Justification = DomainGuard.Required(justification, "Purchase request justification is required.");
                Status = PurchaseRequestStatus.Draft;
            }

            public void AddItem(Guid productId, string description, decimal quantity, Money unitPrice)
            {
                EnsureDraft();
                ArgumentNullException.ThrowIfNull(unitPrice);
                if (Currency is null)
                {
                    Currency = unitPrice.Currency;
                }
                else if (!string.Equals(Currency,unitPrice.Currency,StringComparison.OrdinalIgnoreCase))
                {
                    throw new DomainException("All purchase request items must use the same currency.");
                }
                var item = new PurchaseRequestItem(productId, description, quantity, unitPrice);
                _items.Add(item);
            }

            public void RemoveItem(Guid itemId)
            {
                EnsureDraft();
                var item = _items.FirstOrDefault(x => x.Id == itemId);
                item = item ?? throw new DomainException("Purchase request item was not found.");
                _items.Remove(item);
                if (_items.Count == 0)
                {
                    Currency = null;
                }
            }

            public void Submit()
            {
                if(Status != PurchaseRequestStatus.Draft)
                {
                    throw new DomainException("Only draft purchase requests can be submitted.");
                }
                if(_items.Count == 0)
                {
                    throw new DomainException("A purchase request must contain at least one item.");
                }
                Status = PurchaseRequestStatus.Submitted;
                AddDomainEvent(new PurchaseRequestSubmittedEvent(Id, RequesterId));
            }

            public void StartReview()
            {
                if(Status != PurchaseRequestStatus.Submitted)
                {
                    throw new DomainException("Only submitted purchase requests can enter review.");
                }
                Status = PurchaseRequestStatus.UnderReview;
            }

            public void Approve()
            {
                if (Status != PurchaseRequestStatus.UnderReview)
                {
                    throw new DomainException("Only purchase requests under review can be approved.");
                }
                Status = PurchaseRequestStatus.Approved;
            }

            public void Reject()
            {
                if (Status != PurchaseRequestStatus.UnderReview)
                {
                    throw new DomainException("Only purchase requests under review can be rejected.");
                }
                Status = PurchaseRequestStatus.Rejected;
            }

            public void Cancel()
            {
                if(Status != PurchaseRequestStatus.Draft && Status != PurchaseRequestStatus.Submitted && Status != PurchaseRequestStatus.UnderReview)
                {
                    throw new DomainException("Only draft, submitted, or under-review purchase requests can be cancelled.");
                }
                Status = PurchaseRequestStatus.Cancelled;
            }

            private void EnsureDraft()
            {
                if(Status != PurchaseRequestStatus.Draft)
                {
                    throw new DomainException("Purchase request can only be modified while in draft status.");
                }
            }

            private void EnsureUnderReview()
            {
                if (Status != PurchaseRequestStatus.UnderReview)
                {
                    throw new DomainException("Purchase request can only be modified while in under review status.");
                }
            }

            public Money GetTotal()
            {
                if(_items.Count == 0)
                {
                    throw new DomainException("Cannot calculate total for a purchase request without items.");
                }
                var total = _items[0].GetTotal();
                foreach(var item in _items.Skip(1))
                {
                    total = total.Add(item.GetTotal());
                }
                return total;
            }

            public void CreateApprovals(IReadOnlyCollection<ApprovalRequirement> requirements)
            {
                EnsureDraft();
                ArgumentNullException.ThrowIfNull(requirements);
                if(requirements.Count == 0)
                {
                    throw new DomainException("At least one approval requirement is required.");
                }
                if(_approvals.Count > 0)
                {
                    throw new DomainException("Approvals have already been created for this purchase request.");
                }

                foreach(var requirement in requirements)
                {
                    _approvals.Add(new Approval(Id, requirement.ApprovalRoleId, requirement.Sequence));
                }
                Status = PurchaseRequestStatus.UnderReview;
            }

            public void ApproveStep(Guid approvelId, string? comments = null)
            {
                EnsureUnderReview();
                var approval = _approvals.FirstOrDefault(ap =>  ap.Id == approvelId);
                if(approval == null)
                {
                    throw new DomainException("Approval was not found");
                }
                var previousApprovals = _approvals
                   .Where(a => a.Sequence < approval.Sequence)
                   .OrderByDescending(a => a.Sequence)
                   .FirstOrDefault();
                if (previousApprovals is not null && previousApprovals.Status != ApprovalStatus.Approved)
                {
                    throw new DomainException("Cannot reject this approval step because the previous step has not been approved.");
                }
                approval.Approve(comments);
            AddDomainEvent(new PurchaseRequestApprovedEvent(Id, RequesterId));
            }

            public void RejectStep(Guid approvelId, string comments)
            {
                EnsureUnderReview();
                var approval = _approvals.FirstOrDefault(ap => ap.Id == approvelId);
                if (approval is null)
                {
                    throw new DomainException("Approval was not found");
                }
                var previousApprovals = _approvals
                    .Where(a => a.Sequence < approval.Sequence)
                    .OrderByDescending(a => a.Sequence)
                    .FirstOrDefault();
                if(previousApprovals is not null && previousApprovals.Status != ApprovalStatus.Approved)
                {
                    throw new DomainException("Cannot reject this approval step because the previous step has not been approved.");
                }
                approval.Reject(comments);
                Status = PurchaseRequestStatus.Rejected;
                AddDomainEvent(new PurchaseRequestRejectedEvent(Id, RequesterId));
            }
        }  
    }
