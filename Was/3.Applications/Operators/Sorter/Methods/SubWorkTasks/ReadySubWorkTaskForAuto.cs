using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class ReadySubWorkTaskForAuto : CommonExecuteMethod
    {
        public ReadySubWorkTaskForAuto()
        {
            Id = "ReadySorterSubWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            ReadySubWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}