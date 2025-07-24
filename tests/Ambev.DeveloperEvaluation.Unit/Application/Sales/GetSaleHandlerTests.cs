using Ambev.DeveloperEvaluation.Application.Sales.GetSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class GetSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly GetSaleHandler _handler;

        private readonly Faker _faker = new("pt_BR");

        public GetSaleHandlerTests()
        {
            _handler = new GetSaleHandler(_saleRepository, _mapper);
        }

        [Fact]
        public async Task Should_Return_Sale_When_Exists()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var sale = new Sale
            {
                Id = saleId,
                SaleNumber = "S-321",
                CustomerName = _faker.Name.FullName(),
                CustomerId = Guid.NewGuid().ToString(),
                BranchId = "BR-02",
                BranchName = "Filial Oeste",
                SaleDate = DateTime.UtcNow
            };

            var expectedResult = new GetSaleResult
            {
                Id = saleId,
                SaleNumber = sale.SaleNumber,
                Customer = sale.CustomerName
            };

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);
            _mapper.Map<GetSaleResult>(sale).Returns(expectedResult);

            var request = new GetSaleQuery { Id = saleId };

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(saleId);
            result.Customer.Should().Be(sale.CustomerName);

            await _saleRepository.Received(1).GetByIdAsync(saleId, Arg.Any<CancellationToken>());
            _mapper.Received(1).Map<GetSaleResult>(sale);
        }

        [Fact]
        public async Task Should_Throw_SaleNotFoundException_When_Sale_Does_Not_Exist()
        {
            // Arrange
            var request = new GetSaleQuery { Id = Guid.NewGuid() };

            _saleRepository.GetByIdAsync(request.Id, Arg.Any<CancellationToken>())
                .Returns((Sale?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<SaleNotFoundException>()
                .WithMessage($"Venda com ID '{request.Id}' não encontrada.");
        }

        [Fact]
        public async Task Should_Throw_SaleValidationException_When_Id_Is_Empty()
        {
            // Arrange
            var request = new GetSaleQuery { Id = Guid.Empty };

            // Act
            Func<Task> act = async () => await _handler.Handle(request, CancellationToken.None);

            // Assert
            var exception = await act.Should().ThrowAsync<SaleValidationException>();
            exception.Which.Errors.Should().Contain(e => e.PropertyName == "Id");
        }
    }
}
