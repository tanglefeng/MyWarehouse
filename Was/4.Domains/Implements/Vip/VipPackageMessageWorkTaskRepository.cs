using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Vip;

namespace Kengic.Was.Domain.Vip
{
    public class VipPackageMessageWorkTaskRepository : RepositoryForOnlyDb<string, VipPackageMessageWorkTask>,
        IVipPackageMessageWorkTaskRepository
    {
        public VipPackageMessageWorkTaskRepository(IQueryableUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}