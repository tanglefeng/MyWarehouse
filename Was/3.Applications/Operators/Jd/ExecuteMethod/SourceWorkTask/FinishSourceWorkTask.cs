﻿using System;

namespace Kengic.Was.Operator.Jd.ExecuteMethod.SourceWorkTask
{
    public class FinishSourceWorkTask : CommonExecuteMethod
    {
        public FinishSourceWorkTask()
        {
            Id = "FinishSourceWorkTask";
        }

        protected override Tuple<bool, string> Execute(string message) => FinishSourceWorkTask(message);
    }
}