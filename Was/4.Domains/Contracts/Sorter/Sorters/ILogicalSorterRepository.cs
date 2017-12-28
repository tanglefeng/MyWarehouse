using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Sorters
{
    public interface ILogicalSorterRepository : IRepositoryForSyncDb<string, LogicalSorter>
    {
        List<LogicalSorter> GetValueByPhysicalSorter(string value);
        LogicalSorter GetValueByNodeId(string value);
    }
}