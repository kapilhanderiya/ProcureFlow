using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Approvals;
using ProcureFlow.Domain.Common.Exceptions;


namespace ProcureFlow.Domain.Tests.Approvals
{
    public class ApprovalTests
    {
        [Fact]
        public void Constructor_ShouldCreatePendingApproval()
        {
            var purchaseRequestId = Guid.NewGuid();
            var approverRoleId = Guid.NewGuid();

            var approval = new Approval(purchaseRequestId, approverRoleId, 1);

            Assert.Equal(purchaseRequestId, approval.PurchaseRequestId);
            Assert.Equal(approverRoleId, approval.ApproverRoleId);
            Assert.Equal(ApprovalStatus.Pending, approval.Status);
            Assert.Equal(1, approval.Sequence);
        }


        private static Approval CreateApproval()
        {
            var purchaseRequestId = Guid.NewGuid();
            var approverRoleId = Guid.NewGuid();
            return new Approval(purchaseRequestId, approverRoleId, 1);
        }

        [Fact]
        public void Approve_ShouldChangeStatusToApproved()
        {
            var approval = CreateApproval();

            approval.Approve("Approved for purchase");

            Assert.Equal("Approved for purchase", approval.Comments);
            Assert.Equal(ApprovalStatus.Approved, approval.Status);
            Assert.NotNull(approval.DecisionAt);
        }

        [Fact]
        public void Reject_ShouldRequireComments()
        {
            var approval = CreateApproval();
            var action = () => approval.Reject("");
            Assert.Throws<DomainException>(action);
            Assert.Equal(ApprovalStatus.Pending, approval.Status);
        }

        [Fact]
        public void Reject_ShouldChangeStatusToRejected()
        {
            var approval = CreateApproval();
            approval.Reject("Budget is Insuficient");
            Assert.Equal("Budget is Insuficient", approval.Comments);
            Assert.Equal(ApprovalStatus.Rejected, approval.Status);
            Assert.NotNull(approval.DecisionAt);
        }

        [Fact]
        public void Approve_ShouldNotAllowSecondDecision()
        {
            var approval = CreateApproval();
            approval.Approve("Approved for purchase");
            var action = () => approval.Reject("Rejecting after approving");
            Assert.Throws<DomainException>(action);
            Assert.Equal(ApprovalStatus.Approved, approval.Status);
        }

    }
}
