using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Validator for GetSaleRequest.
    /// Ensures that the provided sale ID is valid.
    /// </summary>
    public class GetSaleValidator : AbstractValidator<GetSaleQuery>
    {
        public GetSaleValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("The sale ID must not be empty.")
                .NotEqual(Guid.Empty).WithMessage("The sale ID must be a valid GUID.");
        }
    }
}
