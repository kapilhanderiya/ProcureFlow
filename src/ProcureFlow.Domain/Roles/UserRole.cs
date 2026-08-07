using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Users;

namespace ProcureFlow.Domain.Roles;

public class UserRole : AuditableEntity
{
    public Guid UserId { get; private set; }

    public Guid RoleId { get; private set; }

    public User User { get; private set; } = null!;

    public Role Role { get; private set; } = null!;

    private UserRole()
    {
    }

    public UserRole(Guid userId, Guid roleId)
    {
        UserId = userId;
        RoleId = roleId;
    }
}