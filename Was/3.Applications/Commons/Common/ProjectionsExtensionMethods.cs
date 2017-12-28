using System.Collections.Generic;
using System.Linq;
using Kengic.Was.CrossCutting.Common.Adapters;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.Application.WasModel.Common
{
    public static class ProjectionsExtensionMethods
    {
        private static readonly ITypeAdapter Adapter = ServiceLocator.Current.GetInstance<ITypeAdapter>();

        /// <summary>
        ///     Project a type using a DTO
        /// </summary>
        /// <typeparam name="TProjection">The dto projection</typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="item">The source entity to project</param>
        /// <returns>
        ///     The projected type
        /// </returns>
        public static TProjection ProjectedAs<TSource, TProjection>(this TSource item)
            where TProjection : class, new() => Adapter.Adapt<TProjection>(item);

        /// <summary>
        ///     projected a enumerable collection of <paramref name="items" />
        /// </summary>
        /// <typeparam name="TProjection">The dtop projection type</typeparam>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="items">the collection of entity items</param>
        /// <returns>
        ///     Projected collection
        /// </returns>
        public static List<TProjection> ProjectedAsCollection<TSource, TProjection>(this IEnumerable<TSource> items)
            where TProjection : class, new() => Adapter.Adapt<List<TProjection>>(items);

        public static IQueryable<TProjection> ProjectedAsQueryable<TSource, TProjection>(this IQueryable<TSource> items)
            where TProjection : class, new() where TSource : class => Adapter.Adapt<TSource, TProjection>(items);
    }
}