using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.ValueObjects;


namespace ProcureFlow.Domain.Users
{
    public class User : SoftDeletableEntity
    {
        public string FirstName { get; private set; } = null!;

        public string LastName { get; private set; } = null!;

        public Email Email { get; private set; } = null!;

        public UserStatus Status { get; private set; }

        private User()
        {
        }

        public User(string firstName, string lastName, string email)
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
    }
}