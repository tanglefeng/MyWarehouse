using System;

namespace Kengic.Was.Operator.Sorter.Methods.SubWorkTasks
{
    public class ReadySubWorkTaskForComplement : CommonExecuteMethod
    {
        public ReadySubWorkTaskForComplement()
        {
            Id = "ReadySorterSubWorkTaskForComplement";
        }

        protected override Tuple<bool, string> Execute(string message) => ReadySubWorkTaskForComplement(message);
    }
}