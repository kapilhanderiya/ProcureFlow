using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.Guards;

namespace ProcureFlow.Domain.Roles;

public class Role : SoftDeletableEntity
{
    private readonly List<RolePermission> _rolePermissions = [];

    public string Name { get; private set; } = null!;

    public string Description { get; private set; } = null!;

    public IReadOnlyCollection<RolePermission> RolePermissions
        => _rolePermissions.AsReadOnly();

    private Role()
    {
        // Required by EF Core
    }

    public Role(string name, string description)
    {
        Name = DomainGuard.Required(name, "Role name is required.");
        Description = DomainGuard.Required(description, "Role description is required.");
    }

    public void Rename(string name)
    {
        Name = DomainGuard.Required(name, "Role name is required.");
    }

    public void UpdateDescription(string description)
    {
        Description = DomainGuard.Required(description, "Role description is required.");
    }

    public void AddPermission(Permission permission)
    {
        ArgumentNullException.ThrowIfNull(permission);

        if (_rolePermissions.Any(rp => rp.PermissionId == permission.Id))
            throw new DomainException("The role already contains this permission.");

        _rolePermissions.Add(new RolePermission(Id, permission.Id));
    }

    public void RemovePermission(Guid permissionId)
    {
        var rolePermission = _rolePermissions
            .FirstOrDefault(rp => rp.PermissionId == permissionId);

        if (rolePermission is null)
            return;

        _rolePermissions.Remove(rolePermission);
    }
}