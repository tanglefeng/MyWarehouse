using System;

namespace Kengic.Was.Operator.Sorter.Methods.ExecuteWorkTasks
{
    public class CreateExecuteWorkTask : CommonExecuteMethod
    {
        public CreateExecuteWorkTask()
        {
            Id = "CreateSorterExecuteWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => CreateExecuteWorkTask(message);
    }
}