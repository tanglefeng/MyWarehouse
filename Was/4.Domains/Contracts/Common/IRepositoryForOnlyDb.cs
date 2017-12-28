using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kengic.Was.Domain.Entity.Common
{
    /// <summary>
    ///     Base <see langword="interface" /> for implement a "Repository Pattern",
    ///     for more information about this pattern see
    ///     http://martinfowler.com/eaaCatalog/repository.html or
    ///     http://blogs.msdn.com/adonet/archive/2009/06/16/using-repository-and-unit-of-work-patterns-with-entity-framework-4-0.aspx
    /// </summary>
    /// <remarks>
    ///     Indeed, one might think that IDbSet already a generic repository and
    ///     therefore would not need this item. Using this
    ///     <see langword="interface" /> allows us to ensure PI principle within our
    ///     domain model
    /// </remarks>
    /// <typeparam name="TEntity">
    ///     <see cref="Type" /> of entity for this repository
    /// </typeparam>
    /// <typeparam name="TKey"></typeparam>
    public interface IRepositoryForOnlyDb<in TKey, TEntity> : IEditRepository<TKey, TEntity>,
        IQueryRepository<TKey, TEntity>
        where TEntity : EntityBase<TKey>
    {
        IUnitOfWork UnitOfWork { get; }
        string LogName { get; set; }

        /// <summary>
        ///     Track entity into this repository, really in UnitOfWork. In EF this
        ///     can be done with Attach and with Update in NH
        /// </summary>
        /// <param name="item">Item to attach</param>
        void TrackItem(TEntity item);

        /// <summary>
        ///     Get elements of type <typeparamref name="TEntity" /> in repository
        /// </summary>
        /// <param name="filter">Filter that each element do match</param>
        /// <returns>
        ///     <see cref="IEnumerable{T}" /> of selected elements
        /// </returns>
        IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter);
    }
}