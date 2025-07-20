namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales
{
    /// <summary>
    /// Response model for a single sale returned in the paginated list.
    /// </summary>
    public class GetAllSalesResponse
    {
        public IEnumerable<GetSaleListItemResponse> Sales { get; set; } = Enumerable.Empty<GetSaleListItemResponse>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    public class GetSaleListItemResponse
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public string CustomerName { get; set; }
        public string BranchName { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
