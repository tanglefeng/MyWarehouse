using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.SystemSequence;

namespace Kengic.Was.Application.Services.SystemSequences
{
    public interface ISystemSequencesApplicationService :
        IEditApplicationService<SystemSequence>, IQueryApplicationService<SystemSequence>
    {
    }
}