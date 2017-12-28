using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Model.TypeConfiguration.WorkTasks;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    internal class SorterExecuteWorkTaskEntityTypeConfiguration
        : ExecuteWorkTaskEntityTypeConfiguration<SorterExecuteWorkTask>
    {
        public SorterExecuteWorkTaskEntityTypeConfiguration()
        {
            var indexAttribute = new IndexAttribute {IsUnique = false};
            var indexAnnotation = new IndexAnnotation(indexAttribute);
            Property(t => t.ObjectToHandle)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
            Property(t => t.LogisticsBarcode)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
            Property(t => t.Wave).HasMaxLength(256);
            Property(t => t.Order).HasMaxLength(256);
            Property(t => t.Shipment).HasMaxLength(256);
            Property(t => t.LogisticsBarcode).HasMaxLength(256);
            Property(t => t.ArticalBarcode).HasMaxLength(256);
            Property(t => t.LogicalDestination).HasMaxLength(256);
        }
    }
}