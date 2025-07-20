using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    /// <summary>
    /// Result containing the list of sales and pagination metadata.
    /// </summary>
    public class GetAllSalesResult
    {
        public IEnumerable<GetSaleListItemResult> Sales { get; set; } = Enumerable.Empty<GetSaleListItemResult>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    public class GetSaleListItemResult
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
