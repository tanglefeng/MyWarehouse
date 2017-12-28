using System;

namespace Kengic.Was.Domain.Entity.Common
{
    public abstract class Entity<TKey> : EntityBase<TKey>
    {
        public virtual string Name { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
    }

    public abstract class EntityBase<TKey>
    {
        public virtual TKey Id { get; set; }
    }

    public abstract class EntityForTime<TKey> : Entity<TKey>
    {
        public virtual string CreateBy { get; set; }
        public virtual DateTime? CreateTime { get; set; }
        public virtual string UpdateBy { get; set; }
        public virtual DateTime? UpdateTime { get; set; }
    }
}