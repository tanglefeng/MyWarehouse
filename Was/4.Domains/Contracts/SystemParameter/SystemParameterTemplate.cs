using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.SystemParameter
{
    public class SystemParameterTemplate : EntityForTime<string>
    {
        public string Value { get; set; }
    }
}