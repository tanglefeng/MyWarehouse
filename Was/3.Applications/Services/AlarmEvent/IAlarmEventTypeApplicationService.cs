using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AlarmEvent;

namespace Kengic.Was.Application.Services.AlarmEvent
{
    public interface IAlarmEventTypeApplicationService :
        IEditApplicationService<AlarmEventType>, IQueryApplicationService<AlarmEventType>
    {
    }
}