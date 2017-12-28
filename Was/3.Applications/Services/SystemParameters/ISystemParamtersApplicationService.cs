using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.SystemParameter;

namespace Kengic.Was.Application.Services.SystemParameters
{
    public interface ISystemParamtersApplicationService :
        IEditApplicationService<SystemParameter>, IQueryApplicationService<SystemParameter>
    {
    }
}