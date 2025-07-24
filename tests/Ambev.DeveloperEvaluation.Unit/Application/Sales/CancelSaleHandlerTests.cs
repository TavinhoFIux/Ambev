using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{

    public class CancelSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly ISaleEventPublisher _saleEventPublisher = Substitute.For<ISaleEventPublisher>();
        private readonly CancelSaleHandler _handler;
        private readonly Faker _faker = new("pt_BR");

        public CancelSaleHandlerTests()
        {
            _handler = new CancelSaleHandler(_saleRepository, _saleEventPublisher);
        }

        [Fact]
        public async Task Should_Cancel_Sale_Successfully()
        {
            // Arrange
            var saleId = Guid.NewGuid();

            var sale = new Sale
            {
                Id = saleId,
                CustomerName = _faker.Name.FullName(),
                SaleDate = DateTime.Now,
                SaleNumber = "S-001",
                CustomerId = _faker.Random.Guid().ToString(),
                BranchId = "BR-001",
                BranchName = "Unidade Central",
                TotalAmount = 200
            };

            var item = new SaleItem
            {
                Id = Guid.NewGuid(),
                ProductId = "PROD-123",
                ProductName = "Produto Teste",
                Quantity = 2,
                UnitPrice = 100,
                SaleId = saleId
            };

            sale.Items.Add(item);

            var command = new CancelSaleCommand { Id = saleId };

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>())
                .Returns(sale);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            sale.IsCancelled.Should().BeTrue();
            sale.Items.All(i => i.IsCancelled).Should().BeTrue();

            await _saleRepository.Received(1).UpdateSaleAsync(sale, Arg.Any<CancellationToken>());

            await _saleEventPublisher.Received(1).PublishAsync(
                Arg.Is<SaleItemCancelledEvent>(e => e.SaleId == saleId),
                Arg.Any<CancellationToken>());

            await _saleEventPublisher.Received(1).PublishAsync(
                Arg.Is<SaleCancelledEvent>(e => e.SaleId == saleId),
                Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Should_Throw_SaleNotFoundException_When_Sale_Does_Not_Exist()
        {
            // Arrange
            var command = new CancelSaleCommand { Id = Guid.NewGuid() };

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns((Sale?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<SaleNotFoundException>()
                .WithMessage($"Venda com ID '{command.Id}' não encontrada.");
        }

        [Fact]
        public async Task Should_Throw_SaleAlreadyCancelledException_When_Sale_Is_Cancelled()
        {
            // Arrange
            var command = new CancelSaleCommand { Id = Guid.NewGuid() };

            var cancelledSale = new Sale
            {
                Id = command.Id,
                CustomerName = _faker.Name.FullName(),
                SaleDate = DateTime.Now,
                SaleNumber = "S-002",
                CustomerId = _faker.Random.Guid().ToString(),
                BranchId = "BR-002",
                BranchName = "Unidade Oeste",
                TotalAmount = 150
            };
            cancelledSale.Cancel();

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns(cancelledSale);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<SaleAlreadyCancelledException>()
                .WithMessage($"A venda com ID '{command.Id}' já está cancelada.");
        }

        [Fact]
        public async Task Should_Throw_SaleValidationException_When_Id_Is_Empty()
        {
            // Arrange
            var command = new CancelSaleCommand { Id = Guid.Empty };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            var exception = await act.Should().ThrowAsync<SaleValidationException>();
            exception.Which.Errors.Should().Contain(e => e.PropertyName == "Id");
        }
    }

}
