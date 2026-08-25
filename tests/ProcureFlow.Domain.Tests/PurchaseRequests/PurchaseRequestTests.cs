using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.ValueObjects;
using ProcureFlow.Domain.PurchaseRequests;
using ProcureFlow.Domain.Approvals;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProcureFlow.Domain.Tests.PurchaseRequests
{
    public class PurchaseRequestTests
    {
        [Fact]
        public void Constructor_ShouldCreateDraftPurchaseRequest()
        {
            var requesterId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();

            var request = new PurchaseRequest(
                "PR-000001",
                requesterId,
                departmentId,
                "Office equipment");

            Assert.Equal(PurchaseRequestStatus.Draft, request.Status);
            Assert.Equal(requesterId, request.RequesterId);
            Assert.Equal(departmentId, request.DepartmentId);
            Assert.Empty(request.Items);
            Assert.Empty(request.Approvals);
        }

        [Fact]
        public void Constructor_ShouldRejectEmptyRequesterId()
        {
            var departmentId = Guid.NewGuid();
            var action = () => new PurchaseRequest(
                "PR-000001",
                Guid.Empty,
                departmentId,
                "Office equipment");
            Assert.Throws<DomainException>(action);
        }

        private static PurchaseRequest CreatePurchaseRequest()
        {
            var requesterId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();

            return new PurchaseRequest(
                "PR-000001",
                requesterId,
                departmentId,
                "Office equipment");
        }

        [Fact]
        public void Submit_ShouldRejectRequestWithoutItems()
        {
            var request = CreatePurchaseRequest();

            var action = () => request.Submit();

            Assert.Throws<DomainException>(action);

            Assert.Equal(PurchaseRequestStatus.Draft, request.Status);
        }

        [Fact]
        public void AddItem_ShouldAddItemToPurchaseRequest()
        {
            var request = CreatePurchaseRequest();
            var productId = Guid.NewGuid();
            var price = new Money(1000m, "INR");

            request.AddItem(productId, "Office Chair", 2, price);

            var item = Assert.Single(request.Items);
            Assert.Equal(productId, item.ProductId);
            Assert.Equal(2m, item.Quantity);
            Assert.Equal(price, item.UnitPrice);
            Assert.Equal("INR", request.Currency);
        }

        [Fact]
        public void AddItem_ShouldRejectItemWithDifferentCurrency()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(Guid.NewGuid(), "Laptop", 1, new Money(70000m, "INR"));

            var action = () => request.AddItem(Guid.NewGuid(), "Monitor", 1, new Money(15000m, "USD"));

            Assert.Throws<DomainException>(action);
            Assert.Single(request.Items);
        }

        [Fact]
        public void GetTotal_ShouldCalculatePurchaseRequestTotal()
        {
            var request = CreatePurchaseRequest();

            request.AddItem(Guid.NewGuid(), "Laptop", 1, new Money(70000m, "INR"));
            request.AddItem(Guid.NewGuid(), "Monitor", 2, new Money(15000m, "INR"));

            var total = request.GetTotal();
            Assert.Equal(100000m, total.Amount);
            Assert.Equal("INR", total.Currency);
        }

        [Fact]
        public void Submit_ShouldMoveRequestToSubmitted()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(Guid.NewGuid(), "Laptop", 1, new Money(70000m, "INR"));
            request.Submit();
            
            Assert.Equal(PurchaseRequestStatus.Submitted, request.Status);
        }

        [Fact]
        public void CreateApprovals_ShouldMoveRequestToUnderReview()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(Guid.NewGuid(), "Laptop", 1, new Money(70000m, "INR"));
            request.Submit();
            var requirements = new[]
            {
                new ApprovalRequirement(Guid.NewGuid(), 1)
            };
            request.CreateApprovals(requirements);
            Assert.Equal(PurchaseRequestStatus.UnderReview, request.Status);
            Assert.Single(request.Approvals);
            Assert.Equal(ApprovalStatus.Pending, request.Approvals.Single().Status);
        }

        [Fact]
        public void ApproveStep_ShouldRequirePreviousStepToBeApproved()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(Guid.NewGuid(), "Laptop", 1, new Money(70000m, "INR"));
            request.Submit();
            var managerRoleId = Guid.NewGuid();
            var financeRoleId = Guid.NewGuid();
            var requirements = new[]
            {
                new ApprovalRequirement(managerRoleId, 1),
                new ApprovalRequirement(financeRoleId, 2)
            };

            request.CreateApprovals(requirements);

            var financeApproval = request.Approvals.Single(a => a.Sequence == 2);

            var action = () => request.ApproveStep(financeApproval.Id);

            Assert.Throws<DomainException>(action);

        }

        [Fact]
        public void ApproveStep_ShouldApproveRequestAfterAllStepsAreApproved()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(Guid.NewGuid(), "Laptop", 1, new Money(70000m, "INR"));
            request.Submit();
            request.CreateApprovals(
            [
                new ApprovalRequirement(Guid.NewGuid(), 1),
                new ApprovalRequirement(Guid.NewGuid(), 2)
            ]);
            var approvals = request.Approvals
                .OrderBy(a => a.Sequence)
                .ToList();
            request.ApproveStep(approvals[0].Id);
            Assert.Equal(PurchaseRequestStatus.UnderReview,request.Status);
            request.ApproveStep(approvals[1].Id);
            Assert.Equal(PurchaseRequestStatus.Approved, request.Status);
        }

        [Fact]
        public void RejectStep_ShouldRejectPurchaseRequest()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(Guid.NewGuid(), "Laptop", 1, new Money(70000m, "INR"));
            request.Submit();
            request.CreateApprovals(
            [
                new ApprovalRequirement(Guid.NewGuid(), 1),
                new ApprovalRequirement(Guid.NewGuid(), 2)
            ]);
            request.ApproveStep(request.Approvals.Single(x => x.Sequence == 1).Id);
            var approval = request.Approvals.Single(x => x.Sequence == 2);

            request.RejectStep(approval.Id, "Not needed");

            Assert.Equal(PurchaseRequestStatus.Rejected, request.Status);
            Assert.Equal("Not needed", approval.Comments);
            Assert.Equal(ApprovalStatus.Rejected, approval.Status);
        }
    }
}
