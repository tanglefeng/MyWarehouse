using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Model.TypeConfiguration.WorkTasks;

namespace Kengic.Was.Model.TypeConfiguration.Sorters
{
    public class SorterSubWorkTaskEntityTypeConfiguration
        : SubWorkTaskEntityTypeConfiguration<SorterSubWorkTask>
    {
        public SorterSubWorkTaskEntityTypeConfiguration()
        {
            var indexAttribute = new IndexAttribute { IsUnique = false };
            var indexAnnotation = new IndexAnnotation(indexAttribute);
            Property(t => t.Status)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);
            Property(t => t.CreateTime)
                .HasColumnAnnotation(IndexAnnotation.AnnotationName, indexAnnotation);

            Property(t => t.ScannerBarcode).HasMaxLength(256);
            Property(t => t.LogicBarcode).HasMaxLength(256);
            Property(t => t.AtricleBarcode).HasMaxLength(256);
            Property(t => t.AtricleLength).HasMaxLength(256);
            Property(t => t.AtricleWidth).HasMaxLength(256);
            Property(t => t.AtricleHeight).HasMaxLength(256);
            Property(t => t.AtricleProfile).HasMaxLength(256);
            Property(t => t.NodeId).HasMaxLength(256);
            Property(t => t.PhysicalSortter).HasMaxLength(256);
            Property(t => t.LogicalSortter).HasMaxLength(256);
            Property(t => t.Induct).HasMaxLength(256);
            Property(t => t.RequestShuteCode).HasMaxLength(256);
            Property(t => t.RequestShuteAddr).HasMaxLength(256);
            Property(t => t.RequestShuteAddrX).HasMaxLength(256);
            Property(t => t.RequestShuteAddrY).HasMaxLength(256);
            Property(t => t.RequestShuteAddrZ).HasMaxLength(256);
            Property(t => t.CurrentShuteCode).HasMaxLength(256);
            Property(t => t.CurrentShuteAddr).HasMaxLength(256);
            Property(t => t.CurrentShuteAddrX).HasMaxLength(256);
            Property(t => t.CurrentShuteAddrY).HasMaxLength(256);
            Property(t => t.CurrentShuteAddrZ).HasMaxLength(256);
            Property(t => t.FinalShuteCode).HasMaxLength(256);
            Property(t => t.FinalShuteAddr).HasMaxLength(256);
            Property(t => t.FinalShuteAddrX).HasMaxLength(256);
            Property(t => t.FinalShuteAddrY).HasMaxLength(256);
            Property(t => t.FinalShuteAddrZ).HasMaxLength(256);
            Property(t => t.SortResultCode).HasMaxLength(256);
            Property(t => t.SortResultSorter).HasMaxLength(256);
            Property(t => t.SotringErrorInfo).HasMaxLength(256);
        }
    }
}