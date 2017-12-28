using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Kengic.Was.Application.Services.Sorter.Sorters;
using Kengic.Was.Connector.Common;
using Kengic.Was.Connector.Scs;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AlarmEvent;
using Kengic.Was.Domain.Entity.Sorter.LogicalDestinations;
using Kengic.Was.Domain.Entity.Sorter.Plans;
using Kengic.Was.Domain.Entity.Sorter.Routings;
using Kengic.Was.Domain.Entity.Sorter.Shutes;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Domain.Entity.SystemParameter;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common.Methods;
using Kengic.Was.Operator.Sorter.Arithmetics;
using Kengic.Was.Operator.Sorter.Parameters;
using Kengic.Was.Operator.Sorter.Queues;
using Microsoft.Practices.ServiceLocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;

namespace Kengic.Was.Operator.Sorter.Methods
{
    public class CommonExecuteMethod : OperatorFunctionMethod
    {
        private SorterExecuteWorkTaskRepository _executeWorkTaskRepository;
        private SorterMessageWorkTaskQueue _messageWorkTaskQueue;
        private SorterSubWorkTaskQueue _subWorkTaskQueue;
        private int _timeoutDayForExecuteWorkTask = 7;
        private int _timeoutSecondForSubWorkTask = 600;

        protected override void InitializeFunction()
        {
            const string executeWorkTaskQueueName = StaticParameterForWorkTask.ExecuteWorkTaskQueueName;
            if (QueueManager.IsExistQueue(executeWorkTaskQueueName))
            {
                _executeWorkTaskRepository =
                    (SorterExecuteWorkTaskRepository)
                        QueueManager.GetQueue(executeWorkTaskQueueName);
            }

            const string subWorkTaskQueueName = StaticParameterForWorkTask.SubWorkTaskQueueName;
            if (QueueManager.IsExistQueue(subWorkTaskQueueName))
            {
                _subWorkTaskQueue =
                    (SorterSubWorkTaskQueue)
                        QueueManager.GetQueue(subWorkTaskQueueName);
            }

            const string messageWorkTaskQueueName = StaticParameterForSorter.MessageWorkTaskQueueName;
            if (QueueManager.IsExistQueue(messageWorkTaskQueueName))
            {
                _messageWorkTaskQueue =
                    (SorterMessageWorkTaskQueue)
                        QueueManager.GetQueue(messageWorkTaskQueueName);
            }

            _timeoutSecondForSubWorkTask =
                ParameterManager.GetParameter<int>(StaticParameterForSorter.SortingMessageTimeout);
            _timeoutDayForExecuteWorkTask =
                ParameterManager.GetParameter<int>(StaticParameterForSorter.SortingExecuteWorkTaskTimeout);
            base.InitializeFunction();
        }

        public void ReceiveSubSystemMessageForAuto()
        {
            //Get all executor of the operator
            var messageDict = new ConcurrentDictionary<string, object>();
            try
            {
                var executorList = GetExecutors();
                if ((executorList == null) || (executorList.Count <= 0))
                {
                    return;
                }
                //Get message from the different executor in turns
               
                foreach (var executor in executorList)
                {
                    var connectionId = executor.Connection;
                     messageDict = GetMessageFromSubsystem(connectionId);
                    if ((messageDict == null) || (messageDict.Count <= 0))
                    {
                        continue;
                    }

                    var messageKeyList = messageDict.Keys;
                    foreach (var messageKey in messageKeyList)
                    {
                        var scsClientMessage = messageDict[messageKey] as ScsClientMessage;
                        if ((scsClientMessage == null) || (scsClientMessage.ScsBodyList.Count <= 0))
                        {
                            continue;
                        }

                        switch (scsClientMessage.ScsHeader.OperationId)
                        {
                            case StaticParameterForSorter.Dr5:
                                CreateSubWorkTask(scsClientMessage, connectionId);
                                break;
                            case StaticParameterForSorter.De5:
                                ActiveSubWorkTask(scsClientMessage, connectionId);
                                break;
                            case StaticParameterForSorter.Ov5:
                                FinishSubWorkTask(scsClientMessage);
                                break;
                            case StaticParameterForSorter.Dn5:
                                SuspendSubWorkTask(scsClientMessage);
                                break;
                            case StaticParameterForSorter.Pl5:
                                //用于报告快件丢失事件
                                FaultSubWorkTask(scsClientMessage);
                                break;
                            case StaticParameterForSorter.Pd5:
                                CreateSubWorkTaskForDp(scsClientMessage, connectionId);
                                break;
                            case StaticParameterForSorter.Pd51:
                                CreateInductMessageWorkTaskFor5Pd1(scsClientMessage, connectionId);
                                break;
                            case StaticParameterForSorter.Pd52:
                                CreateInductMessageWorkTaskFor5Pd2(scsClientMessage, connectionId);
                                break;
                            case StaticParameterForSorter.Pr9:
                                CreatePackageMessageWorkTask(scsClientMessage, connectionId);
                                break;
                        }

                        object outMessage;
                        messageDict.TryRemove(messageKey, out outMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                messageDict.Clear();
                LogRepository.WriteExceptionLog(LogName, StaticParameterForMessage.ReceiveMessageException,
                    ex.ToString());
            }
        }

        public DateTime ConvertToDateTime(string dateTimeString)
        {
            if (dateTimeString.Length < 30)
            {
                return default(DateTime);
            }
            try
            {
                var year = Convert.ToInt32(dateTimeString.Substring(8, 2)) + 2000;
                var month = Convert.ToInt32(dateTimeString.Substring(11, 2));
                var day = Convert.ToInt32(dateTimeString.Substring(14, 2));
                var hour = Convert.ToInt32(dateTimeString.Substring(17, 2));
                var minute = Convert.ToInt32(dateTimeString.Substring(20, 2));
                var second = Convert.ToInt32(dateTimeString.Substring(23, 2));
                var millisecond = Convert.ToInt32(dateTimeString.Substring(26, 3));

                var dateTime = new DateTime(year, month, day, hour, minute, second, millisecond);
                return dateTime;
            }
            catch (Exception)
            {
                LogRepository.WriteExceptionLog(LogName, $"string can not convert to date time '{dateTimeString}'", null);
                return default(DateTime);
            }
        }

        protected bool CreateInductMessageWorkTaskFor5Pd1(ScsClientMessage message, string connector)
        {
            var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.MessageWorkTaskPrefix);
            foreach (var scsbody in message.ScsBodyList)
            {
                var objectToHandle = "";
                if (scsbody.ScsBodyFieldList.Count >= 7)
                {
                    objectToHandle = scsbody.ScsBodyFieldList[6].Value.Trim();
                }

                var messageWorkTask = new SorterMessageWorkTask
                {
                    Id = SequenceCreator.GetIdBySeqNoAndKey(prefix),
                    TrackingId = scsbody.ScsBodyFieldList[0].Value.Trim(),
                    Type = SorterMessageType.InductFailure,
                    Induct = scsbody.ScsBodyFieldList[4].Value.Substring(8, 6).Trim(),
                    Results = scsbody.ScsBodyFieldList[3].Value.Substring(8, 2).Trim(),
                    InductTime = ConvertToDateTime(scsbody.ScsBodyFieldList[5].Value.Trim()),
                    ObjectToHandle = objectToHandle,
                    CreateTime = DateTime.Now,
                    CreateBy = OperatorName,
                    OperatorName = OperatorName,
                    TriggerMode = WorkTaskTriggerMode.Immediately,
                    Connect = connector
                };


                _messageWorkTaskQueue.CreateWorkTask(messageWorkTask,
                    e =>
                        (e.Id == message.Id) &&
                        ((int) e.Status < (int) WorkTaskStatus.Completed));
            }

            return true;
        }

        protected bool CreateInductMessageWorkTaskFor5Pd2(ScsClientMessage message, string connector)
        {
            var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.MessageWorkTaskPrefix);
            foreach (var scsbody in message.ScsBodyList)
            {
                var objectToHandle = "";
                if (scsbody.ScsBodyFieldList.Count >= 8)
                {
                    objectToHandle = scsbody.ScsBodyFieldList[7].Value.Trim();
                }

                try
                {
                    var messageWorkTask = new SorterMessageWorkTask
                    {
                        Id = SequenceCreator.GetIdBySeqNoAndKey(prefix),
                        TrackingId = scsbody.ScsBodyFieldList[0].Value.Trim(),
                        Type = SorterMessageType.InductSuccess,
                        Induct = scsbody.ScsBodyFieldList[4].Value.Substring(8, 6).Trim(),
                        InductMode = scsbody.ScsBodyFieldList[3].Value.Substring(8, 2).Trim(),
                        Weight = Convert.ToDecimal(scsbody.ScsBodyFieldList[6].Value.Substring(8, 6).Trim()),
                        InductTime = ConvertToDateTime(scsbody.ScsBodyFieldList[5].Value.Trim()),
                        ObjectToHandle = objectToHandle,
                        CreateTime = DateTime.Now,
                        CreateBy = OperatorName,
                        OperatorName = OperatorName,
                        TriggerMode = WorkTaskTriggerMode.Event,
                        Connect = connector
                    };
               
                    _messageWorkTaskQueue.CreateWorkTask(messageWorkTask,
                    e =>
                        (e.Id == message.Id) &&
                        ((int) e.Status < (int) WorkTaskStatus.Completed));
                }
                catch (Exception ex)
                {
                    LogRepository.WriteExceptionLog(LogName, $"CreateInductMessageWorkTaskFor5Pd2 '{ex.ToString()}'", null);
                }
            }

            return true;
        }

        protected bool CreatePackageMessageWorkTask(ScsClientMessage message, string connector)
        {
            var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.MessageWorkTaskPrefix);
            foreach (var scsbody in message.ScsBodyList)
            {
                var workTaskType = SorterMessageType.CreatePackage;
                var packageMode = scsbody.ScsBodyFieldList[1].Value.Substring(8, 2).Trim();
                switch (packageMode)
                {
                    case "PP":
                        workTaskType = SorterMessageType.PreCreatePackage;
                        break;
                    case "PN":
                        workTaskType = SorterMessageType.CancelPackage;
                        break;
                }


                var messageWorkTask = new SorterMessageWorkTask
                {
                    Id = SequenceCreator.GetIdBySeqNoAndKey(prefix),
                    TrackingId = scsbody.ScsBodyFieldList[0].Value.Trim(),
                    Type = workTaskType,
                    ObjectToHandle = scsbody.ScsBodyFieldList[2].Value.Substring(8, 6).Trim(),
                    CreateTime = DateTime.Now,
                    CreateBy = OperatorName,
                    OperatorName = OperatorName,
                    TriggerMode = WorkTaskTriggerMode.Immediately,
                    Connect = connector
                };


                _messageWorkTaskQueue.CreateWorkTask(messageWorkTask,
                    e =>
                        (e.Id == message.Id) &&
                        ((int) e.Status < (int) WorkTaskStatus.Completed));
            }

            return true;
        }

