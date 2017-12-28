using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.Shutes;

namespace Kengic.Was.Application.Services.Sorter.Shutes
{
    public interface IShuteApplicationService : IEditApplicationService<Shute>, IQueryApplicationService<Shute>
    {
    }
}