using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.SystemParameter;

namespace Kengic.Was.Domain.SystemParameters
{
    public class SystemParameterTemplateRepository : RepositoryForSyncDb<string, SystemParameterTemplate>,
        ISystemParameterTemplateRepository
    {
        public SystemParameterTemplateRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }
    }
}