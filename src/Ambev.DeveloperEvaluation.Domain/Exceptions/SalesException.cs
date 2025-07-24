using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Domain.Exceptions
{
    public class SaleNotFoundException : Exception
    {
        public SaleNotFoundException(Guid saleId)
            : base($"Venda com ID '{saleId}' não encontrada.") { }
    }

    public class SaleAlreadyCancelledException : Exception
    {
        public SaleAlreadyCancelledException(Guid saleId)
            : base($"A venda com ID '{saleId}' já está cancelada.") { }
    }

    public class SaleValidationException : Exception
    {
        public SaleValidationException(IEnumerable<ValidationFailure> errors)
            : base("A requisição para cancelar a venda é inválida.")
        {
            Errors = errors;
        }

        public IEnumerable<ValidationFailure> Errors { get; }
    }
}
