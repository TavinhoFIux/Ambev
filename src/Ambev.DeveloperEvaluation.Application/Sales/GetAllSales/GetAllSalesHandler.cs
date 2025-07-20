using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
    // <summary>
    /// Handles the retrieval of a paginated list of sales from the data store.
    /// </summary>
    public class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, PaginatedList<GetAllSalesResult>>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="GetAllSalesHandler"/> class.
        /// </summary>
        /// <param name="saleRepository">The repository for accessing sales data.</param>
        /// <param name="mapper">The AutoMapper instance for mapping entities to DTOs.</param>
        public GetAllSalesHandler(ISaleRepository saleRepository, IMapper mapper)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles the GetAllSalesQuery request and returns a paginated list of sales.
        /// </summary>
        /// <param name="request">The query with pagination and filter parameters.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A paginated list of sales mapped to GetAllSalesResult.</returns>
        public async Task<PaginatedList<GetAllSalesResult>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
        {
            var paginatedSales = await _saleRepository.GetAllAsync(
                 request.PageNumber,
                 request.PageSize,
                 request.SortColumn,
                 request.SortOrder,
                 cancellationToken
            );

            var mapped = paginatedSales.Map(sale => _mapper.Map<GetAllSalesResult>(sale));
            return mapped;
        }
    }
}
