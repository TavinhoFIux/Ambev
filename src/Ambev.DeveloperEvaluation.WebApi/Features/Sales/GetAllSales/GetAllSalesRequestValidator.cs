using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.GetAllSales
{
    /// <summary>
    /// Validator for the GetAllSalesRequest.
    /// </summary>
    public class GetAllSalesRequestValidator : AbstractValidator<GetAllSalesRequest>
    {
        public GetAllSalesRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
            RuleFor(x => x.SortOrder).Must(x => x == null || x.ToLower() is "asc" or "desc");
        }
    }
}
