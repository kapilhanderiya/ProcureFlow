namespace ProcureFlow.Domain.PurchaseRequests.Events
{
    public sealed record PurchaseRequestSubmittedEvent(Guid PurchaseRequestId, Guid RequesterId) : DomainEvent;
}
