using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls
{
    public class FunctionPrivilegeEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<FunctionPrivilege>
    {
        public FunctionPrivilegeEntityTypeConfiguration()
        {
            HasOptional(c => c.Parent).WithMany().HasForeignKey(c => c.ParentId);

            Property(t => t.FunObjCatalog).HasMaxLength(256);
            Property(t => t.AuthorizationMask).HasMaxLength(256);
            Property(t => t.ConditionExpression).HasMaxLength(256);
            Property(t => t.ParentId).HasMaxLength(256);
        }
    }
}