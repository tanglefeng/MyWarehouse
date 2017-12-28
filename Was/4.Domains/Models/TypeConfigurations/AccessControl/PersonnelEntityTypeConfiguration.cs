using Kengic.Was.Domain.Entity.AccessControl.Personnels;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls
{
    public class PersonnelEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Personnel>
    {
        public PersonnelEntityTypeConfiguration()
        {
            //Department和Personnel是一对多的关系，Department可以为空，
            //Personnel表有一个外键：DepartmentId
            HasOptional(e => e.Department)
                .WithMany(s => s.Personnels).HasForeignKey(r => r.DepartmentId);

            Property(t => t.ChineseName).HasMaxLength(256);
            Property(t => t.EnglishName).HasMaxLength(256);
            Property(t => t.MobileNo).HasMaxLength(256);
            Property(t => t.PostAddress).HasMaxLength(256);
            Property(t => t.CompanyTelephone).HasMaxLength(256);
            Property(t => t.Comments).HasMaxLength(256);
            Property(t => t.PhotoLink).HasMaxLength(256);
            Property(t => t.DepartmentId).HasMaxLength(256);
        }
    }
}