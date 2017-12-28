using System;
using System.Linq;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.SdsSimulation;
using Kengic.Was.Domain.Entity.Sorter.WorkTasks;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Operator.Common.Methods;
using Kengic.Was.Operator.SdsSimulation.Parameters;
using Kengic.Was.Operator.SdsSimulation.Queues;
using Newtonsoft.Json.Linq;

namespace Kengic.Was.Operator.SdsSimulation.Methods
{
    public class CommonExecuteMethod : OperatorFunctionMethod
    {
        private SdsSimulationSourceWorkTaskQueue _sourceWorkTaskQueue;

        protected override void InitializeFunction()
        {
            const string sourceWorkTaskQueueName = StaticParameterForWorkTask.SourceWorkTaskQueueName;
            if (QueueManager.IsExistQueue(sourceWorkTaskQueueName))
            {
                _sourceWorkTaskQueue = (SdsSimulationSourceWorkTaskQueue) QueueManager.GetQueue(sourceWorkTaskQueueName);
            }


            base.InitializeFunction();
        }

        protected Tuple<bool, string> CreateSourceWorkTask(string message)
        {
            var workTask = message.JsonToValue<SdsSimulationSourceWorkTask>();

            if (!string.IsNullOrEmpty(workTask.Id)) return CreateSourceWorkTask(workTask);
            var prefix = ParameterManager.GetParameter<string>(StaticParameterForSdsSimulation.SourceWorkTaskPrefix);
            workTask.Id = SequenceCreator.GetIdBySeqNoAndKey(prefix);

            return CreateSourceWorkTask(workTask);
        }


        protected void CreateSourceWorkTaskForAuto()
        {
            if (!ParameterManager.GetParameter<bool>(StaticParameterForSdsSimulation.Simulation))
            {
                return;
            }

            var logicalDestinationRandom = new Random();

            var prefix = ParameterManager.GetParameter<string>(StaticParameterForSdsSimulation.SourceWorkTaskPrefix);
            var barcode = SequenceCreator.GetIdBySeqNoAndKey("VIP");
            var workTask = new SdsSimulationSourceWorkTask
            {
                Id = SequenceCreator.GetIdBySeqNoAndKey(prefix),
                ObjectToHandle = barcode,
                LogisticsBarcode = barcode,
                LogicalDestination = $"L1{logicalDestinationRandom.Next(1, 50).ToString("000")}",
                OperatorName = OperatorName,
                SortterTaskMode = SortterTaskMode.Unique,
                TriggerMode = WorkTaskTriggerMode.Immediately
            };


            CreateSourceWorkTask(workTask);
        }

        protected Tuple<bool, string> CreateSourceWorkTask(SdsSimulationSourceWorkTask workTask)
            => _sourceWorkTaskQueue.CreateWorkTask(workTask, e =>
                (e.Id == workTask.Id) &&
                ((int) e.Status < (int) WorkTaskStatus.Completed));

        protected void ReadySourceWorkTaskForAuto()
        {
            var readyWorkTaskList = _sourceWorkTaskQueue.GetCreateWorkTask();

            if ((readyWorkTaskList == null) || (readyWorkTaskList.Count <= 0))
            {
                return;
            }
            foreach (
                var releaseWorkTask in
                    readyWorkTaskList.Where(
                        releaseWorkTask =>
                            SendMessage(releaseWorkTask, StaticParameterForSdsSimulation.NotifySubOperatorCreateWorkTask,
                                5)
                                .Item1))
            {
                _sourceWorkTaskQueue.ReleaseWorkTask(releaseWorkTask, OperatorName);
            }
        }


