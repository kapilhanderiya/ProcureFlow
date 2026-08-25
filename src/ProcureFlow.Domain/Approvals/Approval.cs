using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.Guards;


namespace ProcureFlow.Domain.Approvals
{
    public class Approval : AuditableEntity
    {
        public Guid PurchaseRequestId { get; private set; }

        public Guid ApproverRoleId { get; private set; }

        public int Sequence { get; private set; }
        
        public ApprovalStatus Status { get; private set; }

        public string? Comments { get; private set; }

        public DateTime? DecisionAt { get; private set; }

        private Approval()
        {
        }

        public Approval(Guid purchaseRequestId, Guid approverRoleId, int sequence)
        {
            if(purchaseRequestId == Guid.Empty)
            {
                throw new DomainException("Purchase request ID is required.");
            }
            if(approverRoleId == Guid.Empty)
            {
                throw new DomainException("Approver ID is required.");
            }
            if(sequence <= 0)
            {
                throw new DomainException("Approval sequence must be greater than zero.");
            }
            PurchaseRequestId = purchaseRequestId;
            ApproverRoleId = approverRoleId;
            Sequence = sequence;
            Status = ApprovalStatus.Pending;
        }

        public void Approve(string? comments = null)
        {
            EnsurePending();
            Status = ApprovalStatus.Approved;
            Comments = NormalizeComments(comments);
            DecisionAt = DateTime.UtcNow;
        }

        public void Reject(string comments)
        {
            EnsurePending();
            Comments = DomainGuard.Required(NormalizeComments(comments), "Rejection comments are required.");
            Status = ApprovalStatus.Rejected;
            DecisionAt = DateTime.UtcNow;
        }

        public void skip()
        {
            EnsurePending();
            Status = ApprovalStatus.Skipped;
            DecisionAt = DateTime.UtcNow;
        }

        public void EnsurePending()
        {
            if(Status != ApprovalStatus.Pending)
            {
                throw new DomainException("This approval is already been decided.");
            }
        }

        public string? NormalizeComments(string? comments)
        { 
            return string.IsNullOrWhiteSpace(comments) ? null : comments.Trim();
        }
    }
}
