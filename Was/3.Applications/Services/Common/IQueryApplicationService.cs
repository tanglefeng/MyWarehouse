using System.Linq;

namespace Kengic.Was.Application.Services.Common
{
    public interface IQueryApplicationService<out T>
    {
        IQueryable<T> GetAll();
    }
}