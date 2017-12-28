using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.Inducts;

namespace Kengic.Was.Application.Services.Sorter.Inducts
{
    public interface IInductApplicationService : IEditApplicationService<Induct>, IQueryApplicationService<Induct>
    {
    }
}