using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcureFlow.Domain.PurchaseRequests
{
    public enum PurchaseRequestStatus
    {
        Draft = 1,
        Submitted = 2,
        UnderReview = 3,
        Approved = 4,
        Rejected = 5,
        Cancelled = 6
    }
}
