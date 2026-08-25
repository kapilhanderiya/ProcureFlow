using ProcureFlow.Domain.Approvals;
using ProcureFlow.Domain.Approvals.Services;
using ProcureFlow.Domain.Common.ValueObjects;
using ProcureFlow.Domain.PurchaseRequests;

namespace ProcureFlow.Domain.Tests.Approvals
{
    public class ApprovalWorkflowServiceTests
    {   
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
        public void DetermineRequirements_ShouldRequireAmountApproval_WhenThresholdIsMet()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(
                Guid.NewGuid(),
                "Laptop",
                2,
                new Money(300000m, "INR"));
            var roleId = Guid.NewGuid();
            var rule = new ApprovalRule(
                "Finance Approval",
                ApprovalRuleType.AmountThreshold,
                roleId,
                1,
                minimumAmount: 500000m);
            var service = new ApprovalWorkflowService();
            var requirements = service.DetermineRequirements(
                request,
                [rule]);
            var requirement = Assert.Single(requirements);

            Assert.Equal(roleId, requirement.ApproverRoleId);
            Assert.Equal(1, requirement.Sequence);
        }

        [Fact]
        public void DetermineRequirements_ShouldNotRequireAmountApproval_WhenThresholdIsNotMet()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(
                Guid.NewGuid(),
                "Laptop",
                1,
                new Money(100000m, "INR"));
            var roleId = Guid.NewGuid();
            var rule = new ApprovalRule(
                "Finance Approval",
                ApprovalRuleType.AmountThreshold,
                roleId,
                1,
                minimumAmount: 500000m);
            var service = new ApprovalWorkflowService();
            var requirements = service.DetermineRequirements(
                request,
                [rule]);

            Assert.Empty(requirements);
        }

        [Fact]
        public void DetermineRequirements_ShouldRequireDepartmentApproval_WhenDepartmentMatches()
        {
            var departmentId = Guid.NewGuid();
            var roleId = Guid.NewGuid();
            var request = new PurchaseRequest(
                "PR-000001",
                Guid.NewGuid(),
                departmentId,
                "Engineering equipment");
            request.AddItem(
                Guid.NewGuid(),
                "Laptop",
                1,
                new Money(100000m, "INR"));
            var rule = new ApprovalRule(
                "Engineering Approval",
                ApprovalRuleType.Department,
                roleId,
                1,
                departmentId: departmentId);
            var service = new ApprovalWorkflowService();
            var requirement = service.DetermineRequirements(
                request,
                [rule]).Single();
            Assert.Equal(roleId, requirement.ApproverRoleId);
        }

        [Fact]
        public void DetermineRequirements_ShouldNotRequireDepartmentApproval_WhenDepartmentDoesNotMatch()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(
               Guid.NewGuid(),
               "Laptop",
               1,
               new Money(100000m, "INR"));
            var rule = new ApprovalRule(
                "Engineering Approval",
                ApprovalRuleType.Department,
                Guid.NewGuid(),
                1,
                departmentId: Guid.NewGuid());
            var service = new ApprovalWorkflowService();
            var requirements = service.DetermineRequirements(
                request,
                [rule]);
            Assert.Empty(requirements);
        }

        [Fact]
        public void DetermineRequirements_ShouldReturnRequirementsInSequenceOrder()
        {
            var request = CreatePurchaseRequest();
            request.AddItem(
               Guid.NewGuid(),
               "Laptop",
               1,
               new Money(1000000m, "INR"));
            var firstRoleId = Guid.NewGuid();
            var secondRoleId = Guid.NewGuid();
            var rules = new[]
            {
                new ApprovalRule(
                    "Finance Approval",
                    ApprovalRuleType.AmountThreshold,
                    secondRoleId,
                    2,
                    minimumAmount: 500000m),
                new ApprovalRule(
                    "Manager Approval",
                    ApprovalRuleType.AmountThreshold,
                    firstRoleId,
                    1,
                    minimumAmount: 100000m)
            };
            var service = new ApprovalWorkflowService();
            Console.WriteLine($"Total: {request.GetTotal().Amount}");

            foreach (var rule in rules)
            {
                Console.WriteLine(
                    $"Rule: {rule.Name}, " +
                    $"Role: {rule.ApproverRoleId}, " +
                    $"Sequence: {rule.Sequence}, " +
                    $"MinimumAmount: {rule.MinimumAmount}");
            }

            var requirements = service.DetermineRequirements(
                request,
                rules);
            var ordered = requirements.ToList();

            Assert.Equal(2, ordered.Count);
            Assert.Equal(1, ordered[0].Sequence);
            Assert.Equal(firstRoleId, ordered[0].ApproverRoleId);
            Assert.Equal(2, ordered[1].Sequence);
            Assert.Equal(secondRoleId, ordered[1].ApproverRoleId);
        }
    }
}
