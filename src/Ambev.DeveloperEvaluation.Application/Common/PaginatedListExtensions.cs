using Ambev.DeveloperEvaluation.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Common
{
    public static class PaginatedListExtensions
    {
        public static PaginatedList<TDestination> Map<TSource, TDestination>(
            this PaginatedList<TSource> source,
            Func<TSource, TDestination> mapFunc)
        {
            var mappedItems = source.Select(mapFunc).ToList();

            return new PaginatedList<TDestination>(
                mappedItems,
                source.TotalCount,
                source.CurrentPage,
                source.PageSize
            );
        }
    }
}
