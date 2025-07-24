using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Ambev.DeveloperEvaluation.ORM.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Repositories
{
    public class SaleRepository : ISaleRepository
    {
        private readonly DefaultContext _context;

        public SaleRepository(DefaultContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Sale sale, CancellationToken cancellationToken)
        {
            await _context.Sales.AddAsync(sale, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Sale?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(id, includeItems: true, cancellationToken);
        }

        public async Task<Sale?> GetByIdAsync(Guid id, bool includeItems, CancellationToken cancellationToken)
        {
            var query = _context.Sales.AsQueryable();

            if (includeItems)
                query = query.Include(s => s.Items);

            return await query.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
        }

        public async Task<PaginatedList<Sale>> GetAllAsync(
            int pageNumber,
            int pageSize,
            string? sortColumn,
            string? sortOrder,
            CancellationToken cancellationToken)
        {
            var query = _context.Sales.Include(s => s.Items).AsQueryable();

            query = query.ApplyOrdering(sortColumn, sortOrder);

            return await PaginatedList<Sale>.CreateAsync(query, pageNumber, pageSize, cancellationToken);
        }

        public async Task UpdateSaleAsync(Sale sale, CancellationToken cancellationToken)
        {
            var exists = await _context.Sales.AnyAsync(s => s.Id == sale.Id, cancellationToken);

            if (!exists)
                throw new SaleNotFoundException(sale.Id);

            _context.Sales.Update(sale);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateSaleWithItemsAsync(Sale sale, List<SaleItem> newItems, CancellationToken cancellationToken)
        {
            using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

            try
            {
                _context.Sales.Update(sale);

                var existingItems = await _context.SaleItems
                    .Where(i => i.SaleId == sale.Id)
                    .ToListAsync(cancellationToken);

                _context.SaleItems.RemoveRange(existingItems);

                foreach (var item in newItems)
                {
                    item.Id = Guid.NewGuid();
                    item.SaleId = sale.Id;
                }

                await _context.SaleItems.AddRangeAsync(newItems, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        public async Task DeleteAsync(Sale sale, CancellationToken cancellationToken)
        {
            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
