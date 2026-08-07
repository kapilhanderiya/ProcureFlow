using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.ValueObjects;
using ProcureFlow.Domain.Roles;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProcureFlow.Domain.Users
{
    public class User : SoftDeletableEntity
    {

        private readonly List<UserRole> _userRoles = [];

        public string FirstName { get; private set; } = null!;

        public string LastName { get; private set; } = null!;

        public Email Email { get; private set; } = null!;

        public UserStatus Status { get; private set; }

        public IReadOnlyCollection<UserRole> UserRoles => _userRoles.AsReadOnly();

        private User()
        {
        }

        public User(string firstName, string lastName, Email email)
        {
            SetName(firstName, lastName);
            ChangeEmail(email);
            Status = UserStatus.Active;
        }

        public void SetName(string firstName, string lastName)
        {
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
        }

        public void ChangeEmail(Email email)
        {
            Email = email;
        }

        public void Activate()
        {
            Status = UserStatus.Active;
        }

        public void Deactivate()
        {
            Status = UserStatus.Inactive;
        }

        public void Lock()
        {
            Status = UserStatus.Locked;
        }

        public void AssignRole(Role role)
        {
            ArgumentNullException.ThrowIfNull(role);

            if (_userRoles.Any(ur => ur.RoleId == role.Id))
                throw new DomainException("The user is already assigned to this role.");

            _userRoles.Add(new UserRole(Id, role.Id));
        }

        public void RemoveRole(Guid roleId)
        {
            var userRole = _userRoles
                .FirstOrDefault(ur => ur.RoleId == roleId);

            if (userRole is null)
                return;

            _userRoles.Remove(userRole);
        }

    }
}