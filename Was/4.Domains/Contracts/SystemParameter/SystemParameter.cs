using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.SystemParameter
{
    public class SystemParameter : EntityForTime<string>
    {
        public string Value { get; set; }
        public string Template { get; set; }
    }
}