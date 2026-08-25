using ProcureFlow.Domain.Common.Events;
namespace ProcureFlow.Domain.PurchaseRequests.Events
{
    public sealed record PurchaseRequestApprovedEvent(Guid PurchaseRequestId, Guid RequesterId) : DomainEvent;
}
