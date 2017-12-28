﻿using System.Linq;

namespace Kengic.Was.CrossCutting.LinqToWcf
{
    /// <summary>
    ///     Client IQueryable extensions.
    /// </summary>
    public static class InterLinqExtensions
    {
        /// <summary>
        ///     Specifies the related objects to include in the query results.
        /// </summary>
        /// <typeparam name="T">Entity type for the query.</typeparam>
        /// <param name="query">The InterLinqQueryOfT to apply the include string.</param>
        /// <param name="include">The releated objects to include in the query result.</param>
        /// <returns>InterLinqQueryOfT with the Include parameter populated.</returns>
        public static IQueryable<T> Include<T>(this IQueryable<T> query, string include)
        {
            var interLinqQuery = query as InterLinqQuery<T>;

            if (interLinqQuery != null)
            {
                //interLinqQuery.Include = include;
            }

            return query;
        }
    }
}