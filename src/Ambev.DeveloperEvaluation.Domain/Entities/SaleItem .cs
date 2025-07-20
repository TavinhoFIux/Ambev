using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a single item in a sale, including pricing and discount.
    /// </summary>
    public class SaleItem : BaseEntity
    {
        /// <summary>
        /// External product reference.
        /// </summary>
        public string ProductId { get; set; } = null!;

        /// <summary>
        /// Product description or name.
        /// </summary>
        public string ProductName { get; set; } = null!;

        /// <summary>
        /// Quantity of the product sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Total discount applied to this item.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Indicates whether this item was cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Total price after discount.
        /// </summary>
        public decimal TotalPrice => (UnitPrice * Quantity) - Discount;

        /// <summary>
        /// Applies discount based on quantity rules.
        /// </summary>
        public void CalculateDiscount()
        {
            if (Quantity > 20)
                throw new InvalidOperationException("Cannot sell more than 20 units.");

            if (Quantity >= 10)
                Discount = UnitPrice * Quantity * 0.20m;
            else if (Quantity >= 4)
                Discount = UnitPrice * Quantity * 0.10m;
            else
                Discount = 0;
        }

        /// <summary>
        /// Cancels this sale item.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
        }
    }
}
