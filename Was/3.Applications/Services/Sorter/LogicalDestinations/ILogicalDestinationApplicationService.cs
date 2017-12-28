using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.LogicalDestinations;

namespace Kengic.Was.Application.Services.Sorter.LogicalDestinations
{
    public interface ILogicalDestinationApplicationService :
        IEditApplicationService<LogicalDestination>, IQueryApplicationService<LogicalDestination>
    {
    }
}