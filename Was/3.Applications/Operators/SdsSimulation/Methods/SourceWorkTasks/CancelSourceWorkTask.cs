using System;

namespace Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks
{
    public class CancelSourceWorkTask : CommonExecuteMethod
    {
        public CancelSourceWorkTask()
        {
            Id = "CancelSdsSimulationSourceWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => CancelSourceWorkTask(message);
    }
}