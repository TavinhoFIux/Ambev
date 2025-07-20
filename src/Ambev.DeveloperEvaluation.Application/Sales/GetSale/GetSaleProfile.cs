using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Profile for mapping entities related to sale retrieval operations.
    /// </summary>
    public class GetSaleProfile : Profile
    {
        /// <summary>
        /// Initializes the mappings for GetSale operations.
        /// </summary>
        public GetSaleProfile()
        {
            CreateMap<Sale, GetSaleResult>()
                .ForMember(dest => dest.TotalValue, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.Customer, opt => opt.MapFrom(src => src.CustomerName))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.SaleDate))
                .ForMember(dest => dest.Branch, opt => opt.MapFrom(src => src.BranchName));

            CreateMap<SaleItem, GetSaleItemResult>()
                  .ForMember(dest => dest.TotalItem, opt => opt.MapFrom(src => src.Quantity))
                       .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.ProductName)); 
        }
    }
}
