using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kengic.Was.CrossCutting.LinqToWcf.QueryProvider;

namespace Kengic.Was.CrossCutting.LinqToWcf.Communications
{
    /// <summary>
    ///     Abstract implementation of an <see cref="IQueryHandler" />.
    ///     This class provides methods to get an <see cref="IQueryable{T}" />.
    /// </summary>
    /// <seealso cref="IQueryHandler" />
    public abstract class InterLinqQueryHandler : IQueryHandler
    {
        protected static MethodInfo GetTableMethodWithoutPara;
        protected static MethodInfo GetTableMethod;

        private readonly Dictionary<Type, MethodInfo> _genericMethodsCache1 = new Dictionary<Type, MethodInfo>();

        private readonly Dictionary<Type, MethodInfo> _genericMethodsCache2 = new Dictionary<Type, MethodInfo>();

        static InterLinqQueryHandler()
        {
            GetTableMethodWithoutPara = typeof (InterLinqQueryHandler).GetMethod("Get", new Type[] {});
            GetTableMethod = typeof (InterLinqQueryHandler).GetMethod("Get",
                new[] {typeof (object), typeof (string), typeof (object), typeof (object[])});
        }

        /// <summary>
        ///     Gets the <see cref="IQueryProvider" />.
        /// </summary>
        public abstract IQueryProvider QueryProvider { get; }

        /// <summary>
        ///     Returns a <see cref="IQueryable{T}" />
        /// </summary>
        /// <param name="type">Generic Argument of the returned <see cref="IQueryable{T}" />.</param>
        /// <param name="name">The name of the query.</param>
        /// <param name="parameters">Parameters for the quey.</param>
        /// <returns>Returns a <see cref="IQueryable{T}" />.</returns>
        public virtual IQueryable Get(Type type)
        {
            MethodInfo genericGetTableMethod;

            if (!_genericMethodsCache2.TryGetValue(type, out genericGetTableMethod))
            {
                genericGetTableMethod = GetTableMethod.MakeGenericMethod(type);
                _genericMethodsCache2.Add(type, genericGetTableMethod);
            }

            return
                (IQueryable)
                    genericGetTableMethod.Invoke(this, new object[] {});
        }

        /// <summary>
        /// </summary>
        /// <typeparam name="T">The entity used on <see cref="IQueryable{T}" /></typeparam>
        /// <param name="name">The name of the query.</param>
        /// <param name="parameters">Parameters for the quey.</param>
        /// <returns>Returns a <see cref="IQueryable{T}" />.</returns>
        public virtual IQueryable<T> Get<T>() where T : class => new InterLinqQuery<T>(QueryProvider, null);

        ///// <summary>
        /////     Returns an <see cref="IQueryable{T}" />.
        ///// </summary>
        ///// <param name="type">Type of the returned <see cref="IQueryable{T}" />.</param>
        ///// <returns>Returns an <see cref="IQueryable{T}" />.</returns>
        //public IQueryable Get(Type type)
        //{

        //    MethodInfo genericGetTableMethod;

        //    if (!_genericMethodsCache1.TryGetValue(type, out genericGetTableMethod))
        //    {
        //        genericGetTableMethod = GetTableMethodWithoutPara.MakeGenericMethod(type);
        //        _genericMethodsCache1.Add(type, genericGetTableMethod);
        //    }
        //    return (IQueryable) genericGetTableMethod.Invoke(this, new object[] {});
        //}

        ///// <summary>
        /////     Returns an <see cref="IQueryable{T}" />.
        ///// </summary>
        ///// <typeparam name="T">Generic Argument of the returned <see cref="IQueryable{T}" />.</typeparam>
        ///// <returns>Returns an <see cref="IQueryable{T}" />.</returns>
        //public IQueryable<T> Get<T>() where T : class => new InterLinqQuery<T>(QueryProvider);
    }
}