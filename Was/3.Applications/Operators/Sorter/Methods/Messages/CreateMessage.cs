using System;

namespace Kengic.Was.Operator.Sorter.Methods.Messages
{
    public class CreateMessage : CommonExecuteMethod
    {
        public CreateMessage()
        {
            Id = "CreateSorterMessageWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => CreateMessageWorkTask(message);
    }
}