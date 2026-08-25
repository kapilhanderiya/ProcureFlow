using ProcureFlow.Domain.Common.Events;

namespace ProcureFlow.Domain.PurchaseRequests.Events
{
    public sealed record PurchaseRequestRejectedEvent(Guid PurchaseRequestId, Guid RequesterId) : DomainEvent;
}
