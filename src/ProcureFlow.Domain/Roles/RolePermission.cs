using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Entities;

namespace ProcureFlow.Domain.Roles
{
    public class RolePermission : AuditableEntity
    {
        public Guid RoleId { get; private set; }

        public Guid PermissionId { get; private set; }

        public Role Role { get; private set; } = null!;

        public Permission Permission { get; private set; } = null!;

        private RolePermission()
        {
        }
        
        public RolePermission(Guid roleId, Guid permissionId)
        {
            RoleId = roleId;
            PermissionId = permissionId;
        }

    }
}
