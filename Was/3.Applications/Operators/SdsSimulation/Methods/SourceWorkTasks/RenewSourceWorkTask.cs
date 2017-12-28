using System;

namespace Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks
{
    public class RenewSourceWorkTask : CommonExecuteMethod
    {
        public RenewSourceWorkTask()
        {
            Id = "RenewSdsSimulationSourceWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => RenewSourceWorkTask(message);
    }
}