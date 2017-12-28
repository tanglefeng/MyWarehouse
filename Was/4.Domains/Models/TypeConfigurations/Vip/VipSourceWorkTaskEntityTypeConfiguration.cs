using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Kengic.Was.Domain.Entity.Vip;
using Kengic.Was.Model.TypeConfiguration.WorkTasks;

namespace Kengic.Was.Model.TypeConfiguration.Vips
{
    public class VipSourceWorkTaskEntityTypeConfiguration
        : SourceWorkTaskEntityTypeConfiguration<VipSourceWorkTask>
    {
        public VipSourceWorkTaskEntityTypeConfiguration()
        {
            var indexAttribute = new IndexAttribute {IsUnique = false};
            var indexAnnotation = new IndexAnnotation(indexAttribute);
            Property(t => t.Status)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
        }
    }
}