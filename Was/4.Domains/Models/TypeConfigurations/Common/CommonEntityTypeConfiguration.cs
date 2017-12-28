using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Model.TypeConfiguration.Commons
{
    public class CommonEntityTypeConfiguration<TEntity>
        : EntityTypeConfiguration<TEntity> where TEntity : Entity<string>
    {
        public CommonEntityTypeConfiguration()
        {
            Property(t => t.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(t => t.Name).HasMaxLength(256);
            Property(t => t.Code).HasMaxLength(256);
            Property(t => t.Description).HasMaxLength(1024);
        }
    }
}