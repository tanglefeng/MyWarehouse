using System.Collections.Generic;
using Kengic.Was.Application.Services.Common;
using Kengic.Was.Domain.Entity.AccessControl.Departments;

namespace Kengic.Was.Application.Services.AccessControl.Departments
{
    public interface IDepartmentApplicationServices :
        IEditApplicationService<Department>, IQueryApplicationService<Department>
    {
        IEnumerable<Department> GetChildrenValue(Department department);
    }
}