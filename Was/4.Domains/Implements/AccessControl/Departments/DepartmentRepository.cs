using System.Data.Entity;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.Departments;

namespace Kengic.Was.Domain.AccessControl.Departments
{
    public class DepartmentRepository : RepositoryForOnlyDb<string, Department>, IDepartmentRepository
    {
        public DepartmentRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Department GetWithAll(string id)
        {
            if (id != null)
            {
                return GetSet().Where(r => r.Id == id).Include(r => r.Company)
                    .Include(r => r.Personnels).FirstOrDefault();
            }
            return null;
        }
    }
}