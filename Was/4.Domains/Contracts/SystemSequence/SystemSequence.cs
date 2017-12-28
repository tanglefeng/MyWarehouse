using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.SystemSequence
{
    public class SystemSequence : EntityForTime<string>
    {
        public string Prefix { get; set; }
        public int Value { get; set; }
        public int MaxValue { get; set; }
        public int MinValue { get; set; }
        public int IncreaseRate { get; set; }
    }
}