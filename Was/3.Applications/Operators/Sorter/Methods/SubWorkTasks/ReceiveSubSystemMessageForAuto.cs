using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class ReceiveSubSystemMessageForAuto : CommonExecuteMethod
    {
        public ReceiveSubSystemMessageForAuto()
        {
            Id = "ReceiveSorterSubSystemMessageForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            ReceiveSubSystemMessageForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}