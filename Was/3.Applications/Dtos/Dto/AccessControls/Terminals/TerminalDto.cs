using System.Collections.Generic;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Terminals
{
    public class TerminalDto : EntityForTimeDto<string>
    {
        public int TerminalType { get; set; }
        public int TerminalStatus { get; set; }
        public string TerminalIp { get; set; }
        public bool IsChecked { get; set; }
        public IList<ForeignUser> Users { get; set; }
        public IList<ForeignWorkgroup> Workgroups { get; set; }
    }
}