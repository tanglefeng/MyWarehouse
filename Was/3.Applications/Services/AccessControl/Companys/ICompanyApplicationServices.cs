using System.Collections.Generic;
using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.Companys;

namespace Kengic.Was.Application.Services.AccessControl.Companys
{
    public interface ICompanyApplicationServices :
        IEditApplicationService<Company>, IQueryApplicationService<Company>
    {
        IEnumerable<Company> GetChildrenValue(Company company);
    }
}