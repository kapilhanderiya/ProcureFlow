namespace ProcureFlow.Domain.Common.Events
{
    public interface IDomainEvent
    {
        DateTime OccuredAtUtc { get; }
    }
}
