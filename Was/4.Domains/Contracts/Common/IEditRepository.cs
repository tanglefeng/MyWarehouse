using System;

namespace Kengic.Was.Domain.Entity.Common
{
    public interface IEditRepository<in TKey, in TEntity>
        where TEntity : EntityBase<TKey>
    {
        Tuple<bool, string> Create(TEntity item);
        Tuple<bool, string> Remove(TEntity item);
        Tuple<bool, string> Update(TEntity item);
    }
}