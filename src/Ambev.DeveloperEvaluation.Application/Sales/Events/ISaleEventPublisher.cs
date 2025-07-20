using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public interface ISaleEventPublisher
    {
        Task PublishAsync(SaleEventBase saleEvent, CancellationToken cancellationToken = default);
    }

}
