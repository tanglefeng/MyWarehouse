using System.Data.Entity;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.Roles;

namespace Kengic.Was.Domain.AccessControl.Roles
{
    public class RoleRepository : RepositoryForOnlyDb<string, Role>, IRoleRepository
    {
        public RoleRepository(IQueryableUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public Role GetValues(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.FunctionPrivileges).FirstOrDefault() : null;
    }
}