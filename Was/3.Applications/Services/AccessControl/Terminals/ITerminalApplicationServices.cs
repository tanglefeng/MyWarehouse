using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.Terminals;

namespace Kengic.Was.Application.Services.AccessControl.Terminals
{
    public interface ITerminalApplicationServices :
        IEditApplicationService<Terminal>, IQueryApplicationService<Terminal>
    {
    }
}