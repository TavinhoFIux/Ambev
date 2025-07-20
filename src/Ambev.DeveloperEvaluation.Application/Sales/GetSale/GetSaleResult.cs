using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    public class GetSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the sale number.
        /// </summary>
        public string SaleNumber { get; set; }

        public string Customer { get; set; }

        /// <summary>
        /// Gets or sets the branch where the sale occurred.
        /// </summary>
        public string Branch { get; set; }
        /// <summary>
        /// Gets or sets the total value of the sale.
        /// </summary>
        public decimal TotalValue { get; set; }

        /// <summary>
        /// Gets or sets the sale date.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the payment method.
        /// </summary>
        public int PaymentMethod { get; set; }

        /// <summary>
        /// Gets or sets whether the sale is cancelled.
        /// </summary>
        public bool IsCancelled { get; set; }

        /// <summary>
        /// Gets or sets the sale items associated with this sale.
        /// </summary>
        public List<GetSaleItemResult> Items { get; set; } = new();
    }


    public class GetSaleItemResult
    {
        /// <summary>
        /// Gets or sets the product identifier.
        /// </summary>
        public string Product { get; set; }


        /// <summary>
        /// Gets or sets the quantity sold.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price of the product.
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the discount value applied to this item.
        /// </summary>
        public decimal Discount { get; set; }

        /// <summary>
        /// Gets or sets the total amount for this item.
        /// </summary>
        public decimal TotalItem { get; set; }
    }

}
