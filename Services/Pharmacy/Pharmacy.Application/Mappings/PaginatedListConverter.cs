using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pharmacy.Application.Wrapper;

namespace Pharmacy.Application.Mappings
{
    public class PaginatedListConverter<TSource, TDestination> : ITypeConverter<PaginatedList<TSource>, PaginatedList<TDestination>>
    {
        private readonly IMapper _mapper;

        public PaginatedListConverter(IMapper mapper)
        {
            _mapper = mapper;
        }

        public PaginatedList<TDestination> Convert(PaginatedList<TSource> source, PaginatedList<TDestination> destination, ResolutionContext context)
        {
            var mappedItems = _mapper.Map<List<TDestination>>(source.Items);
            return new PaginatedList<TDestination>(mappedItems, source.TotalCount, source.TotalPages, source.PageIndex);
        }
    }

}