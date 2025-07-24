using Ambev.DeveloperEvaluation.Application.Sales.GetAllSales;
using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class GetAllSalesHandlerTests
    {
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly GetAllSalesHandler _handler;
        private readonly Faker _faker = new("pt_BR");

        public GetAllSalesHandlerTests()
        {
            _handler = new GetAllSalesHandler(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Should_Return_Paginated_Sale_List()
        {
            // Arrange
            var fakeSales = new List<Sale>();
            for (int i = 0; i < 3; i++)
            {
                fakeSales.Add(new Sale
                {
                    Id = Guid.NewGuid(),
                    SaleNumber = $"S-{i + 1:000}",
                    CustomerName = _faker.Name.FullName(),
                    SaleDate = DateTime.Now.AddDays(-i),
                    CustomerId = Guid.NewGuid().ToString(),
                    BranchId = "BR-001",
                    BranchName = "Filial A"
                });
            }

            var paginatedSales = new PaginatedList<Sale>(
                fakeSales,
                count: 3,
                pageNumber: 1,
                pageSize: 10
            );

            _saleRepository.GetAllAsync(1, 10, null, null, Arg.Any<CancellationToken>())
                .Returns(paginatedSales);

            _mapper.Map<GetSaleListItemResult>(Arg.Any<Sale>())
                .Returns(call =>
                {
                    var sale = call.Arg<Sale>();
                    return new GetSaleListItemResult
                    {
                        Id = sale.Id,
                        SaleNumber = sale.SaleNumber,
                        CustomerName = sale.CustomerName
                    };
                });

            var request = new GetAllSalesQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SortColumn = null,
                SortOrder = null
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(3);
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task Should_Return_Empty_List_When_No_Sales()
        {
            // Arrange
            var emptyPaginated = new PaginatedList<Sale>(
                new List<Sale>(),
                count: 0,
                pageNumber: 1,
                pageSize: 10
            );

            _saleRepository.GetAllAsync(1, 10, null, null, Arg.Any<CancellationToken>())
                .Returns(emptyPaginated);

            var request = new GetAllSalesQuery
            {
                PageNumber = 1,
                PageSize = 10
            };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalCount.Should().Be(0);
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
        }
    }
}
