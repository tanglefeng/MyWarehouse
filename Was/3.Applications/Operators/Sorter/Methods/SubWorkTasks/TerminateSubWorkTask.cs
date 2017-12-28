using System;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class TerminateSubWorkTask : CommonExecuteMethod
    {
        public TerminateSubWorkTask()
        {
            Id = "TerminateSorterSubWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => TerminateSubWorkTask(message);
    }
}