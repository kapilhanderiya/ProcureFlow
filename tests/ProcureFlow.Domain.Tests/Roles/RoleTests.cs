using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Roles;

namespace ProcureFlow.Domain.Tests.Roles;
public class RoleTests
{
    [Fact]
    public void Constructor_ShouldCreateRole()
    {
        var role = new Role(
            "Manager",
            "Can approve purchase requests.");

        Assert.Equal("Manager", role.Name);
        Assert.Equal("Can approve purchase requests.", role.Description);
        Assert.Empty(role.RolePermissions);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyName()
    {
        var action = () => new Role(
            "",
            "Can approve purchase requests.");

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyDescription()
    {
        var action = () => new Role(
            "Manager",
            "");

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Rename_ShouldUpdateName()
    {
        var role = new Role(
            "Manager",
            "Can approve purchase requests.");

        role.Rename("Finance Manager");

        Assert.Equal("Finance Manager", role.Name);
    }

    [Fact]
    public void UpdateDescription_ShouldUpdateDescription()
    {
        var role = new Role(
            "Manager",
            "Can approve purchase requests.");

        role.UpdateDescription("Can approve financial purchase requests.");

        Assert.Equal("Can approve financial purchase requests.", role.Description);
    }

    [Fact]
    public void AddPermission_ShouldAddPermissionToRole()
    {
        var role = new Role(
            "Manager",
            "Can approve purchase requests.");

        var permission = new Permission(
            "purchase_request.read",
            "Read Purchase Requests",
            "Allows reading purchase requests.");

        role.AddPermission(permission);

        var rolePermission = Assert.Single(role.RolePermissions);

        Assert.Equal(role.Id, rolePermission.RoleId);

        Assert.Equal(permission.Id, rolePermission.PermissionId);
    }

    [Fact]
    public void AddPermission_ShouldRejectDuplicatePermission()
    {
        var role = new Role(
            "Manager",
            "Can approve purchase requests.");

        var permission = new Permission(
            "purchase_request.read",
            "Read Purchase Requests",
            "Allows reading purchase requests.");

        role.AddPermission(permission);

        var action = () => role.AddPermission(permission);

        Assert.Throws<DomainException>(action);

        Assert.Single(role.RolePermissions);
    }

    [Fact]
    public void AddPermission_ShouldRejectNullPermission()
    {
        var role = new Role(
            "Manager",
            "Can approve purchase requests.");

        var action = () => role.AddPermission(null!);

        Assert.Throws<ArgumentNullException>(action);
    }

    [Fact]
    public void RemovePermission_ShouldRemoveAssignedPermission()
    {
        var role = new Role(
            "Manager",
            "Can approve purchase requests.");

        var permission = new Permission(
            "purchase_request.read",
            "Read Purchase Requests",
            "Allows reading purchase requests.");

        role.AddPermission(permission);
        role.RemovePermission(permission.Id);

        Assert.Empty(role.RolePermissions);
    }

    [Fact]
    public void RemovePermission_ShouldDoNothing_WhenPermissionIsNotAssigned()
    {
        var role = new Role(
            "Manager",
            "Can approve purchase requests.");

        role.RemovePermission(Guid.NewGuid());

        Assert.Empty(role.RolePermissions);
    }
}