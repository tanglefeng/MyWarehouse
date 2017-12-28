using System;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class CreateSubWorkTask : CommonExecuteMethod
    {
        public CreateSubWorkTask()
        {
            Id = "CreateSorterSubWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => CreateSubWorkTask(message);
    }
}