using System.Collections.Generic;

namespace Kengic.Was.Application.WasModel.Dto.Sorters.LogicalDestinations
{
    public class LogicalDestinationDto : EntityForTimeDto<string>
    {
        public string ParentId { get; set; }
        public string DisplayName { get; set; }
        public bool IsEnable { get; set; }
        public bool IsActive { get; set; }
        public List<LogicalDestinationDto> Children { get; set; }
    }
}