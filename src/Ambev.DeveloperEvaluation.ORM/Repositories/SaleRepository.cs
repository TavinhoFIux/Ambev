using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    /// <summary>
    /// Implements the <see cref="ISaleRepository"/> using Entity Framework Core.
    /// </summary>
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="SaleRepository"/> class.
        /// </summary>
        /// <param name="context">The EF Core database context.</param>
        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new sale in the database
        /// </summary>
        /// <param name="sale">The sale to create</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>The created sale</returns>
        public async Task CreateAsync(Sale sale, CancellationToken cancellationToken)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Retrieves a sale by its unique identifier, including its related items.
        /// </summary>
        /// <param name="id">The unique identifier of the sale.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>The sale entity if found; otherwise, null.</returns>
        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        /// <summary>
        /// Retrieves a paginated list of all sales, including their items,
        /// with optional sorting by a specified column and direction.
        /// </summary>
        /// <param name="pageNumber">The number of the page to retrieve.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="sortColumn">The name of the column to sort by.</param>
        /// <param name="sortOrder">The sort direction ("asc" for ascending, "desc" for descending).</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A paginated list of sales.</returns>
        public async Task<PaginatedList<Sale>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? sortColumn,
            string? sortOrder,
            CancellationToken cancellationToken)
        {
            var query = _context.Sales
                .Include(s => s.Items)
                .AsQueryable();

            if (!string.IsNullOrEmpty(sortColumn))
            {
                query = sortOrder?.ToLower() == "desc"
                    ? query.OrderByDescending(e => EF.Property<object>(e, sortColumn))
                    : query.OrderBy(e => EF.Property<object>(e, sortColumn));
            }

            return await PaginatedList<Sale>.CreateAsync(query, pageNumber, pageSize, cancellationToken);
        }

        /// <summary>
        /// Updates an existing sale in the database.
        /// </summary>
        /// <param name="sale">The sale entity with updated data.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken)
        {
            var existingSale = await _context.Sales
                .Include(s => s.Items)
                .FirstOrDefaultAsync(s => s.Id == sale.Id, cancellationToken);

            if (existingSale is null)
                throw new KeyNotFoundException($"Sale with ID {sale.Id} not found.");

            _context.SaleItems.RemoveRange(existingSale.Items);

            foreach (var item in sale.Items)
            {
                existingSale.Items.Add(new SaleItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.TotalPrice - (item.UnitPrice * item.Quantity),
                    IsCancelled = item.IsCancelled
                });
            }
            existingSale.CustomerName = sale.CustomerName;
            existingSale.TotalAmount = sale.TotalAmount;

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(Sale sale, CancellationToken cancellationToken)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
