using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AlarmEvent
{
    public interface IAlarmEventTypeRepository : IRepositoryForOnlyDb<string, AlarmEventType>
    {
    }
}