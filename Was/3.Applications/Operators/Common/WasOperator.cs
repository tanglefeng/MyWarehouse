using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Operators;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Operator.Common.Methods;
using Kengic.Was.Operator.Common.Processes;
using Kengic.Was.Operator.Common.Threads;
using Kengic.Was.Systems.ActivityContracts;

namespace Kengic.Was.Operator.Common
{
    public class WasOperator : IOperator
    {
        private readonly OperatorFunctionMethodManager _functionMethodManager = new OperatorFunctionMethodManager();
        private readonly OperatorFunctionProcessManager _functionProcessManager = new OperatorFunctionProcessManager();
        private readonly OperatorFunctionThreadManager _functionThreadManager = new OperatorFunctionThreadManager();
        private readonly OperatorMessageManager _messageManager = new OperatorMessageManager();
        private readonly OperatorParameterManager _parameterManager = new OperatorParameterManager();
        private readonly OperatorQueueManager _queueManager = new OperatorQueueManager();
        private OperatorPropertySection _operatorPropertySection;
        public string Id { get; set; }
        public string Name { get; set; }
        public OperatorElement OperatorElement { get; set; }
        public bool IsReciveMessage { get; set; }
        public string LogName { get; set; }
        public OperatorStatus Status { get; set; }

        public bool Initialize()
        {
            LogRepository.WriteEventLog(LogName, StaticParameterForMessage.InitializeOperator, Id);
            _operatorPropertySection =
                ConfigurationOperation<OperatorPropertySection>.GetCustomSection(OperatorElement.FilePath,
                    OperatorElement.SectionName);
            OnInitializing(this, EventArgs.Empty);
            if (!OperatorInitialze())
            {
                return false;
            }
            OnInitialized(this, EventArgs.Empty);

            LogRepository.WriteEventLog(LogName, StaticParameterForMessage.Separator, Id);

            return true;
        }

        public bool Start()
        {
            LogRepository.WriteEventLog(LogName, StaticParameterForMessage.StartOperator, Id);
            OnStarting(this, EventArgs.Empty);

            if (!OperatorStart())
            {
                return false;
            }
            OnStarted(this, EventArgs.Empty);

            LogRepository.WriteEventLog(LogName, StaticParameterForMessage.Separator, Id);
            return true;
        }

        public bool Close()
        {
            LogRepository.WriteEventLog(LogName, StaticParameterForMessage.StopOperator, Id);

            OnStoping(this, EventArgs.Empty);

            if (!OperatorStop())
            {
                return false;
            }
            OnStoped(this, EventArgs.Empty);

            LogRepository.WriteEventLog(LogName, StaticParameterForMessage.Separator, Id);

            return true;
        }

        public event EventHandler Initializing;
        public event EventHandler Initialized;
        public event EventHandler Starting;
        public event EventHandler Started;
        public event EventHandler Stoping;
        public event EventHandler Stoped;
        protected virtual void OnInitializing(object sender, EventArgs e) => Initializing?.Invoke(sender, e);
        protected virtual void OnInitialized(object sender, EventArgs e) => Initialized?.Invoke(sender, e);
        protected virtual void OnStarting(object sender, EventArgs e) => Starting?.Invoke(sender, e);
        protected virtual void OnStarted(object sender, EventArgs e) => Started?.Invoke(sender, e);
        protected virtual void OnStoping(object sender, EventArgs e) => Stoping?.Invoke(sender, e);
        protected virtual void OnStoped(object sender, EventArgs e) => Stoped?.Invoke(sender, e);

        private void RegisterParameters()
        {
            foreach (ParameterElement paraEeLement in _operatorPropertySection.Parameters)
            {
                _parameterManager.RegisterParameter(paraEeLement.Id, paraEeLement.Type, paraEeLement.Value);

                LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.RegisterParameter,
                    paraEeLement.Id + "," + paraEeLement.Type + "," + paraEeLement.Value);
            }
        }

