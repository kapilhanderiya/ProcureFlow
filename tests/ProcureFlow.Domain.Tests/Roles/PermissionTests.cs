using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Roles;

namespace ProcureFlow.Domain.Tests.Roles;

public class PermissionTests
{
    [Fact]
    public void Constructor_ShouldCreatePermission()
    {
        var permission = new Permission(
            "purchase_request.read",
            "Read Purchase Requests",
            "Allows reading purchase requests.");

        Assert.Equal("PURCHASE_REQUEST.READ", permission.Code);
        Assert.Equal("Read Purchase Requests", permission.Name);
        Assert.Equal("Allows reading purchase requests.", permission.Description);
    }

    [Fact]
    public void Constructor_ShouldNormalizeCode()
    {
        var permission = new Permission(
            " purchase_request.read ",
            "Read Purchase Requests",
            "Allows reading purchase requests.");

        Assert.Equal("PURCHASE_REQUEST.READ", permission.Code);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyCode()
    {
        var action = () => new Permission(
            "",
            "Read Purchase Requests",
            "Allows reading purchase requests.");

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyName()
    {
        var action = () => new Permission(
            "purchase_request.read",
            "",
            "Allows reading purchase requests.");

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyDescription()
    {
        var action = () => new Permission(
            "purchase_request.read",
            "Read Purchase Requests",
            "");

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Rename_ShouldUpdateName()
    {
        var permission = new Permission(
            "purchase_request.read",
            "Read Purchase Requests",
            "Allows reading purchase requests.");

        permission.Rename("View Purchase Requests");

        Assert.Equal("View Purchase Requests", permission.Name);
    }

    [Fact]
    public void ChangeDescription_ShouldUpdateDescription()
    {
        var permission = new Permission(
            "purchase_request.read",
            "Read Purchase Requests",
            "Allows reading purchase requests.");

        permission.ChangeDescription("Allows users to view purchase requests.");

        Assert.Equal("Allows users to view purchase requests.", permission.Description);
    }
}
