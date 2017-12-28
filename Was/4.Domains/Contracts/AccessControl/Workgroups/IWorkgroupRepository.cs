using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Workgroups
{
    public interface IWorkgroupRepository : IRepositoryForOnlyDb<string, Workgroup>
    {
    }
}