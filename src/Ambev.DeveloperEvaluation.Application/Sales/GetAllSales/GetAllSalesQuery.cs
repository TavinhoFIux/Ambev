using Ambev.DeveloperEvaluation.Domain.Common;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    /// <summary>
    /// Query to get a paginated list of sales.
    /// </summary>
    public class GetAllSalesQuery : IRequest<PaginatedList<GetAllSalesResult>>
    {
        /// <summary>
        /// Page number to retrieve.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Number of items per page.
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Column to sort by.
        /// </summary>
        public string? SortColumn { get; set; }

        /// <summary>
        /// Sorting direction (asc or desc).
        /// </summary>
        public string? SortOrder { get; set; }
    }
}
