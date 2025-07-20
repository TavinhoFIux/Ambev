using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Domain.Repositories
{
    /// <summary>
    /// Defines the contract for managing sales data in the data store.
    /// </summary>
    public interface ISaleRepository
    {
        /// <summary>
        /// Persists a new sale in the data store.
        /// </summary>
        /// <param name="sale">The sale entity to be created.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task CreateAsync(Sale sale, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a sale by its unique identifier, including its associated items.
        /// </summary>
        /// <param name="id">The unique identifier of the sale.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>The sale entity if found; otherwise, null.</returns>
        Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Retrieves a paginated list of sales from the database with sorting.
        /// </summary>
        /// <param name="pageNumber">Page number.</param>
        /// <param name="pageSize">Number of records per page.</param>
        /// <param name="sortColumn">Column to sort by.</param>
        /// <param name="sortOrder">asc or desc.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>Paginated list of sales.</returns>
        Task<PaginatedList<Sale>> GetAllAsync(int pageNumber, int pageSize, string? sortColumn, string? sortOrder, CancellationToken cancellationToken);

        /// <summary>
        /// Updates an existing sale in the data store.
        /// </summary>
        /// <param name="sale">The sale entity with updated information.</param>
        /// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous update operation.</returns>
        Task UpdateSaleItemsAsync(Guid saleId, List<SaleItem> newItems, CancellationToken cancellationToken);

        Task UpdateSaleAsync(Sale sale, CancellationToken cancellationToken);

        Task DeleteAsync(Sale sale, CancellationToken cancellationToken);
    }
}
