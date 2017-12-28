using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Vip;

namespace Kengic.Was.Domain.Vip
{
    public class VipSourceWorkTaskRepository : RepositoryForOnlyDb<string, VipSourceWorkTask>,
        IVipSourceWorkTaskRepository
    {
        public VipSourceWorkTaskRepository(IQueryableUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}