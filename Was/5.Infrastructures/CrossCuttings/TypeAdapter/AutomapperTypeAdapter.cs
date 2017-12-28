using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kengic.Was.CrossCutting.Common.Adapters;

namespace Kengic.Was.CrossCutting.TypeAdapter
{
    /// <summary>
    ///     Automapper type adapter implementation
    /// </summary>
    public class AutomapperTypeAdapter
        : ITypeAdapter
    {
        /// <summary>
        ///     <see cref="ITypeAdapter" />
        /// </summary>
        /// <typeparam name="TSource">
        ///     <see cref="ITypeAdapter" />
        /// </typeparam>
        /// <typeparam name="TTarget">
        ///     <see cref="ITypeAdapter" />
        /// </typeparam>
        /// <param name="source">
        ///     <see cref="ITypeAdapter" />
        /// </param>
        /// <returns>
        ///     <see cref="ITypeAdapter" />
        /// </returns>
        public TTarget Adapt<TSource, TTarget>(TSource source)
            where TSource : class
            where TTarget : class, new() => Mapper.Map<TSource, TTarget>(source);

        /// <summary>
        ///     <see cref="ITypeAdapter" />
        /// </summary>
        /// <typeparam name="TTarget">
        ///     <see cref="ITypeAdapter" />
        /// </typeparam>
        /// <param name="source">
        ///     <see cref="ITypeAdapter" />
        /// </param>
        /// <returns>
        ///     <see cref="ITypeAdapter" />
        /// </returns>
        public TTarget Adapt<TTarget>(object source) where TTarget : class, new() => Mapper.Map<TTarget>(source);

        public IQueryable<TTarget> Adapt<TSource, TTarget>(IQueryable<TSource> source) where TSource : class
            where TTarget : class, new() => source.ProjectTo<TTarget>();
    }
}