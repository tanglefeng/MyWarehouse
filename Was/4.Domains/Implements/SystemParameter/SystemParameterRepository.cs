using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.SystemParameter;

namespace Kengic.Was.Domain.SystemParameters
{
    public class SystemParameterRepository : RepositoryForSyncDb<string, SystemParameter>, ISystemParameterRepository
    {
        public SystemParameterRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public List<SystemParameter> GetValueByTemplate(string value)
            => ValueQueue.Values.Where(e => e.Template == value).ToList();
    }
}