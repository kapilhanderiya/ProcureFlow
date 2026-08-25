namespace ProcureFlow.Domain.Common.Events
{
    public abstract record DomainEvent : IDomainEvent
    {
        public DateTime OccuredAtUtc { get; init; } = DateTime.UtcNow;
    }
}
