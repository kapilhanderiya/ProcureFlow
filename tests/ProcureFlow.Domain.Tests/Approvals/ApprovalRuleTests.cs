using ProcureFlow.Domain.Approvals;
using ProcureFlow.Domain.Common.Exceptions;

namespace ProcureFlow.Domain.Tests.Approvals
{
    public class ApprovalRuleTests
    {
        [Fact]
        public void AmountThresholdRule_ShouldRequireMinimunAmount()
        {
            var action = () => new ApprovalRule(
                "Finance Approval",
                ApprovalRuleType.AmountThreshold,
                Guid.NewGuid(),
                1);
            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void DepartmentRule_ShouldRequireDepartment()
        {
            var action = () => new ApprovalRule(
                "Engineering Department",
                ApprovalRuleType.Department,
                Guid.NewGuid(),
                1);
            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void AmountThreshodRule_ShouldBeCreated()
        {
            var roleId = Guid.NewGuid();
            var rule = new ApprovalRule(
                "Finance Approval",
                ApprovalRuleType.AmountThreshold,
                roleId,
                1,
                minimumAmount: 500000m);

            Assert.Equal(ApprovalRuleType.AmountThreshold, rule.RuleType);
            Assert.Equal(500000m, rule.MinimumAmount);
            Assert.Equal(roleId, rule.ApproverRoleId);
            Assert.Equal(1, rule.Sequence);
        }

        [Fact]
        public void DepartmentRule_ShouldBeCreated()
        {
            var roleId = Guid.NewGuid();
            var departmentId = Guid.NewGuid();
            var rule = new ApprovalRule(
                "Engineering Approval",
                ApprovalRuleType.Department,
                roleId,
                1,
                departmentId: departmentId);

            Assert.Equal(ApprovalRuleType.Department, rule.RuleType);
            Assert.Equal(departmentId, rule.DepartmentId);
            Assert.Equal(roleId, rule.ApproverRoleId);
        }
    }
}
