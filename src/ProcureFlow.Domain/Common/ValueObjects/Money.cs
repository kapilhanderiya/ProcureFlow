using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using ProcureFlow.Domain.Common.Exceptions;

namespace ProcureFlow.Domain.Common.ValueObjects
{
    public sealed class Money : IEquatable<Money>
    {
        public  decimal Amount { get; }

        public string Currency { get; }

        public Money(decimal amount, string currency)
        {
            if (amount < 0)
            {
                throw new DomainException("Amount cannot be negative.");
            }
            if (string.IsNullOrWhiteSpace(currency))
            {
                throw new DomainException("Currency cannot be null or empty.");
            }
            if (Currency.Length != 3)
            {
                throw new DomainException("Currency must be a 3-letter ISO currency code.");
            }
            Amount = amount;
            Currency = currency.Trim().ToUpperInvariant();
        }

        public Money Add(Money other)
        {
            ArgumentNullException.ThrowIfNull(other);

            EnsureSameCurrency(other);

            return new Money(Amount + other.Amount, Currency);
        }

        public Money Subtract(Money other)
        {
            ArgumentNullException.ThrowIfNull(other);

            EnsureSameCurrency(other);

            if (other.Amount > Amount)
            {
                throw new DomainException("Money Amount cannot become negative.");
            }

            return new Money(Amount - other.Amount, Currency);
        }

        public Money Multiply(decimal multiplier)
        {
            if(multiplier < 0)
            {
                throw new DomainException("Multiplier cannot be negative.");
            }

            return new Money(Amount * multiplier, Currency);
        }

        private void EnsureSameCurrency(Money other)
        {
           if(!string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase))
            {
                throw new DomainException("Cannot perform monetary operations with different currencies.");
            }
        }

        public bool Equals(Money? other)
        {
            if(other is null)
            {
                return false;
            }
            return Amount == other.Amount && string.Equals(Currency, other.Currency, StringComparison.OrdinalIgnoreCase);

        }

        public override bool Equals(object? obj) => Equals(obj as Money);

        public override int GetHashCode() => HashCode.Combine(Amount, Currency.ToUpperInvariant());

        public override string ToString() => $"{Amount:0.00} {Currency}";
    }
}
