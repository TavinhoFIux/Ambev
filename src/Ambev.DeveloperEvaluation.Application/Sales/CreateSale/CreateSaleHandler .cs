using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    /// <summary>
    /// Handler for processing <see cref="CreateSaleCommand"/> requests.
    /// </summary>
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, CreateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ISaleEventPublisher _saleEventPublisher;

        public CreateSaleHandler(
            ISaleRepository saleRepository,
            IMapper mapper,
            ISaleEventPublisher saleEventPublisher)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _saleEventPublisher = saleEventPublisher;
        }

        public async Task<CreateSaleResult> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CreateSaleCommandValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new SaleValidationException(validationResult.Errors);

            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = command.SaleNumber,
                SaleDate = command.SaleDate,
                CustomerId = command.CustomerId,
                CustomerName = command.CustomerName,
                BranchId = command.BranchId,
                BranchName = command.BranchName
            };

            foreach (var item in command.Items)
            {
                var saleItem = new SaleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice
                };

                saleItem.CalculateDiscount();
                sale.AddItem(saleItem);
            }

            await _saleRepository.CreateAsync(sale, cancellationToken);

            await _saleEventPublisher.PublishAsync(new SaleCreatedEvent
            {
                SaleId = sale.Id,
                CustomerName = sale.CustomerName
            }, cancellationToken);

            return new CreateSaleResult { Id = sale.Id };
        }
    }

}
