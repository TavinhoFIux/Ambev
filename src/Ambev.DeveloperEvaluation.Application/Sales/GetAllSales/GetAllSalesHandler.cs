using Ambev.DeveloperEvaluation.Application.Common;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetAllSales
{
        public class GetAllSalesHandler : IRequestHandler<GetAllSalesQuery, PaginatedList<GetSaleListItemResult>>
        {
            private readonly ISaleRepository _saleRepository;
            private readonly IMapper _mapper;

            public GetAllSalesHandler(ISaleRepository saleRepository, IMapper mapper)
            {
                _saleRepository = saleRepository;
                _mapper = mapper;
            }

            public async Task<PaginatedList<GetSaleListItemResult>> Handle(GetAllSalesQuery request, CancellationToken cancellationToken)
            {
                var paginatedSales = await _saleRepository.GetAllAsync(
                    request.PageNumber,
                    request.PageSize,
                    request.SortColumn,
                    request.SortOrder,
                    cancellationToken);

                var mappedItems = paginatedSales.Select(sale => _mapper.Map<GetSaleListItemResult>(sale)).ToList();

                return new PaginatedList<GetSaleListItemResult>(
                    mappedItems,
                    paginatedSales.TotalCount,
                    paginatedSales.CurrentPage,
                    paginatedSales.PageSize
                );
            }
        }
}
