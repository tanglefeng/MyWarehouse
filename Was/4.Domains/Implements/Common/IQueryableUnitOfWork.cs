using System.Collections.Generic;
using System.Data.Entity;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Common
{
    public interface IQueryableUnitOfWork
        : IUnitOfWork, ISql
    {
        bool AutoDetectChangesEnabled { get; set; }
        void SetPropertiesModified<TEntity>(TEntity item, List<string> propertyEntries) where TEntity : class;
        IDbSet<TEntity> CreateSet<TEntity>() where TEntity : class;
        void Attach<TEntity>(TEntity item) where TEntity : class;
        void SetModified<TEntity>(TEntity item) where TEntity : class;
        void SetDeleted<TEntity>(TEntity item) where TEntity : class;
        void SetDetached<TEntity>(TEntity item) where TEntity : class;
    }
}