using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Operator.Common.Queues
{
    public class SmartWorkTaskQueue<TKey, TValue> : WorkTaskQueue<TKey, TValue> where TValue : WorkTask<TKey>, new()
    {
        public string LogName;

        public SmartWorkTaskQueue(IQueryableUnitOfWork unitOfWork, Expression<Func<TValue, bool>> filter)
            : base(unitOfWork, filter)
        {
        }

        public Tuple<bool, string> CreateWorkTask(TValue workTask,
            Func<TValue, bool> filterMethod)
        {
            var messageInformation = workTask.ToShortString();
            workTask.Status = WorkTaskStatus.Create;
            workTask.CreateTime = DateTime.Now;
            workTask.ReadyBy = null;
            workTask.ReadyTime = null;
            workTask.ReleaseBy = null;
            workTask.ReleaseTime = null;
            workTask.ActiveBy = null;
            workTask.ActiveTime = null;
            workTask.SuspendedBy = null;
            workTask.SuspendedTime = null;
            workTask.ResumeBy = null;
            workTask.ResumeTime = null;
            workTask.CancelledBy = null;
            workTask.CancelledTime = null;
            workTask.TerminatedBy = null;
            workTask.TerminatedTime = null;
            workTask.UpdateBy = null;
            workTask.UpdateTime = null;
            workTask.CompleteTime = null;
            workTask.FaultTime = null;
            var returnValue = Create(workTask, filterMethod);
            string messageCode;
            if (returnValue)
            {
                messageCode = StaticParameterForMessage.CreateSuccess;
                LogRepository.WriteInfomationLog(LogName, messageCode, messageInformation);
            }
            else
            {
                messageCode = StaticParameterForMessage.CreateFailure;
                LogRepository.WriteErrorLog(LogName, messageCode, messageInformation);
            }

            return new Tuple<bool, string>(returnValue, messageCode);
        }

        public Tuple<bool, string> UpdateWorkTask(TValue workTask, Func<TValue, TValue> newValueFunction,
            Func<TValue, bool> validateFunction, bool ifWriteLog,
            List<string> modifiedProperties = null)
        {
            var returnValue = Update(workTask.Id, newValueFunction, validateFunction, modifiedProperties);

            var messageInformation = workTask.ToShortString();
            string messageCode;
            if (returnValue)
            {
                messageCode = StaticParameterForMessage.UpdateSuccess;
                if (ifWriteLog)
                {
                    LogRepository.WriteInfomationLog(LogName, messageCode, messageInformation);
                }
            }
            else
            {
                messageCode = StaticParameterForMessage.UpdateFailure;
                if (ifWriteLog)
                {
                    LogRepository.WriteErrorLog(LogName, messageCode, messageInformation);
                }
            }

            return new Tuple<bool, string>(returnValue, messageCode);
        }

        public Tuple<bool, string> UpdateWorkTask(TValue workTask, Func<TValue, TValue> tNewValueFunction,
            string successMessage,
            string failureMessage,
            List<string> modifiedProperties = null)
        {
            var returnValue = Update(workTask.Id, tNewValueFunction, r => r.Status <= WorkTaskStatus.Completed,
                modifiedProperties);

            var messageInformation = workTask.ToShortString();
            string messageCode;
            if (returnValue)
            {
                messageCode = successMessage;
                LogRepository.WriteInfomationLog(LogName, messageCode, messageInformation);
            }
            else
            {
                messageCode = failureMessage;
                LogRepository.WriteErrorLog(LogName, messageCode, messageInformation);
            }

            return new Tuple<bool, string>(returnValue, messageCode);
        }

        //public Tuple<bool, string> CreateAndUpdateWorkTask(TValue workTask, Func<TValue, bool> filterMethod,
        //    bool ifUpdate,
        //    WorkTaskStatus lessThanStatusForUpdate)
        //{
        //    workTask.Status = WorkTaskStatus.Create;
        //    workTask.CreateTime = DateTime.Now;
        //    var rtnValue = CreateWorkTask(workTask, filterMethod);
        //    if (rtnValue.Item1)
        //    {
        //        return rtnValue;
        //    }

        //    if (!ifUpdate)
        //    {
        //        return new Tuple<bool, string>(false, StaticParameterForMessage.UpdateFailure);
        //    }

        //    return UpdateWorkTask(workTask, r => workTask,
        //        r => r.Status <= lessThanStatusForUpdate, true);
        //}

        public Tuple<bool, string> ReadyWorkTask(TValue workTask, Func<TValue, TValue> tNewValueFunction,
            string releaseBy) => UpdateWorkTask(workTask, tNewValueFunction, StaticParameterForMessage.ReadySuccess,
                StaticParameterForMessage.ReadyFailure);

        public Tuple<bool, string> RenewWorkTask(TValue workTask, string readyBy, string reason)
            => UpdateWorkTask(workTask, e =>
            {
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Ready;
                e.ReadyTime = DateTime.Now;
                e.ReadyBy = readyBy;
                e.Comments = reason;
                return e;
            }, StaticParameterForMessage.RenewSuccess, StaticParameterForMessage.RenewFailure, new List<string>
            {
                "SourceStatus",
                "Status",
                "ReadyTime",
                "ReadyBy",
                "Comments"
            });

        public Tuple<bool, string> ReleaseWorkTask(TValue workTask, string releaseBy) => UpdateWorkTask(workTask, e =>
        {
            e.SourceStatus = e.Status;
            e.Status = WorkTaskStatus.Release;
            e.ReleaseTime = DateTime.Now;
            e.ReleaseBy = releaseBy;
            return e;
        }, StaticParameterForMessage.ReleaseSuccess, StaticParameterForMessage.ReleaseFailure, new List<string>
        {
            "SourceStatus",
            "Status",
            "ReleaseTime",
            "ReleaseBy"
        });

        public Tuple<bool, string> ReleaseWorkTask(TValue workTask, Func<TValue, TValue> tNewValueFunction,
            string releaseBy) => UpdateWorkTask(workTask, tNewValueFunction, StaticParameterForMessage.ReleaseSuccess,
                StaticParameterForMessage.ReleaseFailure);

        public Tuple<bool, string> ActiveWorkTask(TValue workTask, string activeBy) => UpdateWorkTask(workTask, e =>
        {
            e.SourceStatus = e.Status;
            e.Status = WorkTaskStatus.Active;
            e.ActiveTime = DateTime.Now;
            e.ActiveBy = activeBy;
            return e;
        }, StaticParameterForMessage.ActiveSuccess, StaticParameterForMessage.ActiveFailure, new List<string>
        {
            "SourceStatus",
            "Status",
            "ActiveTime",
            "ActiveBy"
        });

        public Tuple<bool, string> SuspendWorkTask(TValue workTask, string suspendedBy, string reason)
            => UpdateWorkTask(workTask, e =>
            {
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Suspended;
                e.SuspendedTime = DateTime.Now;
                e.SuspendedBy = suspendedBy;
                e.Comments = reason;
                return e;
            }, StaticParameterForMessage.SuspendSuccess, StaticParameterForMessage.SuspendFailure, new List<string>
            {
                "SourceStatus",
                "Status",
                "SuspendedTime",
                "SuspendedBy",
                "Comments"
            });

        public Tuple<bool, string> SuspendWorkTask(TValue workTask, Func<TValue, TValue> tNewValueFunction)
            => UpdateWorkTask(workTask, tNewValueFunction, StaticParameterForMessage.SuspendSuccess,
                StaticParameterForMessage.SuspendFailure);

        public Tuple<bool, string> FinishWorkTask(TValue workTask, string finishBy) => UpdateWorkTask(workTask, e =>
        {
            e.SourceStatus = e.Status;
            e.Status = WorkTaskStatus.Completed;
            e.CompleteTime = DateTime.Now;
            e.UpdateTime = DateTime.Now;
            e.UpdateBy = finishBy;
            return e;
        }, StaticParameterForMessage.FinishSuccess, StaticParameterForMessage.FinishFailure, new List<string>
        {
            "SourceStatus",
            "Status",
            "CompleteTime",
            "UpdateTime",
            "UpdateBy"
        });

        public Tuple<bool, string> TerminateWorkTask(TValue workTask, string terminatedBy, string reason)
            => UpdateWorkTask(workTask, e =>
            {
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Terminated;
                e.TerminatedTime = DateTime.Now;
                e.TerminatedBy = terminatedBy;
                e.Comments = reason;
                return e;
            }, StaticParameterForMessage.TerminateSuccess, StaticParameterForMessage.TerminateFailure, new List<string>
            {
                "SourceStatus",
                "Status",
                "TerminatedTime",
                "TerminatedBy",
                "Comments"
            });

        public Tuple<bool, string> CancelWorkTask(TValue workTask, string cancelBy, string reason)
            => UpdateWorkTask(workTask, e =>
            {
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Cancelled;
                e.CancelledTime = DateTime.Now;
                e.CancelledBy = cancelBy;
                e.Comments = reason;
                return e;
            },
                StaticParameterForMessage.CancelSuccess, StaticParameterForMessage.CancelFailure, new List<string>
                {
                    "SourceStatus",
                    "Status",
                    "CancelledTime",
                    "CancelledBy",
                    "Comments"
                });

        public Tuple<bool, string> CancelWorkTask(TValue workTask, Func<TValue, TValue> tNewValueFunction)
            => UpdateWorkTask(workTask, tNewValueFunction, StaticParameterForMessage.CancelSuccess,
                StaticParameterForMessage.CancelFailure);

        public Tuple<bool, string> FaultWorkTask(TValue workTask, string faultBy, string reason)
            => UpdateWorkTask(workTask, e =>
            {
                e.SourceStatus = e.Status;
                e.Status = WorkTaskStatus.Faulted;
                e.FaultTime = DateTime.Now;
                e.UpdateBy = faultBy;
                e.Comments = reason;
                return e;
            }, StaticParameterForMessage.FaultSuccess, StaticParameterForMessage.FaultFailure, new List<string>
            {
                "SourceStatus",
                "Status",
                "FaultTime",
                "UpdateBy",
                "Comments"
            });

        public Tuple<bool, string> IsExistWorkTask(TKey workTaskId)
        {
            if (IsExist(workTaskId))
            {
                return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
            }

            const string messageCode = StaticParameterForMessage.WorkTaskIsNotExist;
            return new Tuple<bool, string>(false, messageCode);
        }

        public Tuple<bool, string> IsExistWorkTaskByObject(string objectToHandle)
        {
            var isExist = Values.Any(e => e.ObjectToHandle == objectToHandle);
            if (isExist)
            {
                return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
            }
            const string messageCode = StaticParameterForMessage.WorkTaskIsNotExist;
            return new Tuple<bool, string>(false, messageCode);
        }

        public TValue GetWorkTaskByObject(string objectToHandle)
        {
            var workTaskList = Values.Where(e => e.ObjectToHandle == objectToHandle).ToList();
            return workTaskList.Count <= 0 ? null : workTaskList.FirstOrDefault();
        }

        private TValue GetWorkTaskForOnlyOne(Func<TValue, bool> filterFunction,
            string objectMessage)
        {
            var filterWorkTaskList = Values.Where(filterFunction).ToList();
            return IsOnlyOneWorkTask(filterWorkTaskList, objectMessage).Item1 ? filterWorkTaskList[0] : null;
        }

        public TValue GetRunningWorkTaskForOnlyOne(string objectToHandle) => GetWorkTaskForOnlyOne(e =>
            ((e.Status == WorkTaskStatus.Active) || (e.Status == WorkTaskStatus.Release)) &&
            (e.ObjectToHandle == objectToHandle), objectToHandle);

        public TValue GetReadyWorkTaskForOnlyOne(string objectToHandle) => GetWorkTaskForOnlyOne(e =>
            ((e.Status == WorkTaskStatus.Create) || (e.Status == WorkTaskStatus.Ready)) &&
            (e.ObjectToHandle == objectToHandle), objectToHandle);

        public Tuple<bool, string> IsOnlyOneWorkTask(List<TValue> valueList, string objectMessage)
        {
            string messageCode;
            if (valueList.Count <= 0)
            {
                messageCode = StaticParameterForMessage.WorkTaskIsNotExist;
                return new Tuple<bool, string>(false, messageCode);
            }

            if (valueList.Count >= 2)
            {
                messageCode = StaticParameterForMessage.MultiWorkTaskExist;
                return new Tuple<bool, string>(false, messageCode);
            }

            return new Tuple<bool, string>(true, StaticParameterForMessage.Ok);
        }
    }
}