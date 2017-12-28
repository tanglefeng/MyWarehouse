using System.Collections.Generic;
using System.Threading;
using Kengic.Was.Operator.Common.Methods;
using Kengic.Was.Operator.Common.Processes;

namespace Kengic.Was.Operator.Common.Threads
{
    public class OperatorFunctionThread
    {
        public OperatorFunctionThread()
        {
            ThreadIntervalTime = 100;
            FuncProcessList = new List<OperatorFunctionProcess>();
        }

        public string Name { get; set; }
        public string Id { get; set; }
        public bool ThreadIsBegin { get; set; }
        public bool ThreadIsClosed { get; set; }
        public int ThreadIntervalTime { get; set; }
        public OperatorMessageManager MessageManager { get; set; }
        public OperatorQueueManager QueueManager { get; set; }
        public OperatorParameterManager ParameterManager { get; set; }
        public OperatorFunctionMethodManager FuncMethodManager { get; set; }
        public List<OperatorFunctionProcess> FuncProcessList { get; set; }

        public void Start()
        {
            var newThread = new Thread(RunThread) {Name = Name, IsBackground = true};
            newThread.Start();
        }

        public void RunThread()
        {
            while (ThreadIsBegin)
            {
                ThreadIsClosed = false;

                if ((FuncProcessList != null) && (FuncProcessList.Count > 0))
                {
                    foreach (var opFuncProcess in FuncProcessList)
                    {
                        opFuncProcess.ProcessExecute();
                    }
                }

                Thread.Sleep(ThreadIntervalTime);
            }

            ThreadIsClosed = true;
        }
    }
}