using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.SystemParameter;

namespace Kengic.Was.Application.Services.SystemParameters
{
    public interface ISystemParamtersTemplateApplicationService :
        IEditApplicationService<SystemParameterTemplate>, IQueryApplicationService<SystemParameterTemplate>
    {
    }
}