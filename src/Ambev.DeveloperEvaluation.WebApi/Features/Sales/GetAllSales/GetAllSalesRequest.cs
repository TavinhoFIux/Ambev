namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales
{
    /// <summary>
    /// Request model for retrieving a paginated list of sales.
    /// </summary>
    public class GetAllSalesRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
    }
}
