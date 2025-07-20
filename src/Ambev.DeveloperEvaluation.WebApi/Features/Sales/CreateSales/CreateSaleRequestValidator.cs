using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales.CreateSales
{
    /// <summary>
    /// Validator for CreateSaleRequest
    /// </summary>
    public class CreateSaleRequestValidator : AbstractValidator<CreateSaleRequest>
    {
        public CreateSaleRequestValidator()
        {
            RuleFor(x => x.SaleDate)
                .NotEmpty().WithMessage("Sale date is required.")
                .Must(date => date.Kind == DateTimeKind.Utc)
                .WithMessage("Sale date must be in UTC format (e.g., 2025-07-20T15:00:00Z).");
            RuleFor(x => x.SaleNumber).NotEmpty().WithMessage("Sale number is required.");
            RuleFor(x => x.SaleDate).NotEmpty().WithMessage("Sale date is required.");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("Customer name is required.");
            RuleFor(x => x.BranchId).NotEmpty().WithMessage("Branch ID is required.");
            RuleFor(x => x.BranchName).NotEmpty().WithMessage("Branch name is required.");
            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("At least one item is required.")
                .ForEach(child =>
                {
                    child.SetValidator(new CreateSaleItemRequestValidator());
                });
        }
    }

    /// <summary>
    /// Validator for CreateSaleItemRequest
    /// </summary>
    public class CreateSaleItemRequestValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public CreateSaleItemRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.ProductName).NotEmpty().WithMessage("Product name is required.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than 0.");
        }
    }
}
