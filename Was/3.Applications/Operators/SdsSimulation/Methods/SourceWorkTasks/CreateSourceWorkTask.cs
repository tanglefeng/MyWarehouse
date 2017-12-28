using System;

namespace Kengic.Was.Operator.SdsSimulation.Methods.SourceWorkTasks
{
    public class CreateSourceWorkTask : CommonExecuteMethod
    {
        public CreateSourceWorkTask()
        {
            Id = "CreateSdsSimulationSourceWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => CreateSourceWorkTask(message);
    }
}