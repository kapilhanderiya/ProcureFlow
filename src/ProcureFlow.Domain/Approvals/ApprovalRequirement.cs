using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Exceptions;


namespace ProcureFlow.Domain.Approvals
{
    public sealed class ApprovalRequirement
    {
        public Guid ApprovalRoleId { get; }

        public int Sequence { get; }

        public ApprovalRequirement(Guid approvalRoleId,  int sequence)
        {
            if(approvalRoleId == Guid.Empty)
            {
                throw new DomainException("Approver role ID is required.");
            }
            if(sequence <= 0)
            {
                throw new DomainException("Approval sequence must be greater than zero.");
            }
            ApprovalRoleId = approvalRoleId;
            Sequence = sequence;
        }
    }
}
