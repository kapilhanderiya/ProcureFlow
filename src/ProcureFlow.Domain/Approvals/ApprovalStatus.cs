using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcureFlow.Domain.Approvals
{
    public enum ApprovalStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3,
        Skipped = 4
    }
}
