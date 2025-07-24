using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using AutoMapper;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.UpdateSale
{
    public class UpdateSaleHandler : IRequestHandler<UpdateSaleCommand, UpdateSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly IMapper _mapper;
        private readonly ISaleEventPublisher _saleEventPublisher;


        public UpdateSaleHandler(ISaleRepository saleRepository, IMapper mapper, ISaleEventPublisher saleEventPublisher)
        {
            _saleRepository = saleRepository;
            _mapper = mapper;
            _saleEventPublisher = saleEventPublisher;
        }

        public async Task<UpdateSaleResult> Handle(UpdateSaleCommand request, CancellationToken cancellationToken)
        {
            var validator = new UpdateSaleValidator();
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
                throw new SaleValidationException(validationResult.Errors);

            var existingSale = await _saleRepository.GetByIdAsync(request.Id, cancellationToken);
            if (existingSale is null)
                throw new SaleNotFoundException(request.Id);

            existingSale.CustomerName = request.CustomerName;

            var newItems = request.Items.Select(itemDto =>
            {
                var item = new SaleItem
                {
                    Id = Guid.NewGuid(),
                    SaleId = existingSale.Id,
                    ProductId = itemDto.ProductId,
                    ProductName = itemDto.ProductName,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.UnitPrice,
                    IsCancelled = false
                };

                item.CalculateDiscount();
                return item;
            }).ToList();

            existingSale.TotalAmount = newItems.Sum(i => i.TotalPrice);


            await _saleRepository.UpdateSaleAsync(existingSale, cancellationToken);

            await _saleRepository.UpdateSaleWithItemsAsync(existingSale, newItems, cancellationToken);


            await _saleEventPublisher.PublishAsync(new SaleUpdatedEvent
            {
                SaleId = existingSale.Id,
            }, cancellationToken);

            return new UpdateSaleResult
            {
                Id = existingSale.Id
            };
        }
    }
}
