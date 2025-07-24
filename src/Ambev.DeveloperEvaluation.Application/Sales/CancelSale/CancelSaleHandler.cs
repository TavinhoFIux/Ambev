using Ambev.DeveloperEvaluation.Application.Sales.Events;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.CancelSale
{
    public class CancelSaleHandler : IRequestHandler<CancelSaleCommand, CancelSaleResult>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ISaleEventPublisher _saleEventPublisher;

        public CancelSaleHandler(ISaleRepository saleRepository, ISaleEventPublisher saleEventPublisher)
        {
            _saleRepository = saleRepository;
            _saleEventPublisher = saleEventPublisher;
        }

        public async Task<CancelSaleResult> Handle(CancelSaleCommand command, CancellationToken cancellationToken)
        {
            var validator = new CancelSaleValidator();
            var validationResult = await validator.ValidateAsync(command, cancellationToken);

            if (!validationResult.IsValid)
                throw new SaleValidationException(validationResult.Errors);

            var sale = await _saleRepository.GetByIdAsync(command.Id, cancellationToken);

            if (sale is null)
                throw new SaleNotFoundException(command.Id);

            if (sale.IsCancelled)
                throw new SaleAlreadyCancelledException(command.Id);

            sale.Cancel();

            foreach (var item in sale.Items)
                item.IsCancelled = true;

            await _saleRepository.UpdateSaleAsync(sale, cancellationToken);

            await _saleEventPublisher.PublishAsync(new SaleItemCancelledEvent
            {
                SaleId = sale.Id,
            }, cancellationToken);

            await _saleEventPublisher.PublishAsync(new SaleCancelledEvent
            {
                SaleId = sale.Id,
            }, cancellationToken);

            return new CancelSaleResult { Success = true };
        }
    }

}
