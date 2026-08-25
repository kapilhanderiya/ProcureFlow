using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Guards;

namespace ProcureFlow.Domain.Departments
{
    public class Department : SoftDeletableEntity
    {
        public string Name { get; private set; } = null!;

        public string Code { get; private set; } = null!;

        private Department()
        {

        }

        public Department(string name, string code)
        {
            Name = DomainGuard.Required(name, "Department name is required.");
            Code = DomainGuard.Required(code, "Department code is required.").ToUpperInvariant();
        }

        public void Rename(string name)
        {
            Name = DomainGuard.Required(name, "Department name is required.");
        }

        public void ChangeCode(string code)
        {
            Code = DomainGuard.Required(code, "Department code is required.").ToUpperInvariant();
        }

    }
}
