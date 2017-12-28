using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class FaultTimeoutSubWorkTaskForAuto : CommonExecuteMethod
    {
        public FaultTimeoutSubWorkTaskForAuto()
        {
            Id = "FaultTimeoutSorterSubWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            FaultTimeoutSubWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}