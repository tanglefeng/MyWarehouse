using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.Sorter.Parameters;

namespace Kengic.Was.Application.Services.Sorter.SorterParameters
{
    public interface ISorterParameterApplicationService :
        IEditApplicationService<SorterParameter>, IQueryApplicationService<SorterParameter>
    {
    }
}