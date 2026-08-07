using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcureFlow.Domain.Common.Enums
{
    public enum PurchaseOrderStatus
    {
        Draft = 1,
        Issued = 2,
        PartiallyReceived = 3,
        Received = 4,
        Closed = 5,
        Cancelled = 6
    }
}
