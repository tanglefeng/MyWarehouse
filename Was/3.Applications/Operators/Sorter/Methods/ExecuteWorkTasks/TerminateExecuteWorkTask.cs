using System;

namespace Kengic.Was.Operator.Sorter.Methods.ExecuteWorkTasks
{
    public class TerminateExecuteWorkTask : CommonExecuteMethod
    {
        public TerminateExecuteWorkTask()
        {
            Id = "TerminateSorterExecuteWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => TerminateExecuteWorkTask(message);
    }
}