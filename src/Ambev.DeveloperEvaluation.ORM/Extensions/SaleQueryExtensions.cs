using Ambev.DeveloperEvaluation.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Extensions
{
    public static class SaleQueryExtensions
    {
        public static IQueryable<Sale> ApplyOrdering(this IQueryable<Sale> query, string? column, string? order)
        {
            var sort = order?.ToLower() ?? "asc";

            return (column?.ToLower(), sort) switch
            {
                ("customername", "asc") => query.OrderBy(s => s.CustomerName),
                ("customername", "desc") => query.OrderByDescending(s => s.CustomerName),
                ("totalamount", "asc") => query.OrderBy(s => s.TotalAmount),
                ("totalamount", "desc") => query.OrderByDescending(s => s.TotalAmount),
                ("saledate", "asc") => query.OrderBy(s => s.SaleDate),
                ("saledate", "desc") => query.OrderByDescending(s => s.SaleDate),
                _ => query.OrderBy(s => s.Id),
            };
        }
    }
}
