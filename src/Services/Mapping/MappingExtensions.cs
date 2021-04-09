using System;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;

namespace Services.Mapping
{
    public static class QueryableMappingExtensions
    {
        /// <summary>
        /// Map source data to specified type using AutoMapper
        /// </summary>
        /// <typeparam name="TDestination">The type to map to</typeparam>
        /// <param name="source">The querryable data set to map from</param>
        /// <param name="membersToExpand">Members to expand</param>
        /// <returns>Mapped collection of type <typeparamref name="TDestination"/></returns>
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

        /// <summary>
        /// Map source data to specified type using AutoMapper
        /// </summary>
        /// <typeparam name="TDestination">The type to map to</typeparam>
        /// <param name="source">The querryable data set to map from</param>
        /// <param name="parameters">Mapping parameters</param>
        /// <returns>Mapped collection of type <typeparamref name="TDestination"/></returns>
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
