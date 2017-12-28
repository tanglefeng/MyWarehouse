using Kengic.Was.Domain.Entity.AccessControl.Workgroups;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls
{
    public class WorkgroupEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Workgroup>
    {
        public WorkgroupEntityTypeConfiguration()
        {
            HasMany(s => s.Terminals)
                .WithMany(c => c.Workgroups)
                .Map(cs =>
                {
                    cs.MapLeftKey("WorkgroupId");
                    cs.MapRightKey("TerminalId");
                    cs.ToTable("WorkgroupTerminal");
                });
        }
    }
}