using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.Sorters;

namespace Kengic.Was.Application.Services.Sorter.Sorters
{
    public interface ILogicalSorterApplicationService :
        IEditApplicationService<LogicalSorter>, IQueryApplicationService<LogicalSorter>

    {
        LogicalSorter GetValueByNodeId(string value);
    }
}