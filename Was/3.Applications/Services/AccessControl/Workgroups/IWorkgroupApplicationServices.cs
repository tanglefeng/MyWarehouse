using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.Workgroups;

namespace Kengic.Was.Application.Services.AccessControl.Workgroups
{
    public interface IWorkgroupApplicationServices :
        IEditApplicationService<Workgroup>, IQueryApplicationService<Workgroup>
    {
    }
}