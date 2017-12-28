using System;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class CancelSubWorkTask : CommonExecuteMethod
    {
        public CancelSubWorkTask()
        {
            Id = "CancelSorterSubWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => CancelSubWorkTask(message);
    }
}