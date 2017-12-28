using Kengic.Was.Domain.Entity.AccessControl.Passwords;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls
{
    public class PasswordEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Password>
    {
        public PasswordEntityTypeConfiguration()
        {
            HasOptional(e => e.User)
                .WithMany(s => s.Passwords).HasForeignKey(r => r.UserId);

            Property(t => t.HashCode).HasMaxLength(256);
            Property(t => t.PasswordDefine).HasMaxLength(256);
        }
    }
}