        protected void ReleaseSourceWorkTaskForAuto()
        {
            var workTaskList = _sourceWorkTaskQueue.GetReleaseWorkTask();

            if ((workTaskList == null) || (workTaskList.Count <= 0))
            {
                return;
            }
            foreach (var workTask in workTaskList)
            {
                var inductRandom = new Random();

                var barcode = $"30|61|0123|L0320W0250H0200|X0250Y0200|02|12;{workTask.ObjectToHandle};12;P186558502786|";
                var subWorkTask = new SorterSubWorkTask
                {
                    Id = workTask.Id,
                    WorkTaskType = WorkTaskType.Sorting,
                    TrackingId = Guid.NewGuid().ToString("N"),
                    Induct = $"00040{inductRandom.Next(1, 6)}",
                    Scanner = "000061",
                    CarrierId = "CAI001",
                    ScannerBarcode = barcode,
                    ObjectToHandle = barcode,
                    CreateBy = OperatorName,
                    OperatorName = OperatorName,
                    TriggerMode = WorkTaskTriggerMode.Immediately
                };

                if (SendMessage(subWorkTask, StaticParameterForSdsSimulation.NotifySubOperatorCreateDr, 10).Item1)
                {
                    _sourceWorkTaskQueue.ActiveWorkTask(workTask, OperatorName);
                }
            }
        }

        protected Tuple<bool, string> FinishSourceWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var containerCode = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ObjectToHandle);

            var workTask = _sourceWorkTaskQueue.GetRunningWorkTaskForOnlyOne(containerCode);
            return workTask == null
                ? new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist)
                : FinishSourceWorkTask(workTask);
        }


        internal Tuple<bool, string> FinishSourceWorkTask(SdsSimulationSourceWorkTask workTask)
            => _sourceWorkTaskQueue.FinishWorkTask(workTask, OperatorName);


        protected Tuple<bool, string> TerminateSourceWorkTask(string message, bool isUiMessage)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskid = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var terminateBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.TerminatedBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var rtnValue = _sourceWorkTaskQueue.IsExistWorkTask(workTaskid);
            if (!rtnValue.Item1)
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }


            var sourceWorkTask = _sourceWorkTaskQueue.TryGetValue(workTaskid);
            return _sourceWorkTaskQueue.TerminateWorkTask(sourceWorkTask, terminateBy, reason);
        }

        internal Tuple<bool, string> TerminateSourceWorkTask(SdsSimulationSourceWorkTask sourceWorkTask,
            string terminateBy,
            string reason,
            string notifyMessageId)
        {
            var rtnValue = _sourceWorkTaskQueue.TerminateWorkTask(sourceWorkTask, terminateBy, reason);
            return !rtnValue.Item1 ? rtnValue : SendMessage(sourceWorkTask, notifyMessageId, 5);
        }


        protected Tuple<bool, string> CancelSourceWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskid = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var cancelBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.CancelledBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var rtnValue = _sourceWorkTaskQueue.IsExistWorkTask(workTaskid);
            if (!rtnValue.Item1)
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }


            var sourceWorkTask = _sourceWorkTaskQueue.TryGetValue(workTaskid);

            return _sourceWorkTaskQueue.CancelWorkTask(sourceWorkTask, cancelBy, reason);
        }


        internal Tuple<bool, string> CancelSourceWorkTask(SdsSimulationSourceWorkTask sourceWorkTask, string cancelBy,
            string reason,
            string notifyMessageId)
        {
            var rtnValue = _sourceWorkTaskQueue.CancelWorkTask(sourceWorkTask, cancelBy, reason);
            return !rtnValue.Item1 ? rtnValue : SendMessage(sourceWorkTask, notifyMessageId, 5);
        }


        protected Tuple<bool, string> RenewSourceWorkTask(string message)
        {
            var jObject = message.JsonToValue<JObject>();

            var workTaskid = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Id);
            var readyBy = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.ReadyBy);
            var reason = JSonHelper.GetValue<string>(jObject, StaticParameterForWorkTask.Comments);

            var rtnValue = _sourceWorkTaskQueue.IsExistWorkTask(workTaskid);
            if (!rtnValue.Item1)
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }


            var workTask = _sourceWorkTaskQueue.TryGetValue(workTaskid);

            return _sourceWorkTaskQueue.RenewWorkTask(workTask, readyBy, reason);
        }
    }
}