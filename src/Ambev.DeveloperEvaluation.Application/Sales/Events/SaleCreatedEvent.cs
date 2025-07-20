using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class SaleCreatedEvent : SaleEventBase
    {
        public string CustomerName { get; set; } = string.Empty;
        public override string EventType => "VendaCriada";
    }
}
