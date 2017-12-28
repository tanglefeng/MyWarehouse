using System;

namespace Kengic.Was.Operator.Sorter.Methods.Messages
{
    public class CancelMessage : CommonExecuteMethod
    {
        public CancelMessage()
        {
            Id = "CancelSorterMessageWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => CancelMessageWorkTask(message);
    }
}