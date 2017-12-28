using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AlarmEvent;

namespace Kengic.Was.Domain.AlarmEvent
{
    public class AlarmEventTypeRepository : RepositoryForOnlyDb<string, AlarmEventType>, IAlarmEventTypeRepository
    {
        public AlarmEventTypeRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}