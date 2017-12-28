using System.Collections.Generic;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Personnels
{
    public class DepartmentDto : EntityForTimeDto<string>
    {
        public int DepartmentDegree { get; set; }
        public string ParentId { get; set; }
        public string ParentDepartmentName { get; set; }
        public string CompanyId { get; set; }
        public string CompanyName { get; set; }
        public List<DepartmentDto> Children { get; set; }
    }
}