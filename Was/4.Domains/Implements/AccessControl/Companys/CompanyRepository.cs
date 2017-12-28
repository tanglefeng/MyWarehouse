using System.Data.Entity;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.Companys;

namespace Kengic.Was.Domain.AccessControl.Companys
{
    public class CompanyRepository : RepositoryForOnlyDb<string, Company>, ICompanyRepository
    {
        public CompanyRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Company GetWithAll(string id)
            => id != null ? GetSet().Where(r => r.Id == id).Include(r => r.Departments).FirstOrDefault() : null;
    }
}