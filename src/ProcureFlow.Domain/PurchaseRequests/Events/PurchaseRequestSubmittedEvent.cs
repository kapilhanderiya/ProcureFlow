using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProcureFlow.Domain.Common.Events;

namespace ProcureFlow.Domain.PurchaseRequests.Events
{
    public sealed record PurchaseRequestSubmittedEvent(Guid PurchaseRequestId, Guid RequesterId) : DomainEvent;
}
