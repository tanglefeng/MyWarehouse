using System.Collections.Generic;

namespace Kengic.Was.Domain.Entity.Common
{
    public interface IRepositoryForSyncDb<in TKey, TEntity> : IEditRepository<TKey, TEntity>,
        IQueryRepository<TKey, TEntity>
        where TEntity : Entity<TKey>
    {
        string LogName { get; set; }
        void Load();
        IEnumerable<TEntity> GetValuesFromMemory();
    }
}