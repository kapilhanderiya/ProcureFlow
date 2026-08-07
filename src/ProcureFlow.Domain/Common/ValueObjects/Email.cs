using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace ProcureFlow.Domain.Common.ValueObjects
{
    public sealed class Email : IEquatable<Email>
    {
        private static readonly Regex EmailRegex =
            new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public string Value { get; }

        public Email(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Email is Required.", nameof(value));

            value = value.Trim().ToLowerInvariant();

            if (!EmailRegex.IsMatch(value))
                throw new ArgumentException("Invalid email format.", nameof(value));

            Value = value;
        }

        public override string ToString() => Value;

        public bool Equals(Email? other) => other is not null && Value == other.Value;

        public override bool Equals(object? obj) => Equals(obj as Email);

        public override int GetHashCode() => Value.GetHashCode();

        public static implicit operator string (Email email) => email.Value;
    }
}
