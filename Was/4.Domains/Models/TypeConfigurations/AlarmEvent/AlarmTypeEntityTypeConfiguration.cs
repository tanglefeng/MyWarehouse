using Kengic.Was.Domain.Entity.AlarmEvent;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AlarmEvents
{
    public class AlarmTypeEntityTypeConfiguration
        : EntityForTimeEntityTypeConfiguration<AlarmEventType>
    {
        public AlarmTypeEntityTypeConfiguration()
        {
            Property(t => t.Comments).HasMaxLength(256);
        }
    }
}