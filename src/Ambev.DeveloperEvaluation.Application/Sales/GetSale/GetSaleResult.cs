using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Represents the result returned after successfully retrieving a sale.
    /// </summary>
    public class GetSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the customer identifier related to the sale.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the branch identifier where the sale occurred.
        /// </summary>
        public Guid BranchId { get; set; }

        /// <summary>
        /// Gets or sets the total value of the sale.
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Gets or sets the sale items associated with this sale.
        /// </summary>
        public List<GetSaleItemResult> Items { get; set; } = new();
    }

    /// <summary>
    /// Represents each item in the sale.
    /// </summary>
    public class GetSaleItemResult
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// Gets or sets the quantity sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }
    }
}
