using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcureFlow.Domain.Common.Events
{
    public abstract record DomainEvent : IDomainEvent
    {
        public DateTime OccuredAtUtc { get; init; } = DateTime.UtcNow;
    }
}
