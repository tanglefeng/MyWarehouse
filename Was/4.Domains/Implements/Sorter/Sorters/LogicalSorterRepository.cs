using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Sorters;

namespace Kengic.Was.Domain.Sorter.Sorters
{
    public class LogicalSorterRepository : RepositoryForSyncDb<string, LogicalSorter>, ILogicalSorterRepository
    {
        public LogicalSorterRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public List<LogicalSorter> GetValueByPhysicalSorter(string value)
            => ValueQueue.Values.Where(e => e.PhycialSorter == value).ToList();

        public LogicalSorter GetValueByNodeId(string value)
        {
            var valueList = ValueQueue.Values.Where(e => e.NodeId == value).ToList();
            return valueList.Count <= 0 ? null : valueList[0];
        }
    }
}