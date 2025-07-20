using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class SaleEventLoggerPublisher : ISaleEventPublisher
    {
        private readonly ILogger<SaleEventLoggerPublisher> _logger;

        public SaleEventLoggerPublisher(ILogger<SaleEventLoggerPublisher> logger)
        {
            _logger = logger;
        }

        public Task PublishAsync(SaleEventBase saleEvent, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Evento publicado: {EventType}, VendaId: {SaleId}, Data: {Date}",
                saleEvent.EventType, saleEvent.SaleId, saleEvent.OccurredAt);

            return Task.CompletedTask;
        }
    }
}
