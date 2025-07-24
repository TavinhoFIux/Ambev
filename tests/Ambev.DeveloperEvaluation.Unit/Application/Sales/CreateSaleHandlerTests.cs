using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using Bogus;
using FluentAssertions;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Sales
{
    public class CreateSaleHandlerTests
    {
        private readonly ISaleRepository _saleRepository = Substitute.For<ISaleRepository>();
        private readonly ISaleEventPublisher _saleEventPublisher = Substitute.For<ISaleEventPublisher>();
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly CreateSaleHandler _handler;

        private readonly Faker _faker = new("pt_BR");

        public CreateSaleHandlerTests()
        {
            _handler = new CreateSaleHandler(_saleRepository, _mapper, _saleEventPublisher);
        }

        [Fact]
        public async Task Should_Create_Sale_Successfully()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "S-1001",
                SaleDate = DateTime.UtcNow,
                CustomerId = _faker.Random.Guid().ToString(),
                CustomerName = _faker.Name.FullName(),
                BranchId = "BR-001",
                BranchName = "Unidade Norte",
                Items = new List<CreateSaleItemCommand>
            {
                new CreateSaleItemCommand
                {
                    ProductId = "PROD-001",
                    ProductName = "Cerveja",
                    Quantity = 5,
                    UnitPrice = 10m
                }
            }
            };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Id.Should().NotBeEmpty();

            await _saleRepository.Received(1)
                .CreateAsync(Arg.Is<Sale>(s => s.SaleNumber == command.SaleNumber), Arg.Any<CancellationToken>());

            await _saleEventPublisher.Received(1)
                .PublishAsync(Arg.Is<SaleCreatedEvent>(e => e.CustomerName == command.CustomerName), Arg.Any<CancellationToken>());
        }

        [Fact]
        public async Task Should_Throw_SaleValidationException_When_Command_Is_Invalid()
        {
            // Arrange
            var command = new CreateSaleCommand
            {
                SaleNumber = "",
                SaleDate = default,
                CustomerId = "",
                CustomerName = "",
                BranchId = "",
                BranchName = "",
                Items = new List<CreateSaleItemCommand>() 
            };

            // Act
            Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

            // Assert
            var exception = await act.Should().ThrowAsync<SaleValidationException>();
            exception.Which.Errors.Should().NotBeEmpty();
            exception.Which.Errors.Should().Contain(e => e.PropertyName == "SaleNumber");
            exception.Which.Errors.Should().Contain(e => e.PropertyName == "CustomerName");
        }
    }
}
