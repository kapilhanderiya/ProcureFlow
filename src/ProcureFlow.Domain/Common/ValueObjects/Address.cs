using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.Guards;

namespace ProcureFlow.Domain.Common.ValueObjects
{
    public sealed class Address : IEquatable<Address>
    {
        public string AddressLine1 { get; }

        public string? AddressLine2 { get; }

        public string City { get; }

        public string State { get; }

        public string PostalCode { get; }

        public string Country { get; }

        public Address(string addressLine1, string? addressLine2, string city, string state, string postalCode, string country)
        {
            AddressLine1 = DomainGuard.Required(addressLine1, "Address line 1 is required");
            AddressLine2 = string.IsNullOrWhiteSpace(addressLine2) ? null : addressLine2.Trim();
            City = DomainGuard.Required(city, "City is required");
            State = DomainGuard.Required(state, "State is required");
            PostalCode = DomainGuard.Required(postalCode, "Postal code is required");
            Country = DomainGuard.Required(country, "Country is required");
        }


        public bool Equals(Address? other)
        {
            if(other is null)
            {
                return false;
            }
             return string.Equals(AddressLine1, other.AddressLine1, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(AddressLine2, other.AddressLine2, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(City, other.City, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(State, other.State, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(PostalCode, other.PostalCode, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Country, other.Country, StringComparison.OrdinalIgnoreCase);
        }

        public override bool Equals(object? obj) => Equals(obj as Address);

        public override int GetHashCode() => HashCode.Combine(
            StringComparer.OrdinalIgnoreCase.GetHashCode(AddressLine1),
            StringComparer.OrdinalIgnoreCase.GetHashCode(AddressLine2 ?? string.Empty),
            StringComparer.OrdinalIgnoreCase.GetHashCode(City),
            StringComparer.OrdinalIgnoreCase.GetHashCode(State),
            StringComparer.OrdinalIgnoreCase.GetHashCode(PostalCode),
            StringComparer.OrdinalIgnoreCase.GetHashCode(Country));

        public override string ToString() => string.Join(
            ", ",
            new[] { AddressLine1, AddressLine2, City, State, PostalCode, Country }.Where(s => !string.IsNullOrWhiteSpace(s))
        );
    }
}
