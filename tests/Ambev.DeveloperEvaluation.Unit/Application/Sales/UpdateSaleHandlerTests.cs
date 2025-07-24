using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;
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
    public class UpdateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly ISaleEventPublisher _saleEventPublisher = Substitute.For<ISaleEventPublisher>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly UpdateSaleHandler _handler;
        private readonly Faker _faker = new("pt_BR");

        public UpdateSaleHandlerTests()
        {
            _handler = new UpdateSaleHandler(_saleRepository, _mapper, _saleEventPublisher);
        }

        [Fact]
        public async Task Should_Update_Sale_Successfully()
        {
            // Arrange
            var saleId = Guid.NewGuid();
            var existingSale = new Sale
            {
                Id = saleId,
                CustomerName = "Old Name",
                SaleDate = DateTime.UtcNow,
                SaleNumber = "S-999",
                BranchId = "BR-001",
                BranchName = "Filial A",
                CustomerId = Guid.NewGuid().ToString()
            };

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(existingSale);

            var command = new UpdateSaleCommand
            {
                Id = saleId,
                CustomerName = _faker.Name.FullName(),
                Items = new List<UpdateSaleItemDto>
            {
                new UpdateSaleItemDto
                {
                    Id = Guid.NewGuid(),
                    ProductId = "PROD-001",
                    ProductName = "Produto A",
                    Quantity = 2,
                    UnitPrice = 10,
                    IsCancelled = false
                }
            }
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(saleId);

            await _saleRepository.Received(1).UpdateSaleAsync(existingSale, Arg.Any<CancellationToken>());
            await _saleRepository.Received(1).UpdateSaleWithItemsAsync(existingSale, Arg.Any<List<SaleItem>>(), Arg.Any<CancellationToken>());

            await _saleEventPublisher.Received(1).PublishAsync(
                Arg.Is<SaleUpdatedEvent>(e => e.SaleId == saleId),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Should_Throw_SaleNotFoundException_When_Sale_Does_Not_Exist()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Id = Guid.NewGuid(),
                CustomerName = "Novo Cliente",
                Items = new List<UpdateSaleItemDto>()
            };

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns((Sale?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<SaleNotFoundException>();
        }

        [Fact]
        public async Task Should_Throw_SaleValidationException_When_Command_Is_Invalid()
        {
            // Arrange
            var command = new UpdateSaleCommand
            {
                Id = Guid.Empty,
                CustomerName = "",
                Items = new List<UpdateSaleItemDto>()
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            var exception = await act.Should().ThrowAsync<SaleValidationException>();
            exception.Which.Errors.Should().Contain(e => e.PropertyName == "Id");
            exception.Which.Errors.Should().Contain(e => e.PropertyName == "CustomerName");
        }
    }
}
