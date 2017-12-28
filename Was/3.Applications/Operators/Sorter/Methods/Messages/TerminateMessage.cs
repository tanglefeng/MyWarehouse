using System;

namespace Kengic.Was.Operator.Sorter.Methods.Messages
{
    public class TerminateMessage : CommonExecuteMethod
    {
        public TerminateMessage()
        {
            Id = "TerminateSorterMessageWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => TerminateMessageWorkTask(message);
    }
}