        private void RegisterActivityConfig()
        {
            var operatorActivityList = new List<ActivityElement>();
            foreach (ActivityElement activityELement in _operatorPropertySection.Activitys)
            {
                operatorActivityList.Add(activityELement);

                LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.RegisterActivity,
                    $"{activityELement.Code},{activityELement.ExecuteOperator}");
            }

            _parameterManager.RegisterParameter(StaticParameterForOperator.ActivityQueueName,
                typeof (List<ActivityElement>),
                operatorActivityList);
        }

        public Tuple<bool, string> ReceiveSyncMessage(ProcessMessage message) => RecSyncMessage(message);
        public Tuple<bool, string> ReceiveAsyncMessage(ProcessMessage message) => RecAsyncMessage(message);

        protected virtual bool OperatorInitialze()
        {
            RegisterPara();

            RegisterMessageQueue();

            RegisterQueue();

            RegisterFuncThread();

            RegisterFunctionMethod();

            return true;
        }

        protected virtual bool OperatorStart()
        {
            //Start all thread of the operator
            var threadList = _functionThreadManager.GetThreadList();
            if ((threadList == null) || (threadList.Count <= 0))
            {
                return true;
            }
            foreach (var opThread in threadList.Where(opThread => !opThread.ThreadIsBegin))
            {
                opThread.ThreadIsBegin = true;
                opThread.Start();

                LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.StartThreadSuccess, opThread.Id);
            }

            return true;
        }

        protected virtual bool OperatorStop()
        {
            var threadList = _functionThreadManager.GetThreadList();

            if ((threadList == null) || (threadList.Count <= 0))
            {
                return true;
            }
            foreach (var opThread in threadList.Where(opThread => opThread.ThreadIsBegin))
            {
                opThread.ThreadIsBegin = false;
                Thread.Sleep(20);

                LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.StopThread, opThread.Id);
            }

            return true;
        }

        protected virtual Tuple<bool, string> RecAsyncMessage(ProcessMessage message)
        {
            try
            {
                var recMessageQueue = _messageManager.GetMessageQueue(StaticParameterForOperator.ReceiveMessageQueueName);
                if ((recMessageQueue == null) || (message == null))
                {
                    LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.NoReceiveMessageQueue,
                        StaticParameterForOperator.ReceiveMessageQueueName);
                    return new Tuple<bool, string>(false, StaticParameterForMessage.NoReceiveMessageQueue);
                }

                //receive message queue is max
                var receiveQueueMaxNumber =
                    _parameterManager.GetParameter<int>(StaticParameterForOperator.ReceiveQueueMaxNumber);
                if (recMessageQueue.Values.Count >= receiveQueueMaxNumber)
                {
                    return new Tuple<bool, string>(false, StaticParameterForMessage.MaxMessage);
                }

                LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.ReceiveAsyncMessage,
                    $"{message.ActivityContrace},{message.Message}");

                recMessageQueue.TryAdd(message.Id, message);

                return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(LogName, StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());

                return new Tuple<bool, string>(false, StaticParameterForMessage.ReceiveMessageException);
            }
        }

        protected virtual Tuple<bool, string> RecSyncMessage(ProcessMessage message)
        {
            var activityContractName = message.ActivityContrace;

            try
            {
                var activityContract = ActivityContractRepository.GetActivityContract(activityContractName);

                var functionMethod = _functionMethodManager.GetMethod(activityContract.ActivityContract);

                if (functionMethod == null)
                {
                    const string messageCode = StaticParameterForMessage.MethodIsNotExist;
                    LogRepository.WriteErrorLog(LogName, messageCode, activityContract.ActivityContract);
                    return new Tuple<bool, string>(false, messageCode);
                }

                return functionMethod.MethodExecute(message.Message);
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(LogName, StaticParameterForMessage.MethodExecuteExcept,
                    $"{activityContractName},{ex}");

                return new Tuple<bool, string>(false, StaticParameterForMessage.MethodExecuteExcept);
            }
        }

        protected virtual void RegisterPara()
        {
            RegisterParameters();
            RegisterActivityConfig();
        }

        protected virtual void RegisterMessageQueue()
        {
            foreach (MessageQueueElement cConfigSql in _operatorPropertySection.MessageQueues)
            {
                var queueId = cConfigSql.Id;
                var queueEnable = cConfigSql.Enable;

                if (!queueEnable)
                {
                    continue;
                }

                var messageQueue = new ConcurrentDictionary<string, ProcessMessage>();
                _messageManager.RegisterMessageQueue(queueId, messageQueue);
                LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.RegisterMessageQueue, queueId);
            }
        }

        protected virtual void RegisterQueue()
        {
        }

        protected void RegisterQueue(string queueId, object queue)
        {
            _queueManager.RegisterQueue(queueId, queue);
            LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.RegisterQueue, queueId);
        }

        protected virtual void RegisterFuncThread()
        {
            foreach (ThreadElement threadElement in _operatorPropertySection.Threads)
            {
                var threadId = threadElement.Id;
                var threadName = threadElement.Name;
                var threadInterval = threadElement.Interval;
                var threadEnable = threadElement.Enable;

                if (!threadEnable)
                {
                    continue;
                }

                var functionThread = new OperatorFunctionThread
                {
                    Id = threadId,
                    Name = threadName,
                    ThreadIntervalTime = threadInterval,
                    MessageManager = _messageManager,
                    ParameterManager = _parameterManager,
                    FuncMethodManager = _functionMethodManager
                };

                _functionThreadManager.RegisterThread(threadId, functionThread);
                LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.RegisterThread, threadId);

                var configProcessList = threadElement.Processes;
                if ((configProcessList == null) || (configProcessList.Count <= 0))
                {
                    continue;
                }

                RegisterFuncProcess();

                foreach (ProcessElement processeElement in configProcessList)
                {
                    var processId = processeElement.Id;
                    var processName = processeElement.Name;
                    var priority = processeElement.Priority;

                    if (!_functionProcessManager.IsExistProcess(processId))
                    {
                        continue;
                    }

                    var opFuncProcess = _functionProcessManager.GetProcess(processId);
                    opFuncProcess.Name = processName;
                    opFuncProcess.Priority = priority;

                    functionThread.FuncProcessList.Add(opFuncProcess);

                    LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.RegisterProcess, processId);
                }
            }
        }

        protected virtual void RegisterFuncProcess()
        {
            RegisterFunctionProcess(new AsyncReceiveMessageProcess
            {
                Id = StaticParameterForOperator.AsyncReceiveMessageProcess
            });
            RegisterFunctionProcess(new AsyncSendMessageProcess
            {
                Id = StaticParameterForOperator.AsyncSendMessageProcess
            });
        }

        protected void RegisterFunctionProcess(OperatorFunctionProcess functionProcess)
        {
            functionProcess.MessageManager = _messageManager;
            functionProcess.ParameterManager = _parameterManager;
            functionProcess.QueueManager = _queueManager;
            functionProcess.FunctionMethodManager = _functionMethodManager;
            functionProcess.LogName = LogName;
            _functionProcessManager.RegisterProcess(functionProcess.Id, functionProcess);
        }

        protected virtual void RegisterFunctionMethod()
        {
        }

        protected void RegisterFunctionMethod(OperatorFunctionMethod functionMethod)
        {
            _functionMethodManager.RegisterMethod(functionMethod.Id, functionMethod);

            functionMethod.MessageManager = _messageManager;
            functionMethod.ParameterManager = _parameterManager;
            functionMethod.QueueManager = _queueManager;
            functionMethod.LogName = LogName;
            functionMethod.OperatorName = Name;
            LogRepository.WriteInfomationLog(LogName, StaticParameterForMessage.RegisterMethod, functionMethod.Id);
        }
    }
}