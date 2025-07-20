using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetSale
{
    /// <summary>
    /// AutoMapper profile for mapping GetSale objects.
    /// </summary>
    public class GetSaleProfile : Profile
    {
        public GetSaleProfile()
        {
            CreateMap<GetSaleRequest, GetSaleQuery>();
            CreateMap<GetSaleResult, GetSaleResponse>();
            CreateMap<GetSaleItemResult, GetSaleItemResponse>();


            CreateMap<Sale, GetSaleResponse>()
           .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.CustomerName))
           .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.BranchName))
           .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount));

            CreateMap<SaleItem, GetSaleItemResponse>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.TotalItem, opt => opt.MapFrom(src => src.TotalPrice));
        }
    }
}
