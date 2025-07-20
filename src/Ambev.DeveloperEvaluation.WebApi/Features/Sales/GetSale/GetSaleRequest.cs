namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// Represents the request to retrieve a sale by its ID.
    /// </summary>
    public class GetSaleRequest
    {
        /// <summary>
        /// The unique identifier of the sale to be retrieved.
        /// </summary>
        public Guid Id { get; set; }
    }
}
