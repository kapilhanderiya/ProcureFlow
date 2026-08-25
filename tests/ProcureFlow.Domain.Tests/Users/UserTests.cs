using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.ValueObjects;
using ProcureFlow.Domain.Roles;
using ProcureFlow.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProcureFlow.Domain.Tests.Users
{
    public class UserTests
    {
        [Fact]
        public void Constructor_ShouldCreateActiveUser()
        {
            var email = new Email("johndoe@example.com");
            var user = new User(
                "John",
                "Doe",
                email);

            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
            Assert.Equal(email, user.Email);
            Assert.Equal(UserStatus.Active, user.Status);
            Assert.Empty(user.UserRoles);
        }

        [Fact]
        public void SetName_ShouldRejectEmptyFirstName()
        {
            var email = new Email("johndoe@example.com");
            var action = () => new User(
                "",
                "Doe",
                email);

            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void SetName_ShouldRejectEmptyLastName()
        {
            var email = new Email("johndoe@example.com");
            var action = () => new User(
                "John",
                "",
                email);

            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void ChangeEmail_ShouldUpdateEmail()
        {
            var user = CreateUser();
            var newEmail = new Email("new.email@example.com");
            user.ChangeEmail(newEmail);

            Assert.Equal(newEmail, user.Email);
        }

        [Fact]
        public void Deactivate_ShouldSetStatusToInactive()
        {
            var user = CreateUser();
            user.Deactivate();

            Assert.Equal(UserStatus.Inactive, user.Status);
        }

        [Fact]
        public void Activate_ShouldSetStatusToActive()
        {
            var user = CreateUser();
            user.Deactivate();
            user.Activate();

            Assert.Equal(UserStatus.Active, user.Status);
        }

        [Fact]
        public void Lock_ShouldSetStatusToLocked()
        {
            var user = CreateUser();
            user.Lock();

            Assert.Equal(UserStatus.Locked, user.Status);
        }

        [Fact]
        public void AssignRole_ShouldAssignRoleToUser()
        {
            var user = CreateUser();
            var role = new Role(
                "manager",
                "MANAGER");
            user.AssignRole(role);
            var userRole = Assert.Single(user.UserRoles);
            Assert.Equal(role.Id, userRole.RoleId);
            Assert.Equal(user.Id, userRole.UserId);
        }

        [Fact]
        public void AssignRole_ShouldRejectDuplicateRole()
        {
            var user = CreateUser();
            var role = new Role(
                "manager",
                "MANAGER");
            user.AssignRole(role);
            var action = () => user.AssignRole(role);
            Assert.Throws<DomainException>(action);
            Assert.Single(user.UserRoles);
        }

        [Fact]
        public void RemoveRole_ShouldRemoveAssignedRole()
        {
            var user = CreateUser();
            var role = new Role(
                "manager",
                "MANAGER");

            user.AssignRole(role);
            user.RemoveRole(role.Id);

            Assert.Empty(user.UserRoles);
        }

        [Fact]
        public void RemoveRole_ShouldRejectUnassignedRole()
        {
            var user = CreateUser();
            var roleId = Guid.NewGuid();

            var action = () => user.RemoveRole(roleId);

            Assert.Throws<DomainException>(action);
        }

        private static User CreateUser()
        {
            return new User(
                "John",
                "Doe",
                new Email("john.doe@example.com"));
        }
    }
}
