using System.ComponentModel.DataAnnotations.Schema;
using Kengic.Was.Domain.Entity.Sorter.Routings;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    internal class RoutingEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Routing>
    {
        public RoutingEntityTypeConfiguration()
        {
            //key and properties
            HasKey(t => t.Id).Property(t => t.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(t => t.Name).HasMaxLength(255);
            Property(t => t.Code).HasMaxLength(128);
            Property(t => t.Description).HasMaxLength(255);
            Property(t => t.CreateBy).HasMaxLength(128);
            Property(t => t.UpdateBy).HasMaxLength(128);

            Property(t => t.SorterPlan).HasMaxLength(128);
            Property(t => t.PhycialShute).HasMaxLength(128);
            Property(t => t.LogicalDestination).HasMaxLength(128);
        }
    }
}