using ProcureFlow.Domain.Common.Exceptions;


namespace ProcureFlow.Domain.Approvals
{
    public sealed class ApprovalRequirement
    {
        public Guid ApproverRoleId { get; }

        public int Sequence { get; }

        public ApprovalRequirement(Guid approverRoleId,  int sequence)
        {
            if(approverRoleId == Guid.Empty)
            {
                throw new DomainException("Approver role ID is required.");
            }
            if(sequence <= 0)
            {
                throw new DomainException("Approval sequence must be greater than zero.");
            }
            ApproverRoleId = approverRoleId;
            Sequence = sequence;
        }
    }
}
