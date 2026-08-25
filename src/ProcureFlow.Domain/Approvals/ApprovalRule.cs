using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.Guards;

namespace ProcureFlow.Domain.Approvals
{
    public class ApprovalRule : SoftDeletableEntity
    {
        public string Name { get; private set; } = null!;

        public ApprovalRuleType RuleType { get; private set; }

        public decimal? MinimumAmount { get; private set; }

        public Guid? DepartmentId { get; private set; }

        public Guid ApproverRoleId { get; private set; }

        public int Sequence { get; private set; }

        private ApprovalRule()
        {
        }

        public ApprovalRule(string name, ApprovalRuleType ruleType, Guid approverRoleId, int sequence, decimal? minimumAmount = null, Guid? departmentId = null)
        {
            Name = DomainGuard.Required(name, "Approval rule name is required.");
            if(approverRoleId == Guid.Empty)
            {
                throw new DomainException("Approver role ID is required.");
            }
            if(sequence <= 0)
            {
                throw new DomainException("Approval sequence must be greater than zero.");
            }
            if(minimumAmount is < 0)
            {
                throw new DomainException("Minimum amount must be a positive value.");
            }
            if(departmentId == Guid.Empty)
            {
                throw new DomainException("Department ID cannot be empty.");
            }
            switch (ruleType)
            {
                case ApprovalRuleType.AmountThreshold:
                    if(minimumAmount is null)
                    {
                        throw new DomainException("Amount threshold is required for amount-based approval rules.");
                    }
                    break;

                case ApprovalRuleType.Department:
                    if(departmentId is null)
                    {
                        throw new DomainException("Department ID is required for department-based approval rules.");
                    }
                    break;

                default:
                    throw new DomainException("Unsupported approval rule type.");
            }
            RuleType = ruleType;
            ApproverRoleId = approverRoleId;
            Sequence = sequence;
            MinimumAmount = minimumAmount;
            DepartmentId = departmentId;

        }
        

    }
}
