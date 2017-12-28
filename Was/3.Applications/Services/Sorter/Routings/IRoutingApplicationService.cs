using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.Routings;

namespace Kengic.Was.Application.Services.Sorter.Routings
{
    public interface IRoutingApplicationService : IEditApplicationService<Routing>, IQueryApplicationService<Routing>
    {
    }
}