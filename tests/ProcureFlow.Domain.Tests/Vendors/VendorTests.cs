
using ProcureFlow.Domain.Common.ValueObjects;
using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Vendors;


namespace ProcureFlow.Domain.Tests.Vendors
{
    public class VendorTests
    {
        [Fact]
        public void Constructor_ShouldCreateActiveVendor()
        {
            var email = new Email("vendor@example.com");
            var vendor = new Vendor(
            "Acme Supplies",
            "acme-001",
            "GST123456",
            email,
            "9876543210",
            CreateAddress());

            Assert.Equal("Acme Supplies", vendor.Name);
            Assert.Equal("acme-001".ToUpperInvariant(), vendor.Code);
            Assert.Equal("GST123456", vendor.TaxIdentifier);
            Assert.Equal(email, vendor.Email);
            Assert.Equal("9876543210", vendor.Phone);
            Assert.Equal(VendorStatus.Active, vendor.Status);
        }

        [Fact]
        public void Constructor_ShouldRejectEmptyName()
        {
            var email = new Email("vendor@example.com");
            var action = () => new Vendor(
                "",
                "acme-001",
                "GST123456",
                email,
                "9876543210",
                CreateAddress());
            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void Constructor_ShouldRejectEmptyCode()
        {
            var email = new Email("vendor@example.com");
            var action = () => new Vendor(
                "Acme Supplies",
                "",
                "GST123456",
                email,
                "9876543210",
                CreateAddress());
            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void Rename_ShouldUpdateName()
        {
            var vendor = CreateVendor();
            vendor.Rename("New Vendor Name");
            Assert.Equal("New Vendor Name", vendor.Name);
        }

        [Fact]
        public void ChangePhone_ShouldUpdatePhone()
        {
            var vendor = CreateVendor();
            vendor.ChangePhone("9999999999");
            Assert.Equal("9999999999", vendor.Phone);
        }

        [Fact]
        public void ChangeEmail_ShouldUpdateEmail()
        {
            var vendor = CreateVendor();
            var newEmail = new Email("new.vendor@example.com");
            vendor.ChangeEmail(newEmail);
            Assert.Equal(newEmail, vendor.Email);
        }

        public void ChangeAddress_ShouldUpdateAddress()
        {
            var vendor = CreateVendor();
            var newAddress = new Address(
                "123 Business Street",
                null,
                "Panjim",
                "Goa",
                "------",
                "India");
            vendor.ChangeAddress(newAddress);
            Assert.Equal(newAddress, vendor.Address);
        }

        [Fact]
        public void Deactivate_ShouldSetStatusToInactive()
        {
            var vendor = CreateVendor();
            vendor.Deactivate();
            Assert.Equal(VendorStatus.Inactive, vendor.Status);
        }

        [Fact]
        public void Activate_ShouldSetStatusToActive()
        {
            var vendor = CreateVendor();
            vendor.Deactivate();
            vendor.Activate();
            Assert.Equal(VendorStatus.Active, vendor.Status);
        }

        [Fact]
        public void Blocked_ShouldSetStatusToBlocked()
        {
            var vendor = CreateVendor();
            vendor.Block();
            Assert.Equal(VendorStatus.Blocked, vendor.Status);
        }

        private static Vendor CreateVendor()
        {
            var email = new Email("vendor@example.com");
            return new Vendor(
                "Acme Supplies",
                "acme-001",
                "GST123456",
                email,
                "9876543210",
                CreateAddress());
        }

        private static Address CreateAddress()
        {
            return new Address(
                "123 Business Street",
                null,
                "Ponda",
                "Goa",
                "403401",
                "India");
        }
    }
}
