using System;

namespace Kengic.Was.Operator.Sorter.Methods.ExecuteWorkTasks
{
    public class CancelExecuteWorkTask : CommonExecuteMethod
    {
        public CancelExecuteWorkTask()
        {
            Id = "CancelSorterExecuteWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => CancelExecuteWorkTask(message);
    }
}