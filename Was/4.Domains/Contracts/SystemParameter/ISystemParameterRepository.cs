using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.SystemParameter
{
    public interface ISystemParameterRepository : IRepositoryForSyncDb<string, SystemParameter>
    {
        List<SystemParameter> GetValueByTemplate(string value);
    }
}