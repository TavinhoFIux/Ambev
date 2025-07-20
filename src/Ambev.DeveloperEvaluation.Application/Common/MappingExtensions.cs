using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.Application.Common
{
    public static class MappingExtensions
    {
        public static TResult Map<TResult>(this object source, IMapper mapper)
            => mapper.Map<TResult>(source);
    }
}
