using System;
using System.Linq;

namespace Kengic.Was.CrossCutting.LinqToWcf.Communications
{
    /// <summary>
    ///     Interface providing methods to get <see cref="T:System.Linq.IQueryable`1" />.
    /// </summary>
    public interface IQueryHandler
    {
        /// <summary>
        ///     Returns an <see cref="T:System.Linq.IQueryable" />.
        /// </summary>
        /// <param name="type">Type of the returned <see cref="T:System.Linq.IQueryable" />.</param>
        /// <returns>Returns an <see cref="T:System.Linq.IQueryable" />.</returns>
        IQueryable Get(Type type);

        /// <summary>
        ///     Returns an <see cref="T:System.Linq.IQueryable`1" />.
        /// </summary>
        /// <typeparam name="T">Generic Argument of the returned <see cref="T:System.Linq.IQueryable`1" />.</typeparam>
        /// <returns>Returns an <see cref="T:System.Linq.IQueryable`1" />.</returns>
        IQueryable<T> Get<T>() where T : class;
    }
}