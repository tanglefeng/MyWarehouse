using System.Collections.Generic;
using Kengic.Was.Domain.Entity.AccessControl.Users;
using Kengic.Was.Domain.Entity.AccessControl.Workgroups;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Terminals
{
    public class Terminal : EntityForTime<string>
    {
        public int TerminalType { get; set; }
        public int TerminalStatus { get; set; }
        public string TerminalIp { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Workgroup> Workgroups { get; set; }
    }
}