using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.SystemTracing
{
    public class OperationTracing : EntityForTime<string>
    {
        public string User { get; set; }
        public string Source { get; set; }
        public string Operation { get; set; }
        public string Object { get; set; }
        public string ObjectValue { get; set; }
        public string Result { get; set; }
        public string Context { get; set; }
        public string Comments { get; set; }
    }
}