using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.Events
{
    public class SaleCancelledEvent : SaleEventBase
    {
        public override string EventType => "SaleCancelled";
    }
}
