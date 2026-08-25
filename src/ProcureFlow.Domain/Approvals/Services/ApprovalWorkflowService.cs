using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.ValueObjects;
using ProcureFlow.Domain.PurchaseRequests;

namespace ProcureFlow.Domain.Approvals.Services
{
    public class ApprovalWorkflowService
    {
        public IReadOnlyCollection<ApprovalRequirement> DetermineRequirements(
            PurchaseRequest purchaseRequest,
            IReadOnlyCollection<ApprovalRule> rules)
        {
            ArgumentNullException.ThrowIfNull(purchaseRequest);
            ArgumentNullException.ThrowIfNull(rules);

            if(rules.Count == 0)
            {
                return [];
            }
            var total = purchaseRequest.GetTotal();
            var requirements = rules
                .Where(rule => Matches(rule, purchaseRequest, total))
                .OrderBy(rule => rule.Sequence)
                .Select(rule => new ApprovalRequirement(
                    rule.ApproverRoleId,
                    rule.Sequence))
                .ToList();

            return requirements.AsReadOnly();
            
        }

        private static bool Matches(ApprovalRule rule, PurchaseRequest purchaseRequest, Money total)
        {
            return rule.RuleType switch
            {
                ApprovalRuleType.AmountThreshold => total.Amount >= rule.MinimumAmount!.Value,
                ApprovalRuleType.Department => purchaseRequest.DepartmentId == rule.DepartmentId!.Value,
                _ => throw new DomainException("Unsupported approval rule type.")
            };
        }
    }
}