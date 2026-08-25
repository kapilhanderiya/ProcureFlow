using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Roles;

namespace ProcureFlow.Domain.Tests.Roles;

public class RolePermissionTests
{
    [Fact]
    public void Constructor_ShouldCreateRolePermission()
    {
        var roleId = Guid.NewGuid();
        var permissionId = Guid.NewGuid();

        var rolePermission = new RolePermission(
            roleId,
            permissionId);

        Assert.Equal(roleId, rolePermission.RoleId);
        Assert.Equal(permissionId, rolePermission.PermissionId);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyRoleId()
    {
        var action = () => new RolePermission(
            Guid.Empty,
            Guid.NewGuid());

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyPermissionId()
    {
        var action = () => new RolePermission(
            Guid.NewGuid(),
            Guid.Empty);

        Assert.Throws<DomainException>(action);
    }
}
