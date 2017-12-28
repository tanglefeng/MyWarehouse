using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.Scanners;

namespace Kengic.Was.Application.Services.Sorter.Scanners
{
    public interface IScannerApplicationService : IEditApplicationService<Scanner>, IQueryApplicationService<Scanner>
    {
    }
}