namespace ProcureFlow.Domain.Common.Entities
{
    public abstract class SoftDeletableEntity : AuditableEntity
    {
        public bool IsDeleted { get; protected set; }

        public DateTimeOffset? DeletedAt { get; protected set; }

        public Guid? DeletedBy { get; protected set; }

        public void Delete(Guid deletedBy)
        {
            if (IsDeleted)
                return;
            IsDeleted = true;
            DeletedAt = DateTimeOffset.UtcNow;
            this.DeletedBy = deletedBy;
        }

        public void Restore()
        {
            IsDeleted = false;
            DeletedAt = null;
            DeletedBy = null;
        }

    }
}
