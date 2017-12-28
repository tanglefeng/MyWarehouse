using System;
using System.Collections.Generic;
using Kengic.Was.Domain.Entity.AccessControl.FunctionPrivileges;
using Kengic.Was.Domain.Entity.AccessControl.Passwords;
using Kengic.Was.Domain.Entity.AccessControl.Personnels;
using Kengic.Was.Domain.Entity.AccessControl.Roles;
using Kengic.Was.Domain.Entity.AccessControl.Terminals;
using Kengic.Was.Domain.Entity.AccessControl.Workgroups;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Users
{
    public class User : EntityForTime<string>
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
        public virtual ICollection<Role> Roles { get; set; }

        public virtual Personnel Personnel { get; set; }
        public virtual ICollection<FunctionPrivilege> FunctionPrivileges { get; set; }
        public virtual ICollection<Terminal> Terminals { get; set; }
        public virtual ICollection<Workgroup> Workgroups { get; set; }
        public virtual ICollection<Password> Passwords { get; set; }
        public string PersonnelId { get; set; }
        public string PhotoLink { get; set; }
    }

    public enum UserStatus
    {
        Enabled,
        Disabled
    }
}