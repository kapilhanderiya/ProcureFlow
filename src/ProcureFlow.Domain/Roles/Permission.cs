using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Guards;

namespace ProcureFlow.Domain.Roles
{
    public class Permission : BaseEntity
    {

        public string Code { get; private set; } = null!;

        public string Name { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        private Permission()
        {

        }

        public Permission(string code, string name, string description)
        {
            Code = DomainGuard.Required(code, "Permission code is required").ToUpperInvariant();
            Name = DomainGuard.Required(name, "Permission name is required");
            Description = DomainGuard.Required(description, "Permission description is required");
        }

        public void Rename(string name)
        {
            Name = DomainGuard.Required(name, "Permission name is required");
        }

        public void ChangeDescription(string description)
        {
            Description = DomainGuard.Required(description, "Permission description is required");
        }

    }
}
