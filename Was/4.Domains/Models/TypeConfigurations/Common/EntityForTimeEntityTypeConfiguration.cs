using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Model.TypeConfiguration.Commons
{
    public class EntityForTimeEntityTypeConfiguration<TEntity>
        : CommonEntityTypeConfiguration<TEntity> where TEntity : EntityForTime<string>
    {
        public EntityForTimeEntityTypeConfiguration()
        {
            Property(t => t.CreateBy).HasMaxLength(256);
            Property(t => t.UpdateBy).HasMaxLength(256);
        }
    }
}