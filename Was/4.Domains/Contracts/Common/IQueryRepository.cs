using System;
using System.Linq;

namespace Kengic.Was.Domain.Entity.Common
{
    public interface IQueryRepository<in TKey, out TEntity> : IDisposable
        where TEntity : EntityBase<TKey>
    {
        TEntity TryGetValue(TKey id);
        IQueryable<TEntity> GetAll();
    }
}