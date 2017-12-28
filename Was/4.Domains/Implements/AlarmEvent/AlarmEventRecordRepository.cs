using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AlarmEvent;

namespace Kengic.Was.Domain.AlarmEvent
{
    public class AlarmEventRecordRepository : RepositoryForOnlyDb<string, AlarmEventRecord>,
        IAlarmEventRecordRepository
    {
        public AlarmEventRecordRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public IEnumerable<AlarmEventRecord> GetWithStatus()
            => GetAll().Where(r => r.Status != AlarmEventStatus.Removed);
    }
}