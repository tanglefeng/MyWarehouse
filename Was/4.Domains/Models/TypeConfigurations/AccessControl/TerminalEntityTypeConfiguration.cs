using Kengic.Was.Domain.Entity.AccessControl.Terminals;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AccessControls
{
    public class TerminalEntityTypeConfiguration : EntityForTimeEntityTypeConfiguration<Terminal>
    {
        public TerminalEntityTypeConfiguration()
        {
            Property(t => t.TerminalIp).HasMaxLength(256);
        }
    }
}