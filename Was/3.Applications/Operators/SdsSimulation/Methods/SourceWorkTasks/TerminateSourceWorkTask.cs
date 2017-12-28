using System;

namespace Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks
{
    public class TerminateSourceWorkTask : CommonExecuteMethod
    {
        public TerminateSourceWorkTask()
        {
            Id = "TerminateSdsSimulationSourceWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => TerminateSourceWorkTask(message, false);
    }
}