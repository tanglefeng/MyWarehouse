﻿using System;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Operator.Jd.ExecuteMethod.SourceWorkTask
{
    public class ReceiveSubSystemMessageForAuto : CommonExecuteMethod
    {
        public ReceiveSubSystemMessageForAuto()
        {
            Id = "ReceiveSubSystemMessageForAuto";
        }

        protected override Tuple<bool, string> Execute(string message)
        {
            ReceiveSubSystemMessageForAuto();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}