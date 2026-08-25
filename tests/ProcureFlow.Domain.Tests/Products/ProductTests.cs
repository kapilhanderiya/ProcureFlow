using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.ValueObjects;
using ProcureFlow.Domain.Products;

namespace ProcureFlow.Domain.Tests.Products
{
    public class ProductTests
    {
        [Fact]
        public void Constructor_ShouldCreateActiveProduct()
        {
            var product = CreateProduct();

            Assert.Equal("Office Laptop", product.Name);
            Assert.Equal("LAP-001", product.SKU);
            Assert.Equal(ProductStatus.Active, product.Status);
        }

        [Fact]
        public void Constructor_ShouldRejectEmptyName()
        {
            var action = () => new Product(
                "",
                "LAP-001",
                "Business laptop",
                new Money(75000m, "INR"));
            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void Constructor_ShouldRejectEmptySKU()
        {
            var action = () => new Product(
                "Office Laptop",
                "",
                "Business laptop",
                new Money(75000m, "INR"));
            Assert.Throws<DomainException>(action);
        }

        [Fact]
        public void Rename_ShouldUpdateName()
        {
            var product = CreateProduct();
            product.Rename("Business Laptop");
            Assert.Equal("Business Laptop", product.Name);
        }

        [Fact]
        public void Rename_ShouldUpdatePrice()
        {
            var product = CreateProduct();
            product.ChangeUnitPrice(new Money(100000m, "INR"));
            Assert.Equal(new Money(100000m, "INR"), product.UnitPrice);
        }

        public void Rename_ShouldUpdateDescription()
        {
            var product = CreateProduct();
            product.UpdateDescription("Office Laptop");
            Assert.Equal("Office Laptop", product.Description);
        }

        [Fact]
        public void Activate_ShouldSetStatusToInActive()
        {
            var product = CreateProduct();
            product.Deactivate();
            Assert.Equal(ProductStatus.Inactive, product.Status);
        }

        [Fact]
        public void Activate_ShouldSetStatusToActive()
        {
            var product = CreateProduct();
            product.Deactivate();
            product.Activate();
            Assert.Equal(ProductStatus.Active, product.Status);
        }

        [Fact]
        public void Activate_ShouldSetStatusToDiscontinue()
        {
            var product = CreateProduct();
            product.Discontinue();
            Assert.Equal(ProductStatus.Discontinued, product.Status);
        }

        private static Product CreateProduct()
        {
            return new Product(
            "Office Laptop",
            "LAP-001",
            "Business laptop",
            new Money(75000m, "INR"));
        }
    }
}
