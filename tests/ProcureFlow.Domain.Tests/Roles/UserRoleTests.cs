using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Roles;

namespace ProcureFlow.Domain.Tests.Roles;

public class UserRoleTests
{
    [Fact]
    public void Constructor_ShouldCreateUserRole()
    {
        var userId = Guid.NewGuid();
        var roleId = Guid.NewGuid();

        var userRole = new UserRole(
            userId, 
            roleId);

        Assert.Equal(userId, userRole.UserId);
        Assert.Equal(roleId, userRole.RoleId);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyUserId()
    {
        var action = () => new UserRole(
            Guid.Empty,
            Guid.NewGuid());

        Assert.Throws<DomainException>(action);
    }

    [Fact]
    public void Constructor_ShouldRejectEmptyRoleId()
    {
        var action = () => new UserRole(
            Guid.NewGuid(),
            Guid.Empty);

        Assert.Throws<DomainException>(action);
    }
}
