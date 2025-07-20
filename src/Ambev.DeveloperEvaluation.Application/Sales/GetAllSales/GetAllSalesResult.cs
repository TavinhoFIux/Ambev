using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    /// <summary>
    /// Result containing the list of sales and pagination metadata.
    /// </summary>
    public class GetAllSalesResult
    {
        public IEnumerable<Sale> Sales { get; set; } = Enumerable.Empty<Sale>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }
}
