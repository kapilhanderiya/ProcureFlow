using ProcureFlow.Domain.Common.Entities;
using ProcureFlow.Domain.Common.Exceptions;
using ProcureFlow.Domain.Common.Guards;
using ProcureFlow.Domain.Common.ValueObjects;

namespace ProcureFlow.Domain.PurchaseRequests
{
    public class PurchaseRequestItem : BaseEntity
    {
        public Guid ProductId { get; private set; }

        public string Description { get; private set; } = null!;

        public decimal Quantity { get; private set; }

        public Money UnitPrice { get; private set; } = null!;

        private PurchaseRequestItem()
        {
        }

        public PurchaseRequestItem(Guid productId, string description, decimal quantity, Money unitPrice)
        {
            if(productId == Guid.Empty)
            {
                throw new DomainException("Product ID is required.");
            }
            ProductId = productId;
            Description = DomainGuard.Required(description, "Product Description is required.");
            ChangeQuantity(quantity);
            ArgumentNullException.ThrowIfNull(unitPrice);
            UnitPrice = unitPrice;
        }

        public Money GetTotal()
        {
            return UnitPrice.Multiply(Quantity);
        }

        public void ChangeQuantity(decimal quantity)
        {
            if (quantity <= 0)
            {
                throw new DomainException("Item quantity must be greater than zero.");
            }
            Quantity = quantity;
        }
    }

}
