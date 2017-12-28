using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Package;

namespace Kengic.Was.Domain.Package
{
    public class PackageSourceWorkTaskRepository : RepositoryForOnlyDb<string, PackageSourceWorkTask>,
        IPackageSourceWorkTaskRepository
    {
        public PackageSourceWorkTaskRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}