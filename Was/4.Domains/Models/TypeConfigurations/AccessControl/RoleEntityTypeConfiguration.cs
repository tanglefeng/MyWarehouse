using Kengic.Was.Domain.Entity.AccessControl.Roles;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls
{
    public class RoleEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Role>
    {
        public RoleEntityTypeConfiguration()
        {
            HasMany(s => s.FunctionPrivileges)
                .WithMany()
                .Map(cs =>
                {
                    cs.MapLeftKey("RoleRefId");
                    cs.MapRightKey("FunctionPrivilegeRefId");
                    cs.ToTable("RoleFunctionPrivilege");
                });
        }
    }
}