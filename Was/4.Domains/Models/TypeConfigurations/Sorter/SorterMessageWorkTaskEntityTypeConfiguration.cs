using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Model.TypeConfiguration.WorkTasks;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    public class SorterMessageWorkTaskEntityTypeConfiguration
        : WorkTaskEntityTypeConfiguration<SorterMessageWorkTask>
    {
        public SorterMessageWorkTaskEntityTypeConfiguration()
        {
            var indexAttribute = new IndexAttribute {IsUnique = false};
            var indexAnnotation = new IndexAnnotation(indexAttribute);
            Property(t => t.Status)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
            Property(t => t.TrackingId)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
            Property(t => t.CreateTime)
               .HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);

            Property(t => t.Induct).HasMaxLength(256);
        }
    }
}