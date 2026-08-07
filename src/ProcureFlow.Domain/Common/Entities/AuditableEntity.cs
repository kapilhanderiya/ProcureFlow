using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcureFlow.Domain.Common.Entities
{
    public abstract class AuditableEntity
    {
        public DateTimeOffset CreatedAt { get; protected set; }

        public Guid CreatedBy { get; protected set; }

        public DateTimeOffset? UpdatedAt { get; protected set; }

        public Guid? UpdatedBy { get; protected set; }
    }
}
