using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales
{
    /// <summary>
    /// AutoMapper profile for mapping GetAllSalesResult to GetAllSalesResponse.
    /// </summary>
    public class GetAllSalesProfile : Profile
    {
        public GetAllSalesProfile()
        {
            CreateMap<GetAllSalesResult, GetAllSalesResponse>();
        }
    }
}
