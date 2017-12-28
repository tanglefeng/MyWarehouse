using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.Personnels;

namespace Kengic.Was.Application.Services.AccessControl.Personnels
{
    public interface IPersonnelApplicationServices :
        IEditApplicationService<Personnel>, IQueryApplicationService<Personnel>
    {
    }
}