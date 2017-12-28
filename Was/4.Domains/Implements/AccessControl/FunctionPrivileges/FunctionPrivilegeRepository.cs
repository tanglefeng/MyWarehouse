using System.Data.Entity;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;

namespace Kengic.Was.Domain.AccessControl.FunctionPrivileges
{
    public class FunctionPrivilegeRepository : RepositoryForOnlyDb<string, FunctionPrivilege>,
        IFunctionPrivilegeRepository
    {
        public FunctionPrivilegeRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public FunctionPrivilege GetWithParentFunctionPrivilege(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.ParentId).FirstOrDefault() : null;
    }
}