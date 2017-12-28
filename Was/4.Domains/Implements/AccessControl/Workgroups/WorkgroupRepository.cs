using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.Workgroups;

namespace Kengic.Was.Domain.AccessControl.Workgroups
{
    public class WorkgroupRepository : RepositoryForOnlyDb<string, Workgroup>, IWorkgroupRepository
    {
        public WorkgroupRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}