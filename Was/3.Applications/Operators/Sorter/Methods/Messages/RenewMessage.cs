using System;

namespace Kengic.Was.Operator.Sorter.Methods.Messages
{
    public class RenewMessage : CommonExecuteMethod
    {
        public RenewMessage()
        {
            Id = "RenewSorterMessageWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => RenewMessageWorkTask(message);
    }
}