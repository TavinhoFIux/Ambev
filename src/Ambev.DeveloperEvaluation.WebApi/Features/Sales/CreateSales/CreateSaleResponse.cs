namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSales
{
    /// <summary>
    /// Represents the response returned after successfully creating a sale.
    /// </summary>
    public class CreateSaleResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the created sale.
        /// </summary>
        public Guid Id { get; set; }
    }
}
