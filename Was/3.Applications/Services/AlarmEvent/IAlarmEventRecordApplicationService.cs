using System.Collections.Generic;
using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AlarmEvent;

namespace Kengic.Was.Application.Services.AlarmEvent
{
    public interface IAlarmEventRecordApplicationService :
        IEditApplicationService<AlarmEventRecord>, IQueryApplicationService<AlarmEventRecord>
    {
        IEnumerable<AlarmEventRecord> GetWithStatus();
        int GetDataActiveAlarmNumber();
    }
}