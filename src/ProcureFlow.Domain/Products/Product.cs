using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Guards;
using ProcureFlow.Domain.Common.ValueObjects;



namespace ProcureFlow.Domain.Products
{
    public class Product : SoftDeletableEntity
    {
        public string Name { get; private set; } = null!;

        public string SKU { get; private set; } = null!;

        public string Description { get; private set; } = null!;

        public Money UnitPrice { get; private set; } = null!;

        public ProductStatus Status { get; private set; }

        private Product()
        {
        }

        public Product(string name, string sku, string description, Money unitPrice)
        {
            Rename(name);
            SKU = DomainGuard.Required(sku, "Product SKU is required.").ToUpperInvariant();
            UpdateDescription(description);
            ChangeUnitPrice(unitPrice);
            Status = ProductStatus.Active;
        }

        public void Rename(string name)
        {
            Name = DomainGuard.Required(name, "Product name is required.");
        }

        public void UpdateDescription(string description)
        {
            Description = DomainGuard.Required(description, "Product description is required.");
        }

        public void ChangeUnitPrice(Money unitPrice)
        {
            ArgumentNullException.ThrowIfNull(unitPrice);
            UnitPrice = unitPrice;
        }

        public void Activate()
        {
            if (Status == ProductStatus.Active)
                return;
            Status = ProductStatus.Active;
        }

        public void Deactivate()
        {
            if (Status == ProductStatus.Inactive)
                return;
            Status = ProductStatus.Inactive;
        }

        public void Discontinue()
        {
            if (Status == ProductStatus.Discontinued)
                return;
            Status = ProductStatus.Discontinued;
        }

    }
}
