using Ambev.DeveloperEvaluation.Common.Validation;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.GetSale
{
    /// <summary>
    /// Query for retrieving a sale by ID.
    /// </summary>
    /// <remarks>
    /// This query is used to fetch the details of a sale using its unique identifier.
    /// It implements <see cref="IRequest{TResponse}"/> to trigger a request and return
    /// a <see cref="GetSaleResult"/> as the response.
    /// 
    /// The input is validated through <see cref="GetSaleValidator"/>.
    /// </remarks>
    public class GetSaleQuery : IRequest<GetSaleResult>
    {
        /// <summary>
        /// Gets or sets the unique identifier of the sale.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Validates the query.
        /// </summary>
        public ValidationResultDetail Validate()
        {
            var validator = new GetSaleValidator();
            var result = validator.Validate(this);
            return new ValidationResultDetail
            {
                IsValid = result.IsValid,
                Errors = result.Errors.Select(error => (ValidationErrorDetail)error)
            };
        }
    }
}
