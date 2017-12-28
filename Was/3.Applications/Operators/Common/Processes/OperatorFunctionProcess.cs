using System;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Operator.Common.Methods;

namespace Kengic.Was.Operator.Common.Processes
{
    public class OperatorFunctionProcess
    {
        public int Priority;
        public string Name { get; set; }
        public string Id { get; set; }
        public string LogName { get; set; }
        public OperatorMessageManager MessageManager { get; set; }
        public OperatorQueueManager QueueManager { get; set; }
        public OperatorParameterManager ParameterManager { get; set; }
        public OperatorFunctionMethodManager FunctionMethodManager { get; set; }

        public bool ProcessExecute()
        {
            try
            {
                InitializeProcess();
                return Execute();
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(LogName, StaticParameterForMessage.SendMessageException, ex.ToString());
                return false;
            }
        }

        protected virtual void InitializeProcess()
        {
        }

        protected virtual bool Execute() => false;

        protected void ExecuteMethodInProcess(string activityName)
        {
            if (!FunctionMethodManager.IsExistMethod(activityName))
            {
                return;
            }

            FunctionMethodManager.GetMethod(activityName).MethodExecute(null);
        }
    }
}