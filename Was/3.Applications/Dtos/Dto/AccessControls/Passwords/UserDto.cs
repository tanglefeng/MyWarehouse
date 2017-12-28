using System;
using System.Collections.Generic;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Privileges;
using Kengic.Was.Application.WasModel.Dto.AccessControls.Terminals;
using Kengic.Was.Domain.Entity.AccessControl.Users;

namespace Kengic.Was.Application.WasModel.Dto.AccessControls.Passwords
{
    public class UserDto : EntityForTimeDto<string>
    {
        public string LastAccessIp { get; set; }
        public DateTime LastAccessTimestamp { get; set; }
        public UserStatus UserStatus { get; set; }
        public int MaxPasswordReconNum { get; set; }
        public int TerminalMaxLoginNum { get; set; }
        public int LongestIdleTime { get; set; }
        public int TerminalLimit { get; set; }
        public string TerminalStartIpAddress { get; set; }
        public string TerminalEndIpAddress { get; set; }
        public string TerminalIpAddress { get; set; }
        public int IsManyLogin { get; set; }
        public IList<ForeignFunctionPrivilege> FunctionPrivileges { get; set; }
        public IList<ForeignRole> Roles { get; set; }
        public IList<ForeignTerminal> Terminals { get; set; }
        public IList<ForeignWorkgroup> Workgroups { get; set; }
        //public ForeignPersonnel Personnel { get; set; }
        public string PersonnelId { get; set; }
        public string PersonnelName { get; set; }
        public string PhotoLink { get; set; }
    }
}