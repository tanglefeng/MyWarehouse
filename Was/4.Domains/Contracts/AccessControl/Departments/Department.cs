using System.Collections.Generic;
using Kengic.Was.Domain.Entity.AccessControl.Companys;
using Kengic.Was.Domain.Entity.AccessControl.Personnels;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Departments
{
    public class Department : EntityForTime<string>
    {
        public int DepartmentDegree { get; set; }
        public virtual Department Parent { get; set; }
        public virtual ICollection<Personnel> Personnels { get; set; }
        public virtual Company Company { get; set; }
        public string CompanyId { get; set; }
        public string ParentId { get; set; }
    }
}