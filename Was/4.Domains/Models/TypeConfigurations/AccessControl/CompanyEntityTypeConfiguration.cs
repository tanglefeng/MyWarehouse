using Kengic.Was.Domain.Entity.AccessControl.Companys;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls
{
    public class CompanyEntityTypeConfiguration : CommonEntityTypeConfiguration<Company>
    {
        public CompanyEntityTypeConfiguration()
        {
            HasOptional(c => c.Parent).WithMany().HasForeignKey(c => c.ParentId);
            Property(t => t.Name).HasMaxLength(255);
            Property(t => t.Code).HasMaxLength(128);
            Property(t => t.Description).HasMaxLength(255);
            Property(t => t.CreateBy).HasMaxLength(128);
            Property(t => t.UpdateBy).HasMaxLength(128);
            Property(t => t.AddressDetail).HasMaxLength(255);
            Property(t => t.AddressCode).HasMaxLength(64);
            Property(t => t.Fax).HasMaxLength(64);
            Property(t => t.Telephone).HasMaxLength(32);
            Property(t => t.EmailAddress).HasMaxLength(64);
            Property(t => t.EmailCode).HasMaxLength(60);
            Property(t => t.PhotoLink).HasMaxLength(255);
        }
    }
}