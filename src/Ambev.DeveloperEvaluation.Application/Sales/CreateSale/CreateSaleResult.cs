namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Represents the result of a successful sale creation operation.
    /// </summary>
    /// <remarks>
    /// This result contains the unique identifier of the newly created sale,
    /// which can be used for further reference or retrieval.
    /// </remarks>
    public class CreateSaleResult
    {
        /// <summary>
        /// Gets or sets the unique identifier of the created sale.
        /// </summary>
        /// <value>A GUID representing the ID of the new sale.</value>
        public Guid Id { get; set; }
    }
}
