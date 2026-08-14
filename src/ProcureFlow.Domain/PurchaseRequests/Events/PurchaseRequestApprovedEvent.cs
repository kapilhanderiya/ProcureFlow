using ProcureFlow.Domain.Common.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcureFlow.Domain.PurchaseRequests.Events
{
    public sealed record PurchaseRequestApprovedEvent(Guid PurchaseRequestId, Guid RequesterId) : DomainEvent;
}
