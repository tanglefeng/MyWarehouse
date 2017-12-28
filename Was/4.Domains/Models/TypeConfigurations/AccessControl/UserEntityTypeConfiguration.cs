using Kengic.Was.Domain.Entity.AccessControl.Users;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls

{
    public class UserEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<User>
    {
        public UserEntityTypeConfiguration()
        {
            HasMany(s => s.Roles)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("RoleId");
                    cs.ToTable("UserRole");
                });
            HasMany(s => s.Terminals)
                .WithMany(c => c.Users)
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("TerminalId");
                    cs.ToTable("UserTerminal");
                });
            HasMany(s => s.FunctionPrivileges)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("FunctionPrivilegeId");
                    cs.ToTable("UserFunctionPrivilege");
                });
            HasMany(s => s.Workgroups)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("UserId");
                    cs.MapRightKey("WorkgroupId");
                    cs.ToTable("UserWorkgroup");
                });

            HasRequired(e => e.Personnel)
                .WithOptional(s => s.User);


            Property(t => t.LastAccessIp).HasMaxLength(256);
            Property(t => t.TerminalStartIpAddress).HasMaxLength(256);
            Property(t => t.TerminalEndIpAddress).HasMaxLength(256);
            Property(t => t.TerminalIpAddress).HasMaxLength(256);
            Property(t => t.PersonnelId).HasMaxLength(256);
        }
    }
}