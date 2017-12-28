using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Companys
{
    public interface ICompanyRepository : IRepositoryForOnlyDb<string, Company>
    {
        Company GetWithAll(string id);
    }
}