        protected Tuple<bool, string> CreateMessageWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterMessageWorkTask>();

            if (!string.IsNullOrEmpty(workTask.Id))
                return _messageWorkTaskQueue.CreateWorkTask(workTask,
                    e =>
                        (e.Type == workTask.Type) &&
                        (e.ObjectToHandle == workTask.ObjectToHandle) &&
                        ((int) e.Status < (int) WorkTaskStatus.Completed));
            var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.MessageWorkTaskPrefix);
            workTask.Id = SequenceCreator.GetIdBySeqNoAndKey(prefix);

            return _messageWorkTaskQueue.CreateWorkTask(workTask,
                e =>
                    (e.Type == workTask.Type) &&
                    (e.ObjectToHandle == workTask.ObjectToHandle) &&
                    ((int) e.Status < (int) WorkTaskStatus.Completed));
        }

        protected Tuple<bool, string> RenewMessageWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskid = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var readyBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.UpdateBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var rtnValue = _messageWorkTaskQueue.IsExistWorkTask(workTaskid);

            if (!rtnValue.Item1)
            {
                return rtnValue;
            }

            var workTask = _messageWorkTaskQueue.TryGetValue(workTaskid);

            return _messageWorkTaskQueue.RenewWorkTask(workTask, readyBy, reason);
        }

        protected Tuple<bool, string> CancelMessageWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var cancelBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.CancelledBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var rtnValue = _messageWorkTaskQueue.IsExistWorkTask(workTaskId);

            if (!rtnValue.Item1)
            {
                return rtnValue;
            }

            var workTask = _messageWorkTaskQueue.TryGetValue(workTaskId);

            return _messageWorkTaskQueue.CancelWorkTask(workTask, cancelBy, reason);
        }

        protected Tuple<bool, string> TerminateMessageWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var terminatedBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.TerminatedBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var rtnValue = _messageWorkTaskQueue.IsExistWorkTask(workTaskId);

            if (!rtnValue.Item1)
            {
                return rtnValue;
            }

            var workTask = _messageWorkTaskQueue.TryGetValue(workTaskId);

            return _messageWorkTaskQueue.TerminateWorkTask(workTask, terminatedBy, reason);
        }

        protected void ReleaseMessageWorkTaskForAuto()
        {
            var workTaskList = _messageWorkTaskQueue.GetWorkTaskForRelease();
            if (!((workTaskList == null) || (workTaskList.Count <= 0)))
            {
                foreach (var workTask in workTaskList)
                {
                    var notifyMessageId = string.Empty;

                    switch (workTask.Type)
                    {
                        case SorterMessageType.PreCreatePackage:
                            notifyMessageId = StaticParameterForSorter.NotifyOperatorPreCreatePackage;
                            break;
                        case SorterMessageType.CancelPackage:
                            notifyMessageId = StaticParameterForSorter.NotifyOperatorCancelPackage;
                            break;
                        case SorterMessageType.CreatePackage:
                            notifyMessageId = StaticParameterForSorter.NotifyOperatorCreatePackage;
                            break;
                    }

                    if (string.IsNullOrEmpty(notifyMessageId))
                    {
                        if (workTask.CreateTime < DateTime.Now.AddSeconds(-_timeoutSecondForSubWorkTask))
                        {
                            _messageWorkTaskQueue.FaultWorkTask(workTask, OperatorName,
                                StaticParameterForMessage.Timeout);
                        }
                    }
                    else
                    {
                        if (SendMessage(workTask, notifyMessageId, 5).Item1)
                        {
                            _messageWorkTaskQueue.FinishWorkTask(workTask, OperatorName);
                        }
                    }
                }
            }

            var eventwWorkTaskList = _messageWorkTaskQueue.GetEventWorkTaskForRelease();
            if (!((eventwWorkTaskList == null) || (eventwWorkTaskList.Count <= 0)))
            {
                foreach (var eventwWorkTask in eventwWorkTaskList)
                {
                    if (eventwWorkTask.CreateTime < DateTime.Now.AddSeconds(-_timeoutSecondForSubWorkTask))
                    {
                        _messageWorkTaskQueue.FaultWorkTask(eventwWorkTask, OperatorName,
                            StaticParameterForMessage.Timeout);
                    }
                }
            }
        }

        protected Tuple<bool, string> CreateExecuteWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterExecuteWorkTask>();

            if (string.IsNullOrEmpty(workTask.Id))
            {
                var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.ExecuteWorkTaskPrefix);
                workTask.Id = SequenceCreator.GetIdBySeqNoAndKey(prefix);
            }

            var task = new Task(CreateExecuteWorkTask, workTask);
            task.Start();
            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }

        private void CreateExecuteWorkTask(object obj)
        {
            var workTask = obj as SorterExecuteWorkTask;
            if (workTask != null)
            {
                var sorterExecuteWorkTaskRepository =
                    ServiceLocator.Current.GetInstance<SorterExecuteWorkTaskRepository>();
                sorterExecuteWorkTaskRepository.LogName = OperatorName;
                sorterExecuteWorkTaskRepository.CreateWorkTask(workTask);
            }
        }

        protected Tuple<bool, string> CancelExecuteWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var cancelBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.CancelledBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var workTask = _executeWorkTaskRepository.TryGetValue(workTaskId);

            var subWTaskList =
                _subWorkTaskQueue.GetUnfinishWorkTaskByExeId(workTaskId);
            //first step:cancel all sub work task
            CancelSubWorkTask(subWTaskList, cancelBy, reason);

            //second step:cancel the execute work task
            return _executeWorkTaskRepository.CancelWorkTask(workTask, cancelBy, reason);
        }

        protected Tuple<bool, string> TerminateExecuteWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var terminateBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.TerminatedBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var workTask = _executeWorkTaskRepository.TryGetValue(workTaskId);

            var subWTaskList =
                _subWorkTaskQueue.GetUnfinishWorkTaskByExeId(workTaskId);
            //first step:terminate all sub work task
            TerminateSubWorkTask(subWTaskList, terminateBy, reason);

            //second step:terminate the execute work task
            return _executeWorkTaskRepository.TerminateWorkTask(workTask, terminateBy, reason);
        }

        public bool IsFinishExecuteWorkTask(string executeWorkTaskId)
        {
            var unfinishSubWorkTaskList = _subWorkTaskQueue.GetUnfinishWorkTaskByExeId(executeWorkTaskId);
            return (unfinishSubWorkTaskList == null) || (unfinishSubWorkTaskList.Count <= 0);
        }

        protected Tuple<bool, string> CreateSubWorkTask(string message)
        {
            var workTask = message.JsonToValue<SorterSubWorkTask>();

            if (string.IsNullOrEmpty(workTask.Id))
            {
                var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.SubWorkTaskPrefix);
                workTask.Id = SequenceCreator.GetIdBySeqNoAndKey(prefix);
            }

            if (!string.IsNullOrEmpty(workTask.NodeId))
            {
                //Get sorter information by nodeId
                var logicalSorterApplicationService =
                    ServiceLocator.Current.GetInstance<ILogicalSorterApplicationService>();
                var logicalSorter = logicalSorterApplicationService.GetValueByNodeId(workTask.NodeId);
                if (logicalSorter != null)
                {
                    workTask.PhysicalSortter = logicalSorter.PhycialSorter;
                    workTask.LogicalSortter = logicalSorter.Id;
                }
            }

            var returnValue = _subWorkTaskQueue.CreateWorkTask(workTask, e =>
                (e.Id == workTask.Id) &&
                ((int) e.Status < (int) WorkTaskStatus.Completed));

            return returnValue;
        }

        protected void CreateSubWorkTask(ScsClientMessage message, string connectionId)
        {
            var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.SubWorkTaskPrefix);
            foreach (var workTask in from scsbody in message.ScsBodyList
                let scannerBarcode = scsbody.ScsBodyFieldList[8].Value.Trim()
                select new SorterSubWorkTask
                {
                    Id = SequenceCreator.GetIdBySequenceNo(),
                    WorkTaskType = WorkTaskType.Sorting,
                    TrackingId = scsbody.ScsBodyFieldList[0].Value.Trim(),
                    Induct = scsbody.ScsBodyFieldList[2].Value.Substring(8, 6).Trim(),
                    Scanner = scsbody.ScsBodyFieldList[5].Value.Substring(8, 6).Trim(),
                    CarrierId = scsbody.ScsBodyFieldList[6].Value.Substring(8, 6).Trim(),
                    InductTime = ConvertToDateTime(scsbody.ScsBodyFieldList[3].Value.Trim()),
                    ScannerTime = ConvertToDateTime(scsbody.ScsBodyFieldList[4].Value.Trim()),
                    ScannerBarcode = scannerBarcode,
                    ObjectToHandle = scannerBarcode,
                    CreateBy = OperatorName,
                    OperatorName = OperatorName,
                    LogicalSortter = connectionId,
                    TriggerMode = WorkTaskTriggerMode.Immediately
                })
            {
                _subWorkTaskQueue.CreateWorkTask(workTask, e =>
                    (e.Id == workTask.Id) &&
                    ((int) e.Status < (int) WorkTaskStatus.Completed));
            }
        }

        protected void CreateSubWorkTaskForDp(ScsClientMessage message, string connectionId)
        {
            var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.SubWorkTaskPrefix);
            foreach (var workTask in from scsbody in message.ScsBodyList
                let scannerBarcode = scsbody.ScsBodyFieldList[2].Value.Trim()
                select new SorterSubWorkTask
                {
                    Id = SequenceCreator.GetIdBySeqNoAndKey(prefix),
                    WorkTaskType = WorkTaskType.Sorting,
                    TrackingId = scsbody.ScsBodyFieldList[0].Value.Trim(),
                    Induct = scsbody.ScsBodyFieldList[0].Value.Substring(3, 7).Trim(),
                    InductTime = DateTime.Now,
                    ScannerBarcode = scannerBarcode,
                    ObjectToHandle = scannerBarcode,
                    ComplementBarcode = scannerBarcode,
                    CreateBy = OperatorName,
                    OperatorName = OperatorName,
                    LogicalSortter = connectionId,
                    TriggerMode = WorkTaskTriggerMode.Immediately,
                    Results = StaticParameterForSorter.DisChargeMc
                })
            {
                _subWorkTaskQueue.CreateWorkTask(workTask, e =>
                    (e.Id == workTask.Id) &&
                    ((int) e.Status < (int) WorkTaskStatus.Completed));
            }
        }

        private int sorterNum = 0;
        private int getSoterShute()
        {
            sorterNum = sorterNum + 1;
            if (sorterNum >= 8)
            {
                sorterNum = 1;
            }
            return sorterNum;

        }
      
        protected void ReadySubWorkTaskForAuto()
        {
            var workTaskList = _subWorkTaskQueue.GetWorkTaskForReady();
            if (workTaskList != null && workTaskList.Count > 0)
            {
                foreach (var workTask in workTaskList)
                {
                    SendMessage(workTask, StaticParameterForSorter.NotifyRequestDestination, 5);
                    //_subWorkTaskQueue.SuspendWorkTask(workTask, "system", "Request");
                    if (!ParameterManager.GetParameter<bool>(StaticParameterForSorter.Simulation))
                    {
                        _subWorkTaskQueue.SuspendWorkTask(workTask, "system", "Request");
                    }
                    else
                    {
                        _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
                        {
                            e.SourceStatus = e.Status;
                            e.Status = WorkTaskStatus.Ready;
                            e.Results = StaticParameterForMessage.Ok;
                            e.ObjectToHandle = "NOREAD";
                            e.RequestShuteNum = "01";
                            e.RequestShuteAddr = "10" + getSoterShute().ToString("00");
                            e.TriggerMode = WorkTaskTriggerMode.Immediately;
                            e.ReadyTime = DateTime.Now;
                            e.ReadyBy = OperatorName;
                            e.Results = StaticParameterForSorter.DisChargeNd;
                            return e;
                        }, StaticParameterForMessage.UpdateSuccess, StaticParameterForMessage.UpdateFailure, new List<string>
                        {
                            "SourceStatus",
                            "Status",
                            "Results",
                            "ObjectToHandle",
                            "RequestShuteNum",
                            "RequestShuteAddr",
                            "TriggerMode",
                            "ReadyTime",
                            "ReadyBy",
                            "Results"
                        });
                    }
                }
            }
        }

        internal void ReadySubWorkTask(List<SorterSubWorkTask> workTaskList)
        {
            if ((workTaskList == null) || (workTaskList.Count <= 0))
            {
                return;
            }

            foreach (var workTask in workTaskList)
            {
                ReadySubWorkTask(workTask);
            }
        }

        internal string GetLogicalDestination(string logicalDestination)
        {
            var rtnLogicalDestination = logicalDestination;
            var logicalDestinationRepository = ServiceLocator.Current.GetInstance<ILogicalDestinationRepository>();
            var logicalDestinationObj = logicalDestinationRepository.TryGetValue(logicalDestination);
            if (!string.IsNullOrEmpty(logicalDestinationObj?.ParentId))
            {
                rtnLogicalDestination = logicalDestinationObj.ParentId;
            }

            return rtnLogicalDestination;
        }

        internal RequestShute GetRequestShute(List<Routing> routingList)
        {
            var requestShute = new RequestShute
            {
                RequestShuteNum = 0,
                RequestShuteAddr = "",
                RequestShuteAddrX = "",
                RequestShuteAddrY = "",
                RequestShuteAddrZ = ""
            };


            var shuteRepositorys = ServiceLocator.Current.GetInstance<IShuteRepository>();
            var validRoutingList = new List<Routing>();
            foreach (var theRouting in routingList)
            {
                var theShute = shuteRepositorys.TryGetValue(theRouting.PhycialShute);
                if ((theShute?.DeviceName1 != null) && theShute.IsActive)
                {
                    validRoutingList.Add(theRouting);
                }
            }

            if (validRoutingList.Count <= 0)
            {
                return requestShute;
            }

            switch (validRoutingList.Count)
            {
                case 1:
                    requestShute.RequestShuteNum = 1;
                    requestShute.RequestShuteAddr =
                        shuteRepositorys.TryGetValue(validRoutingList[0].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrX = "000000";
                    requestShute.RequestShuteAddrY = "000000";
                    requestShute.RequestShuteAddrZ = "000000";
                    break;
                case 2:
                    requestShute.RequestShuteNum = 2;
                    requestShute.RequestShuteAddr =
                        shuteRepositorys.TryGetValue(validRoutingList[0].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrX =
                        shuteRepositorys.TryGetValue(validRoutingList[1].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrY = "000000";
                    requestShute.RequestShuteAddrZ = "000000";
                    break;
                case 3:
                    requestShute.RequestShuteNum = 3;
                    requestShute.RequestShuteAddr =
                        shuteRepositorys.TryGetValue(validRoutingList[0].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrX =
                        shuteRepositorys.TryGetValue(validRoutingList[1].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrY =
                        shuteRepositorys.TryGetValue(validRoutingList[2].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrZ = "000000";
                    break;
                default:

                    requestShute.RequestShuteNum = 4;
                    requestShute.RequestShuteAddr =
                        shuteRepositorys.TryGetValue(validRoutingList[0].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrX =
                        shuteRepositorys.TryGetValue(validRoutingList[1].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrY =
                        shuteRepositorys.TryGetValue(validRoutingList[2].PhycialShute).DeviceName1;
                    requestShute.RequestShuteAddrZ =
                        shuteRepositorys.TryGetValue(validRoutingList[3].PhycialShute).DeviceName1;
                    break;
            }

            // Average sorting
            if ((validRoutingList.Count > 0) && (validRoutingList[0].RoutingStrategy == RoutingStrategy.Average))
            {
                var routing = validRoutingList[0];
                if ((routing.InternalPriority == null) || (routing.InternalPriority < 0))
                {
                    routing.InternalPriority = 0;
                }
                else
                {
                    routing.InternalPriority += 1;
                }
                if (validRoutingList.All(r => r.InternalPriority >= validRoutingList.Count))
                {
                    validRoutingList.ForEach(r => r.InternalPriority = 0);
                }
                var routingRepositorys = ServiceLocator.Current.GetInstance<IRoutingRepository>();
                routingRepositorys.Update(routing);
            }

            return requestShute;
        }

        protected Tuple<bool, string> UpdateSubWorkTaskTriggerModeForComplement(SorterSubWorkTask workTask,
            string executeWorkTaskId,
            string formatBarCode, string packageBarcode, string results)
        {
            var systemParamtersRepository = ServiceLocator.Current.GetInstance<ISystemParameterRepository>();
            var complementEnable = systemParamtersRepository.TryGetValue(StaticParameterForSorter.ComplementEnable);

            if ((complementEnable != null) && (complementEnable.Value == "1"))
            {
                workTask.Results = results;
                SendMessage(workTask, StaticParameterForSorter.NotifyComplementCreateWorkTask, 5);

                return _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
                {
                    e.WorkTaskType = WorkTaskType.Complement;
                    e.ObjectToHandle = formatBarCode;
                    e.ExecuteWorkTaskId = executeWorkTaskId;
                    e.AtricleBarcode = packageBarcode;
                    e.SourceTriggerMode = e.TriggerMode;
                    e.TriggerMode = WorkTaskTriggerMode.Event;
                    e.UpdateTime = DateTime.Now;
                    e.UpdateBy = OperatorName;
                    e.Results = results;
                    return e;
                }, StaticParameterForMessage.UpdateSuccess, StaticParameterForMessage.UpdateFailure, new List<string>
                {
                    "AtricleLength",
                    "AtricleWidth",
                    "AtricleHeight",
                    "AtricleVolume",
                    "AtricleProfile",
                    "Code",
                    "WorkTaskType",
                    "ObjectToHandle",
                    "ExecuteWorkTaskId",
                    "AtricleBarcode",
                    "SourceTriggerMode",
                    "TriggerMode",
                    "UpdateTime",
                    "UpdateBy",
                    "Results"
                });
            }

            //Modify barcode show when DE
            return ReadySubWorkTask(workTask, formatBarCode, packageBarcode, executeWorkTaskId,
                results);
        }


        internal Tuple<bool, string> ReadySubWorkTask(SorterSubWorkTask workTask)
        {
            //Formated barcode
            BarcodeInfomation barcodeInfomation;
            var barcodeFormater = ServiceLocator.Current.GetInstance<IBarcodeFormat>("CommonBarcodeFormat");
            var rtnValue = barcodeFormater.FormatBarcode(workTask.ObjectToHandle, out barcodeInfomation);

            if (rtnValue)
                return !barcodeInfomation.IfMultiBracode
                    ? SingleBarcodeExecute(workTask, barcodeInfomation)
                    : MultiBarcodeExecute(workTask, barcodeInfomation);
            //Format barcode failure
            const string messageCode = StaticParameterForMessage.FormatBarcodeFailure;
            LogRepository.WriteInfomationLog(LogName, messageCode,
                workTask.TrackingId + ":" + workTask.ObjectToHandle);
            return UpdateSubWorkTaskTriggerModeForComplement(workTask, null, workTask.ObjectToHandle, "",
                StaticParameterForSorter.DisChargeNr);
        }

        internal Tuple<bool, string> SingleBarcodeExecute(SorterSubWorkTask workTask,
            BarcodeInfomation barcodeInfomation)
        {
            var singleBarcode = barcodeInfomation.Barcode;
            var packageBarcode = barcodeInfomation.PackageBarCode;

            workTask.AtricleLength = barcodeInfomation.Length.ToString();
            workTask.AtricleWidth = barcodeInfomation.Width.ToString();
            workTask.AtricleHeight = barcodeInfomation.Height.ToString();
            workTask.AtricleVolume = barcodeInfomation.Volume;
            workTask.AtricleProfile = barcodeInfomation.CoordinateX + "," + barcodeInfomation.CoordinateY;
            workTask.Code = barcodeInfomation.OrderBarCode;

            if (singleBarcode.Contains(StaticParameterForSorter.NoRead) ||
                singleBarcode.Contains(StaticParameterForSorter.NoResponse))
            {
                //No read
                const string messageCode = StaticParameterForSorter.DisChargeNr;
                LogRepository.WriteInfomationLog(LogName, messageCode, workTask.TrackingId + ":" + singleBarcode);
                return UpdateSubWorkTaskTriggerModeForComplement(workTask, null, workTask.ObjectToHandle, packageBarcode,
                    StaticParameterForSorter.DisChargeNr);
            }

            var executeWorkTasksList = _executeWorkTaskRepository.GetValueByObject(singleBarcode);
            if ((executeWorkTasksList == null) || (executeWorkTasksList.Count <= 0))
            {
                //No mission
                const string messageCode = StaticParameterForMessage.NoMission;
                LogRepository.WriteInfomationLog(LogName, messageCode, workTask.TrackingId + ":" + singleBarcode);
                workTask.WorkTaskType = WorkTaskType.Complement;
                return UpdateSubWorkTaskTriggerModeForComplement(workTask, null, singleBarcode, packageBarcode,
                    StaticParameterForSorter.DisChargeDe);
            }

            var executeWorkTask = executeWorkTasksList[0];
            var executeWorkTaskId = executeWorkTask.Id;
            var logicalDestination = GetLogicalDestination(executeWorkTask.LogicalDestination);

            var sorterPlanRepository = ServiceLocator.Current.GetInstance<ISorterPlanRepository>();
            var activeSorterPlanList = sorterPlanRepository.GetActiveValue();
            if ((activeSorterPlanList == null) || (activeSorterPlanList.Count <= 0))
            {
                //No active sorter plane
                const string messageCode = StaticParameterForMessage.NoActiveSoterPlan;
                LogRepository.WriteInfomationLog(LogName, messageCode, singleBarcode + ":" + logicalDestination);
                return ReadySubWorkTask(workTask, singleBarcode, packageBarcode, executeWorkTaskId,
                    StaticParameterForSorter.DisChargeId);
            }

            var sorterPlan = activeSorterPlanList[0].Id;
            var routingRepositorys = ServiceLocator.Current.GetInstance<IRoutingRepository>();
            var routingList = routingRepositorys.GetPhysicalShuteByLogicalShute(logicalDestination,
                sorterPlan);
            if ((routingList == null) || (routingList.Count <= 0))
            {
                //No Routing
                const string messageCode = StaticParameterForMessage.NoRouting;
                LogRepository.WriteInfomationLog(LogName, messageCode, singleBarcode + ":" + logicalDestination);
                return ReadySubWorkTask(workTask, singleBarcode, packageBarcode, executeWorkTaskId,
                    StaticParameterForSorter.DisChargeId);
            }

            var requestShute = GetRequestShute(routingList);
            if (requestShute.RequestShuteNum == 0)
            {
                const string messageCode = StaticParameterForMessage.NoDestination;
                LogRepository.WriteInfomationLog(LogName, messageCode, singleBarcode + ":" + logicalDestination);
                return ReadySubWorkTask(workTask, singleBarcode, packageBarcode, executeWorkTaskId,
                    StaticParameterForSorter.DisChargeId);
            }

            var resultCode = StaticParameterForSorter.DisChargeNd;
            if ((workTask.Results != null) && (workTask.Results == StaticParameterForSorter.DisChargeMc))
            {
                resultCode = StaticParameterForSorter.DisChargeMc;
            }
            return ReadySubWorkTask(workTask, singleBarcode, packageBarcode, executeWorkTaskId,
                resultCode, requestShute);
        }


        internal Tuple<bool, string> MultiBarcodeExecute(SorterSubWorkTask workTask, BarcodeInfomation barcodeInfomation)
        {
            var singleBarcode = barcodeInfomation.Barcode;
            var packageBarcode = barcodeInfomation.PackageBarCode;

            workTask.AtricleLength = barcodeInfomation.Length.ToString();
            workTask.AtricleWidth = barcodeInfomation.Width.ToString();
            workTask.AtricleHeight = barcodeInfomation.Height.ToString();
            workTask.AtricleVolume = barcodeInfomation.Volume;
            workTask.AtricleProfile = barcodeInfomation.CoordinateX + "," + barcodeInfomation.CoordinateY;
            workTask.Code = barcodeInfomation.OrderBarCode;

            const string messageCode = StaticParameterForMessage.MultiBarcode;
            LogRepository.WriteInfomationLog(LogName, messageCode, workTask.TrackingId + ":" + singleBarcode);
            return UpdateSubWorkTaskTriggerModeForComplement(workTask, null, workTask.ObjectToHandle, packageBarcode,
                StaticParameterForSorter.DisChargeMb);
        }

        internal Tuple<bool, string> ReadySubWorkTask(SorterSubWorkTask workTask, string formatBarCode,
            string packageBarcode, string executeWorkTaskId, string results)
            => _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
            {
                e.ExecuteWorkTaskId = executeWorkTaskId;
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Ready;
                e.Results = results;
                e.ObjectToHandle = formatBarCode;
                e.RequestShuteNum = "00";
                e.RequestShuteAddr = "";
                e.RequestShuteAddrX = "";
                e.RequestShuteAddrY = "";
                e.RequestShuteAddrZ = "";
                e.ReadyTime = DateTime.Now;
                e.ReadyBy = OperatorName;
                return e;
            }, StaticParameterForMessage.UpdateSuccess, StaticParameterForMessage.UpdateFailure, new List<string>
            {
                "AtricleLength",
                "AtricleWidth",
                "AtricleHeight",
                "AtricleVolume",
                "AtricleProfile",
                "Code",
                "ExecuteWorkTaskId",
                "SourceStatus",
                "Status",
                "Results",
                "ObjectToHandle",
                "RequestShuteNum",
                "RequestShuteAddr",
                "RequestShuteAddrX",
                "RequestShuteAddrY",
                "RequestShuteAddrZ",
                "ReadyTime",
                "ReadyBy"
            });


        internal Tuple<bool, string> ReadySubWorkTask(SorterSubWorkTask workTask, string formatBarCode,
            string packageBarcode, string executeWorkTaskId,
            string results, RequestShute requestShute)
            => _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
            {
                e.ExecuteWorkTaskId = executeWorkTaskId;
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Ready;
                e.Results = results;
                e.ObjectToHandle = formatBarCode;
                e.RequestShuteNum = requestShute.RequestShuteNum.ToString("00");
                e.RequestShuteAddr = requestShute.RequestShuteAddr;
                e.RequestShuteAddrX = requestShute.RequestShuteAddrX;
                e.RequestShuteAddrY = requestShute.RequestShuteAddrY;
                e.RequestShuteAddrZ = requestShute.RequestShuteAddrZ;
                e.AtricleBarcode = packageBarcode;
                e.ReadyTime = DateTime.Now;
                e.ReadyBy = OperatorName;
                return e;
            }, StaticParameterForMessage.UpdateSuccess, StaticParameterForMessage.UpdateFailure, new List<string>
            {
                "AtricleLength",
                "AtricleWidth",
                "AtricleHeight",
                "AtricleVolume",
                "AtricleProfile",
                "Code",
                "ExecuteWorkTaskId",
                "SourceStatus",
                "Status",
                "Results",
                "ObjectToHandle",
                "RequestShuteNum",
                "RequestShuteAddr",
                "RequestShuteAddrX",
                "RequestShuteAddrY",
                "RequestShuteAddrZ",
                "AtricleBarcode",
                "ReadyTime",
                "ReadyBy"
            });


        protected void ReleaseSubWorkTaskForAuto()
        {
            //Get all executor of this operator
            var executorList = GetExecutors();
            if ((executorList == null) || (executorList.Count <= 0))
            {
                return;
            }

            foreach (var executor in executorList)
            {
                var connector = ConnectorsRepository.GetConnectorInstance(executor.Connection);
                if (connector == null)
                {
                    continue;
                }

                //Create,Remove alarm automatically
                if (!connector.RecSendMsgStatus)
                {
                    if (!connector.AlarmActiveStatus)
                    {
                        CreateDeviceAlarm(StaticParameterForMessage.Disconnect, OperatorName, executor.Id,
                            AlarmEventGrade.Critical);
                        connector.AlarmActiveStatus = true;
                    }
                    continue;
                }
                if (connector.AlarmActiveStatus)
                {
                    connector.AlarmActiveStatus = false;
                    RemoveDeviceAlarm(StaticParameterForMessage.Disconnect, OperatorName, executor.Id);
                }

                var releaseWorkTaskList = _subWorkTaskQueue.GetWorkTaskForReleaseByLogicalSortter(executor.Id);
                if ((releaseWorkTaskList == null) || (releaseWorkTaskList.Count <= 0))
                {
                    continue;
                }


                foreach (var workTask in from workTask in releaseWorkTaskList
                    let message = JsonConvert.SerializeObject(workTask)
                    let messageList = new List<string> {StaticParameterForSorter.Ds5, message}
                    where connector.SendMessage(messageList)
                    select workTask)
                {
                    _subWorkTaskQueue.ReleaseWorkTask(workTask, OperatorName);
                }
            }
        }

        protected bool ActiveSubWorkTask(ScsClientMessage message, string connectionId)
        {
            foreach (var scsbody in message.ScsBodyList)
            {
                var trcakId = scsbody.ScsBodyFieldList[0].Value.Trim();
                var currentAddr = scsbody.ScsBodyFieldList[2].Value.Substring(8, 6).TrimStart('0');
                var activeTime = ConvertToDateTime(scsbody.ScsBodyFieldList[3].Value.Trim());
                var resultCode = scsbody.ScsBodyFieldList[4].Value.Substring(8, 2).Trim();

                //20160724 
                int circleTime;
                try
                {
                    circleTime=Convert.ToInt32(scsbody.ScsBodyFieldList[5].Value.Substring(8, 4).Trim());
                }
                catch
                {
                    circleTime = 1;
                }


                if (resultCode == StaticParameterForSorter.UnknowPackage)
                {
                    var prefix = ParameterManager.GetParameter<string>(StaticParameterForSorter.MessageWorkTaskPrefix);
                    //If resultCode is UP,create sorter message
                    var messageWorkTask = new SorterMessageWorkTask
                    {
                        Id = SequenceCreator.GetIdBySeqNoAndKey(prefix),
                        TrackingId = trcakId,
                        Type = SorterMessageType.UnknowPackage,
                        Results = resultCode,
                        ActiveTime = activeTime,
                        ActiveBy = OperatorName,
                        CreateTime = DateTime.Now,
                        CreateBy = OperatorName,
                        OperatorName = OperatorName,
                        Connect = connectionId,
                        TriggerMode = WorkTaskTriggerMode.Immediately
                    };

                    _messageWorkTaskQueue.CreateWorkTask(messageWorkTask,
                        e =>
                            (e.Id == message.Id) &&
                            ((int) e.Status < (int) WorkTaskStatus.Completed));

                    _messageWorkTaskQueue.FinishWorkTask(messageWorkTask, OperatorName);

                    continue;
                }

                var workTaskList = _subWorkTaskQueue.GetUnfinishWorkTaskByTrackId(trcakId);
                if ((workTaskList == null) || (workTaskList.Count <= 0))
                {
                    return false;
                }

                foreach (var workTask in workTaskList)
                {
                    var shuteRepositorys = ServiceLocator.Current.GetInstance<IShuteRepository>();
                    var shute = shuteRepositorys.GetValueByDeviceName(currentAddr);
                    var packageNo = StaticParameterForMessage.None;
                    if (shute != null)
                    {
                        packageNo = shute.PackageNo;
                    }

                    decimal packageWeight = 0;
                    var inductMessage = _messageWorkTaskQueue.GetWorkTaskByTracingId(workTask.TrackingId);
                    if (inductMessage != null)
                    {
                        packageWeight = inductMessage.Weight;
                    }

                    var rtnValue = _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
                    {
                        e.SourceStatus = e.Status;
                        e.Status = WorkTaskStatus.Active;
                        e.CurrentShuteAddr = currentAddr;
                        e.SortResultCode = resultCode;
                        e.ActivePackageNo = packageNo;
                        e.ActiveTime = activeTime;
                        e.ActiveBy = OperatorName;
                        e.CycleTimes = circleTime;
                        e.AtricleWeight = packageWeight.ToString(CultureInfo.InvariantCulture);
                        return e;
                    }, StaticParameterForMessage.ActiveSuccess, StaticParameterForMessage.ActiveFailure,
                        new List<string>
                        {
                            "SourceStatus",
                            "Status",
                            "CurrentShuteAddr",
                            "SortResultCode",
                            "ActivePackageNo",
                            "ActiveTime",
                            "ActiveBy",
                            "CycleTimes",
                            "AtricleWeight"
                        });

                    if (inductMessage != null)
                    {
                        _messageWorkTaskQueue.FinishWorkTask(inductMessage, OperatorName);
                    }

                    workTask.AtricleWeight= packageWeight.ToString(CultureInfo.InvariantCulture);
                    SendMessage(workTask, StaticParameterForSorter.NotifyTopOperatorFinishWorkTask, 5);

                    //update package number
                    if (!rtnValue.Item1) continue;
                    if ((shute == null) ||
                        !ParameterManager.GetParameter<bool>(StaticParameterForSorter.IfUpdatePackageNumber)) continue;
                    shute.TheLastPackageNumber = shute.PackageNumber;
                    shute.PackageNumber = shute.PackageNumber + 1;

                    shuteRepositorys.Update(shute);
                }
            }


            return true;
        }

        protected Tuple<bool, string> FinishSubWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);

            var rtnValue = _subWorkTaskQueue.IsExistWorkTask(workTaskId);
            if (!rtnValue.Item1)
            {
                return rtnValue;
            }

            var workTask = _subWorkTaskQueue.TryGetValue(workTaskId);
            return FinishSubWorkTask(workTask);
        }


        protected Tuple<bool, string> FinishSubWorkTask(SorterSubWorkTask workTask)
        {
            //var shuteRepositorys = ServiceLocator.Current.GetInstance<IShuteRepository>();
            //var shute = shuteRepositorys.GetValueByDeviceName(workTask.FinalShuteAddr);
            //var packageNo = StaticParameterForMessage.None;
            //if (shute != null)
            //{
            //    packageNo = shute.PackageNo;
            //}

         
            var rtnValue = _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
            {
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Completed;
                e.FinalShuteAddr = workTask.FinalShuteAddr;
                e.FinalCarrierId = workTask.FinalCarrierId;          
                e.CompleteTime = workTask.CompleteTime;
                return e;
            }, StaticParameterForMessage.FinishSuccess, StaticParameterForMessage.FinishFailure,
                new List<string>
                {
                    "SourceStatus",
                    "Status",
                    "FinalShuteAddr",
                    "FinalCarrierId",
                    "CompleteTime"
                });

            if (!rtnValue.Item1)
            {
                return rtnValue;
            }

            //if (inductMessage != null)
            //{
            //    _messageWorkTaskQueue.FinishWorkTask(inductMessage, OperatorName);
            //}

            //If no recive active message,send top operator finish work task when receive finish message
            if (workTask.SourceStatus < WorkTaskStatus.Active)
            {
                workTask.CurrentShuteAddr = workTask.FinalShuteAddr;
                workTask.Results = "OK";
                workTask.ActiveBy = OperatorName;
                workTask.ActiveTime = DateTime.Now;
                workTask.CycleTimes = 1;
                SendMessage(workTask, StaticParameterForSorter.NotifyTopOperatorFinishWorkTask, 5);
            }

            if (!ParameterManager.GetParameter<bool>(StaticParameterForSorter.IfFinishExecuteWorkTask))
                return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
            //Don't finish execute work task in simulation mode
            if (!IsFinishExecuteWorkTask(workTask.ExecuteWorkTaskId))
                return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
            var executeWorkTask = _executeWorkTaskRepository.TryGetValue(workTask.ExecuteWorkTaskId);
            return executeWorkTask != null
                ? _executeWorkTaskRepository.FinishWorkTask(executeWorkTask, OperatorName)
                : new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }

        protected void FinishSubWorkTaskForAuto()
        {
            if (!ParameterManager.GetParameter<bool>(StaticParameterForSorter.Simulation))
            {
                return;
            }

            //var workTaskList = _subWorkTaskQueue.GetReleaseWorkTask();
            //if ((workTaskList == null) || (workTaskList.Count <= 0))
            //{
            //    return;
            //}
            //foreach (var workTask in workTaskList)
            //{
            //    FinishSubWorkTask(workTask);
            //}
        }

        protected bool FinishSubWorkTask(ScsClientMessage message)
        {
            foreach (var scsbody in message.ScsBodyList)
            {
                var trcakId = scsbody.ScsBodyFieldList[0].Value.Trim();
                var finalAddr = scsbody.ScsBodyFieldList[2].Value.Substring(8, 6).Trim().TrimStart('0');
                var finalCarrierId = scsbody.ScsBodyFieldList[3].Value.Substring(8, 6).Trim();
                var finalTime = ConvertToDateTime(scsbody.ScsBodyFieldList[4].Value.Trim());

                var workTaskList = _subWorkTaskQueue.GetUnfinishWorkTaskByTrackId(trcakId);
                if ((workTaskList == null) || (workTaskList.Count <= 0))
                {
                    return false;
                }

                foreach (var workTask in workTaskList)
                {
                    workTask.FinalShuteAddr = finalAddr;
                    workTask.FinalCarrierId = finalCarrierId;
                    workTask.CompleteTime = finalTime;
                    if ((workTask.RequestShuteAddr == workTask.FinalShuteAddr)
                        || (workTask.RequestShuteAddrX == workTask.FinalShuteAddr)
                        || (workTask.RequestShuteAddrY == workTask.FinalShuteAddr)
                        || (workTask.RequestShuteAddrZ == workTask.FinalShuteAddr))
                    {
                        FinishSubWorkTask(workTask);
                    }
                    else
                    {                     
                        _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
                        {
                            e.SourceStatus = e.Status;
                            e.Status = WorkTaskStatus.Faulted;
                            e.FinalShuteAddr = finalAddr;
                            e.FinalCarrierId = finalCarrierId;
                            e.FaultTime = finalTime;
                            e.Results = StaticParameterForMessage.Ok;
                            return e;
                        }, StaticParameterForMessage.FaultSuccess, StaticParameterForMessage.FaultFailure,
                            new List<string>
                            {
                                "SourceStatus",
                                "Status",
                                "FinalShuteAddr",
                                "FinalCarrierId",
                                "FaultTime",
                                "Results"
                            });
                 
                       SendMessage(workTask, StaticParameterForSorter.NotifyTopOperatorReposeWorkTask, 5);
                    }
                }
            }


            return true;
        }

        protected Tuple<bool, string> CancelSubWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var cancelBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.CancelledBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var rtnValue = _subWorkTaskQueue.IsExistWorkTask(workTaskId);
            if (!rtnValue.Item1)
            {
                return rtnValue;
            }

            var workTask = _subWorkTaskQueue.TryGetValue(workTaskId);
            return _subWorkTaskQueue.CancelWorkTask(workTask, cancelBy, reason);
        }

        internal void CancelSubWorkTask(List<SorterSubWorkTask> workTaskList, string cancelBy, string reason)
        {
            if ((workTaskList == null) || (workTaskList.Count <= 0))
            {
                return;
            }

            foreach (var workTask in workTaskList)
            {
                _subWorkTaskQueue.CancelWorkTask(workTask, cancelBy, reason);
            }
        }

        protected bool SuspendSubWorkTask(ScsClientMessage message)
        {
            foreach (var scsbody in message.ScsBodyList)
            {
                var trcakId = scsbody.ScsBodyFieldList[0].Value.Trim();
                //var finalAddr = scsbody.ScsBodyFieldList[2].Value.Substring(8, 6).Trim().TrimStart('0');
                var finalAddr = "1007";
                var sorterResult = scsbody.ScsBodyFieldList[3].Value.Substring(8, 2).Trim();
                var finalCarryId = scsbody.ScsBodyFieldList[4].Value.Substring(8, 6).Trim();
                var suspendTime = ConvertToDateTime(scsbody.ScsBodyFieldList[5].Value.Trim());

                var workTaskList = _subWorkTaskQueue.GetUnfinishWorkTaskByTrackId(trcakId);
                if ((workTaskList == null) || (workTaskList.Count <= 0))
                {
                    return false;
                }

                foreach (var workTask in workTaskList)
                {
                    _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
                    {
                        e.SourceStatus = e.Status;
                        e.Status = WorkTaskStatus.Faulted;
                        e.FinalShuteAddr = finalAddr;
                        e.FinalCarrierId = finalCarryId;
                        e.SortResultSorter = sorterResult;
                        e.SuspendedBy = OperatorName;
                        e.FaultTime = suspendTime;
                        return e;
                    }, StaticParameterForMessage.CancelSuccess, StaticParameterForMessage.CancelFailure,
                        new List<string>
                        {
                            "SourceStatus",
                            "Status",
                            "FinalShuteAddr",
                            "FinalCarrierId",
                            "SortResultSorter",
                            "SuspendedBy",
                            "FaultTime"
                        });

                    SendMessage(workTask, StaticParameterForSorter.NotifyTopOperatorFinishWorkTask, 5);
                }
            }
            return true;
        }

        protected Tuple<bool, string> TerminateSubWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();
            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var terminateBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.TerminatedBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var rtnValue = _subWorkTaskQueue.IsExistWorkTask(workTaskId);
            if (!rtnValue.Item1)
            {
                return rtnValue;
            }

            var workTask = _subWorkTaskQueue.TryGetValue(workTaskId);
            return TerminateSubWorkTask(workTask, terminateBy, reason);
        }

        internal void TerminateSubWorkTask(List<SorterSubWorkTask> workTaskList, string terminateBy, string reason)
        {
            if ((workTaskList == null) || (workTaskList.Count <= 0))
            {
                return;
            }

            foreach (var workTask in workTaskList)
            {
                TerminateSubWorkTask(workTask, terminateBy, reason);
            }
        }

        internal Tuple<bool, string> TerminateSubWorkTask(SorterSubWorkTask workTask, string terminateBy, string reason)
        {
            var shuteRepositorys = ServiceLocator.Current.GetInstance<IShuteRepository>();
            var shute = shuteRepositorys.GetValueByDeviceName(workTask.RequestShuteAddr);
            var packageNo = StaticParameterForMessage.None;
            if (shute != null)
            {
                packageNo = shute.PackageNo;
            }

            decimal packageWeight = 0;
            var inductMessage = _messageWorkTaskQueue.TryGetValue(workTask.TrackingId);
            if (inductMessage != null)
            {
                packageWeight = inductMessage.Weight;
            }

            var rtnValue = _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
            {
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Terminated;
                e.TerminatedBy = terminateBy;
                e.Comments = reason;
                e.AtricleWeight = packageWeight.ToString(CultureInfo.InvariantCulture);
                e.FinishPackageNo = packageNo;
                return e;
            }, StaticParameterForMessage.TerminateSuccess, StaticParameterForMessage.TerminateFailure,
                new List<string>
                {
                    "SourceStatus",
                    "Status",
                    "TerminatedBy",
                    "Comments",
                    "AtricleWeight",
                    "FinishPackageNo"
                });

            if (rtnValue.Item1)
            {
                SendMessage(workTask, StaticParameterForSorter.NotifyTopOperatorFinishWorkTask, workTask.OperatorName, 5);
            }


            if (!ParameterManager.GetParameter<bool>(StaticParameterForSorter.IfFinishExecuteWorkTask))
                return rtnValue;
            //Don't finish execute work task in simulation mode
            if (!IsFinishExecuteWorkTask(workTask.ExecuteWorkTaskId))
                return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
            var executeWorkTask = _executeWorkTaskRepository.TryGetValue(workTask.ExecuteWorkTaskId);
            return executeWorkTask != null
                ? _executeWorkTaskRepository.FinishWorkTask(executeWorkTask, OperatorName)
                : new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }

        protected bool FaultSubWorkTask(ScsClientMessage message)
        {
            foreach (var scsbody in message.ScsBodyList)
            {
                var trcakId = scsbody.ScsBodyFieldList[0].Value.Trim();
                var finalAddr = scsbody.ScsBodyFieldList[2].Value.Substring(8, 6).Trim().TrimStart('0');
                var finalCarryId = scsbody.ScsBodyFieldList[3].Value.Substring(8, 6).Trim();
                var falutTime = ConvertToDateTime(scsbody.ScsBodyFieldList[4].Value.Trim());

                var workTaskList = _subWorkTaskQueue.GetUnfinishWorkTaskByTrackId(trcakId);
                if ((workTaskList == null) || (workTaskList.Count <= 0))
                {
                    return false;
                }

                foreach (var workTask in workTaskList)
                {
                    decimal packageWeight = 0;
                    var inductMessage = _messageWorkTaskQueue.GetWorkTaskByTracingId(workTask.TrackingId);
                    if (inductMessage != null)
                    {
                        packageWeight = inductMessage.Weight;
                    }

                    _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
                    {
                        e.SourceStatus = e.Status;
                        e.Status = WorkTaskStatus.Completed;
                        e.FinalShuteAddr = finalAddr;
                        e.FinalCarrierId = finalCarryId;
                        e.FaultTime = falutTime;
                        e.AtricleWeight = packageWeight.ToString(CultureInfo.InvariantCulture);
                        return e;
                    }, StaticParameterForMessage.FaultSuccess, StaticParameterForMessage.FaultFailure,
                        new List<string>
                        {
                            "SourceStatus",
                            "Status",
                            "FinalShuteAddr",
                            "FinalCarrierId",
                            "FaultTime",
                            "AtricleWeight"
                        });

                    if (inductMessage != null)
                    {
                        _messageWorkTaskQueue.FinishWorkTask(inductMessage, OperatorName);
                    }
                }
            }


            return true;
        }


        protected void FaultTimeoutSubWorkTaskForAuto()
        {
            var timeoutWorkTaskList = _subWorkTaskQueue.GetTimeOutWorkTask(_timeoutSecondForSubWorkTask);
            if ((timeoutWorkTaskList != null) && (timeoutWorkTaskList.Count > 0))
            {
                foreach (var workTask in timeoutWorkTaskList)
                {
                    _subWorkTaskQueue.FaultWorkTask(workTask, OperatorName, StaticParameterForMessage.Timeout);
                }
            }
        }

        protected void FaultTimeoutExecuteWorkTaskForAuto()
        {
            var timeoutWorkTaskList = _executeWorkTaskRepository.GetTimeOutWorkTask(_timeoutDayForExecuteWorkTask);
            if ((timeoutWorkTaskList != null) && (timeoutWorkTaskList.Count > 0))
            {
                foreach (var workTask in timeoutWorkTaskList)
                {
                    _executeWorkTaskRepository.TerminateWorkTask(workTask, OperatorName,
                        StaticParameterForMessage.Timeout);
                }
            }
        }

        internal Tuple<bool, string> ReadySubWorkTaskForComplement(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskId = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var objectToHandle = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ObjectToHandle);
            var requestShuteNum = JSonHelper.GetValue<string>(jObject, StaticParameterForSorter.RequestShuteNum);
            var requestShuteAddr = JSonHelper.GetValue<string>(jObject, StaticParameterForSorter.RequestShuteAddr);
            var requestShuteAddrX = JSonHelper.GetValue<string>(jObject, StaticParameterForSorter.RequestShuteAddrX);
            var requestShuteAddrY = JSonHelper.GetValue<string>(jObject, StaticParameterForSorter.RequestShuteAddrY);
            var requestShuteAddrZ = JSonHelper.GetValue<string>(jObject, StaticParameterForSorter.RequestShuteAddrZ);


            var rtnValue = _subWorkTaskQueue.IsExistWorkTask(workTaskId);
            if (!rtnValue.Item1)
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.Failure);
            }

            var workTask = _subWorkTaskQueue.TryGetValue(workTaskId);

            return _subWorkTaskQueue.UpdateWorkTask(workTask, e =>
            {
                //Ready
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Ready;
                e.Results = StaticParameterForSorter.DisChargeNd;
                e.ObjectToHandle = objectToHandle;
                e.RequestShuteNum = requestShuteNum;
                e.RequestShuteAddr = requestShuteAddr;
                e.RequestShuteAddrX = requestShuteAddrX;
                e.RequestShuteAddrY = requestShuteAddrY;
                e.RequestShuteAddrZ = requestShuteAddrZ;
                e.TriggerMode = WorkTaskTriggerMode.Immediately;
                e.ReadyTime = DateTime.Now;
                e.ReadyBy = OperatorName;
                return e;
            }, StaticParameterForMessage.UpdateSuccess, StaticParameterForMessage.UpdateFailure, new List<string>
            {
                "SourceStatus",
                "Status",
                "Results",
                "ObjectToHandle",
                "RequestShuteNum",
                "RequestShuteAddr",
                "RequestShuteAddrX",
                "RequestShuteAddrY",
                "RequestShuteAddrZ",
                "TriggerMode",
                "ReadyTime",
                "ReadyBy"
            });
        }
     
    }
}