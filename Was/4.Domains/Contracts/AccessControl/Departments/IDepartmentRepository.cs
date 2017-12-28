using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Departments
{
    public interface IDepartmentRepository : IRepositoryForOnlyDb<string, Department>
    {
        Department GetWithAll(string id);
    }
}