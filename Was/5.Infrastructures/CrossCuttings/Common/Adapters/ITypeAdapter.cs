using System.Linq;

namespace Kengic.Was.CrossCutting.Common.Adapters
{
    /// <summary>
    ///     Base contract for map dto to aggregate or aggregate to dto.
    ///     <remarks>
    ///         This is a contract for work with "auto" mappers (
    ///         automapper,emitmapper,valueinjecter...) or adhoc mappers
    ///     </remarks>
    /// </summary>
    public interface ITypeAdapter
    {
        /// <summary>
        ///     object to an
        ///     instance of type <paramref name="source" />
        /// </summary>
        /// <typeparam name="TSource">
        ///     Type of <paramref name="source" /> item
        /// </typeparam>
        /// <typeparam name="TTarget">Type of target item</typeparam>
        /// <param name="source">Instance to adapt</param>
        /// <returns>
        ///     <paramref name="source" /> mapped to <typeparamref name="TTarget" />
        /// </returns>
        TTarget Adapt<TSource, TTarget>(TSource source)
            where TTarget : class, new()
            where TSource : class;

        /// <summary>
        ///     object to an
        ///     instnace of type <paramref name="source" />
        /// </summary>
        /// <typeparam name="TTarget">Type of target item</typeparam>
        /// <param name="source">Instance to adapt</param>
        /// <returns>
        ///     <paramref name="source" /> mapped to <typeparamref name="TTarget" />
        /// </returns>
        TTarget Adapt<TTarget>(object source)
            where TTarget : class, new();

        IQueryable<TTarget> Adapt<TSource, TTarget>(IQueryable<TSource> source)
            where TTarget : class, new()
            where TSource : class;
    }
}