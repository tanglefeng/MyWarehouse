using System.Collections.Generic;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Terminals
{
    public class WorkgroupDto : EntityForTimeDto<string>
    {
        public bool IsChecked { get; set; }
        public IList<ForeignUser> Users { get; set; }
        public IList<ForeignTerminal> Terminals { get; set; }
    }
}