using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Executor
{
    public class WasExecutor : EntityForTime<string>
    {
        public string ExecuteOperator { get; set; }
        public string Connection { get; set; }
        public string CurrentAddress { get; set; }
    }
}