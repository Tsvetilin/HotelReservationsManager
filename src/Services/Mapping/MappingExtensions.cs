using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;

namespace Services.Mapping
{
    public static class QueryableMappingExtensions
    {
        public static IQueryable<TDestination> ProjectTo<TDestination>(
            this IQueryable source,
            params Expression<Func<TDestination, object>>[] membersToExpand)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo(MappingConfig.Instance.ConfigurationProvider, null, membersToExpand);
        }

        public static IQueryable<TDestination> ProjectTo<TDestination>(
            this IQueryable source,
            object parameters)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            return source.ProjectTo<TDestination>(MappingConfig.Instance.ConfigurationProvider, parameters);
        }
    }
}
