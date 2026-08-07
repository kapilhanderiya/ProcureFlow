using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using ProcureFlow.Domain.Common.Exceptions;

namespace ProcureFlow.Domain.Common.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        private static readonly Regex EmailRegex =
            new (@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Email is required.");

            value = value.Trim().ToLowerInvariant();

            if (!EmailRegex.IsMatch(value))
                throw new DomainException("Invalid email format.");

            Value = value;
        }

        public override string ToString() => Value;

        public bool Equals(Email? other) => other is not null && Value == other.Value;

        public override bool Equals(object? obj) => Equals(obj as Email);

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string (Email email) => email.Value;

        public static explicit operator Email(string value) => new(value);
    }
}
