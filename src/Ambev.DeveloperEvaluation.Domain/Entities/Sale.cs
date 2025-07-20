using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    /// <summary>
    /// Represents a sales transaction containing customer, branch, items, and totals.
    /// </summary>
    public class Sale : BaseEntity
    {
        /// <summary>
        /// Unique identifier string of the sale.
        /// </summary>
        public string SaleNumber { get; set; } = null!;

        /// <summary>
        /// Date and time the sale was created.
        /// </summary>
        public DateTime SaleDate { get; set; }

        /// <summary>
        /// External customer reference.
        /// </summary>
        public string CustomerId { get; set; } = null!;

        /// <summary>
        /// Customer full name.
        /// </summary>
        public string CustomerName { get; set; } = null!;

        /// <summary>
        /// External branch reference.
        /// </summary>
        public string BranchId { get; set; } = null!;

        /// <summary>
        /// Branch name.
        /// </summary>
        public string BranchName { get; set; } = null!;

        /// <summary>
        /// Total sale amount including discounts.
        /// </summary>
        public decimal TotalAmount { get;  set; }

        /// <summary>
        /// Indicates whether the sale was cancelled.
        /// </summary>
        public bool IsCancelled { get;  private set; }

        /// <summary>
        /// List of items in the sale.
        /// </summary>
        public List<SaleItem> Items { get; set; } = new();

        /// <summary>
        /// Cancels the entire sale.
        /// </summary>
        public void Cancel()
        {
            IsCancelled = true;
        }

        /// <summary>
        /// Adds a new item to the sale and recalculates the total.
        /// </summary>
        public void AddItem(SaleItem item)
        {
            item.CalculateDiscount();
            Items.Add(item);
            RecalculateTotal();
        }

        /// <summary>
        /// Recalculates the total sale value.
        /// </summary>
        public void RecalculateTotal()
        {
            TotalAmount = Items.Sum(i => i.TotalPrice);
        }
    }
}
