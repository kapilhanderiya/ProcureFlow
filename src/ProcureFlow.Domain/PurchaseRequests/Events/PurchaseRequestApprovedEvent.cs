namespace ProcureFlow.Domain.PurchaseRequests.Events
{
    public sealed record PurchaseRequestApprovedEvent(Guid PurchaseRequestId, Guid RequesterId) : DomainEvent;
}
