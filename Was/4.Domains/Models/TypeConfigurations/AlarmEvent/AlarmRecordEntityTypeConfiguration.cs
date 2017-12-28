using Kengic.Was.Domain.Entity.AlarmEvent;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.AlarmEvents
{
    public class AlarmRecordEntityTypeConfiguration
        : EntityForTimeEntityTypeConfiguration<AlarmEventRecord>
    {
        public AlarmRecordEntityTypeConfiguration()
        {
            //HasRequired 是必须填写，HasOptional是可以为空，配置AlarmType和AlarmRecord一对多的对应关系
            //AlarmRecord表有一个外键：AlarmTypeId
            //HasOptional(e => e.Type)
            //    .WithMany(s => s.AlarmRecords).HasForeignKey(r => r.AlarmTypeId);
            Property(t => t.Source).HasMaxLength(256);
            Property(t => t.Object).HasMaxLength(256);
            Property(t => t.Comments).HasMaxLength(256);
            Property(t => t.Type).HasMaxLength(256);
        }
    }
}