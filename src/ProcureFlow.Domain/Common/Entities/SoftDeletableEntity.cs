using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcureFlow.Domain.Common.Entities
{
    public abstract class SoftDeletableEntity : AuditableEntity
    {
        public bool isDeleted { get; protected set; }

        public DateTimeOffset? DeletedAt { get; protected set; }

        public Guid? DeletedBy { get; protected set; }

        public void Delete(Guid DeletedBy)
        {
            if (isDeleted)
                return;
            isDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
            this.DeletedBy = DeletedBy;
        }

        public void Restore()
        {
            isDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }

    }
}
