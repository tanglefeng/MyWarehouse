using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class FinishSubWorkTaskForAuto : CommonExecuteMethod
    {
        public FinishSubWorkTaskForAuto()
        {
            Id = "FinishSorterSubWorkTaskForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            FinishSubWorkTaskForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}