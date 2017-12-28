using System;

namespace Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks
{
    public class FinishSourceWorkTask : CommonExecuteMethod
    {
        public FinishSourceWorkTask()
        {
            Id = "FinishSdsSimulationSourceWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => FinishSourceWorkTask(message);
    }
}