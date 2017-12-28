using Kengic.Was.Domain.Entity.AccessControl.Departments;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls
{
    public class DepartmentEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Department>
    {
        public DepartmentEntityTypeConfiguration()
        {
            //Company和Departments是一对多的关系，Company可以为空
            //Department表有一个外键：CompanyId
            HasOptional(a => a.Company)
                .WithMany(s => s.Departments).HasForeignKey(r => r.CompanyId);
            HasOptional(c => c.Parent).WithMany().HasForeignKey(c => c.ParentId);
            Property(t => t.CompanyId).HasMaxLength(256);
            Property(t => t.ParentId).HasMaxLength(256);
        }
    }
}