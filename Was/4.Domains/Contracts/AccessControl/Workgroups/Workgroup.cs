using System.Collections.Generic;
using Kengic.Was.Domain.Entity.AccessControl.Terminals;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Workgroups
{
    public class Workgroup : EntityForTime<string>
    {
        public virtual ICollection<Terminal> Terminals { get; set; }
    }
}