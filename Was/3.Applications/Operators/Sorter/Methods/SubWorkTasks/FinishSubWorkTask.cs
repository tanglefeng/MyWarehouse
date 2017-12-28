using System;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class FinishSubWorkTask : CommonExecuteMethod
    {
        public FinishSubWorkTask()
        {
            Id = "FinishSorterSubWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => FinishSubWorkTask(message);
    }
}