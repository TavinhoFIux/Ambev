using Ambev.DeveloperEvaluation.Application.Sales.DeleteSale;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Bogus;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class DeleteSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly DeleteSaleHandler _handler;

        private readonly Faker _faker = new("pt_BR");

        public DeleteSaleHandlerTests()
        {
            _handler = new DeleteSaleHandler(_saleRepository);
        }

        [Fact]
        public async Task Should_Delete_Sale_Successfully()
        {
            // Arrange
            var saleId = Guid.NewGuid();

            var sale = new Sale
            {
                Id = saleId,
                CustomerName = _faker.Name.FullName(),
                SaleNumber = "S-999",
                CustomerId = _faker.Random.Guid().ToString(),
                BranchId = "BR-123",
                BranchName = "Filial RJ",
                SaleDate = DateTime.UtcNow
            };

            var command = new DeleteSaleCommand { Id = saleId };

            _saleRepository.GetByIdAsync(saleId, Arg.Any<CancellationToken>()).Returns(sale);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Success.Should().BeTrue();
            await _saleRepository.Received(1).DeleteAsync(sale, Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Should_Throw_SaleNotFoundException_When_Sale_Not_Found()
        {
            // Arrange
            var command = new DeleteSaleCommand { Id = Guid.NewGuid() };

            _saleRepository.GetByIdAsync(command.Id, Arg.Any<CancellationToken>())
                .Returns((Sale?)null);

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<SaleNotFoundException>()
                .WithMessage($"Venda com ID '{command.Id}' não encontrada.");
        }

        [Fact]
        public async Task Should_Throw_SaleValidationException_When_Command_Id_Is_Empty()
        {
            // Arrange
            var command = new DeleteSaleCommand { Id = Guid.Empty };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            var exception = await act.Should().ThrowAsync<SaleValidationException>();
            exception.Which.Errors.Should().Contain(e => e.PropertyName == "Id");
        }
    }
}
