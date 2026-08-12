using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.Guards;
using ProcureFlow.Domain.Common.ValueObjects;


namespace ProcureFlow.Domain.Vendors
{
    public class Vendor : SoftDeletableEntity
    {
        public string Name { get; private set; } = null!;

        public string Code { get; private set; } = null!;

        public string TaxIdentifier { get; private set; } = null!;

        public Email Email { get; private set; } = null!;

        public string Phone { get; private set; } = null!;

        public Address Address { get; private set; } = null!;

        public VendorStatus Status { get; private set; }

        private Vendor()
        {
        }

        public Vendor(string name, string code, string taxIdentifier, Email email, string phone, Address address)
        {
            Rename(name);
            Code = DomainGuard.Required(code, "Vendor code is required.").ToUpperInvariant();
            TaxIdentifier = DomainGuard.Required(taxIdentifier, "Vendor tax identifier is required.");
            ChangeEmail(email);
            ChangePhone(phone);
            ChangeAddress(address);
            Status = VendorStatus.Active;
        }

        public void Rename(string name)
        {
            Name = DomainGuard.Required(name, "Vendor name is required.");
        }

        public void ChangeEmail(Email email) {
            ArgumentNullException.ThrowIfNull(email);
            Email = email;
        }

        public void ChangePhone(string phone)
        {
            Phone = DomainGuard.Required(phone, "Vendor phone is required.");
        }

        public void ChangeAddress(Address address)
        {
            ArgumentNullException.ThrowIfNull(address);
            Address = address;
        }

        public void Activate()
        {
            if(Status == VendorStatus.Active)
            {
                return;
            }
            Status = VendorStatus.Active;
        }

        public void Deactivate()
        {
            if(Status == VendorStatus.Inactive)
            {
                return;
            }
            Status = VendorStatus.Inactive;
        }

        public void Block()
        {
            if (Status == VendorStatus.Blocked)
            {
                return;
            }
            Status = VendorStatus.Blocked;
        }

    }
}