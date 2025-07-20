using Ambev.DeveloperEvaluation.Domain.Entities;
using AutoMapper;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// AutoMapper profile for the CreateSale use case.
    /// </summary>
    /// <remarks>
    /// Defines the mappings between the <see cref="CreateSaleCommand"/> and the <see cref="Sale"/> entity,
    /// as well as between <see cref="CreateSaleItemCommand"/> and <see cref="SaleItem"/>.
    /// Also maps <see cref="Sale"/> to <see cref="CreateSaleResult"/>.
    /// </remarks>
    public class CreateSaleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateSaleProfile"/> class.
        /// </summary>
        public CreateSaleProfile()
        {
            CreateMap<CreateSaleCommand, Sale>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.TotalAmount, opt => opt.Ignore())
                .ForMember(dest => dest.IsCancelled, opt => opt.Ignore());

            CreateMap<CreateSaleItemCommand, SaleItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Discount, opt => opt.Ignore())
                .ForMember(dest => dest.IsCancelled, opt => opt.Ignore());

            CreateMap<Sale, CreateSaleResult>();
        }
    }
}
