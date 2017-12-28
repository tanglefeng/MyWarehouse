using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class ReleaseSubWorkTaskForAuto : CommonExecuteMethod
    {
        public ReleaseSubWorkTaskForAuto()
        {
            Id = "ReleaseSorterSubWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            ReleaseSubWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}