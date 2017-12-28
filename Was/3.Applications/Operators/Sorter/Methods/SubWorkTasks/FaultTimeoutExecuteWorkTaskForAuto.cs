using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class FaultTimeoutExecuteWorkTaskForAuto : CommonExecuteMethod
    {
        public FaultTimeoutExecuteWorkTaskForAuto()
        {
            Id = "FaultTimeoutSorterExecuteWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            FaultTimeoutExecuteWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}