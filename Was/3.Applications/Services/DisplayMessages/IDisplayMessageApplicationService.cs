using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.DisplayMessage;

namespace Kengic.Was.Application.Services.DisplayMessages
{
    public interface IDisplayMessageApplicationService :
        IEditApplicationService<DisplayMessage>, IQueryApplicationService<DisplayMessage>
    {
    }
}