using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    /// <summary>
    /// Query to get a paginated list of sales.
    /// </summary>
    public class GetAllSalesQuery : IRequest<PaginatedList<GetSaleListItemResult>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SortColumn { get; set; }
        public string? SortOrder { get; set; }
    }
}
