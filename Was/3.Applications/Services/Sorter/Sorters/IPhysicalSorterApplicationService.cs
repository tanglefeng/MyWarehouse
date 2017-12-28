using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.Sorters;

namespace Kengic.Was.Application.Services.Sorter.Sorters
{
    public interface IPhysicalSorterApplicationService :
        IEditApplicationService<PhysicalSorter>, IQueryApplicationService<PhysicalSorter>
    {
    }
}