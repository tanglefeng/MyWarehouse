using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AlarmEvent
{
    public interface IAlarmEventRecordRepository : IRepositoryForOnlyDb<string, AlarmEventRecord>
    {
        IEnumerable<AlarmEventRecord> GetWithStatus();
    }
}