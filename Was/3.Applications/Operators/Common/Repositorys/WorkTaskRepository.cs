using System;
using System.Collections.Generic;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.WorkTask;
using Kengic.Was.Domain.Entity.WorkTask.WorkTasks;

namespace Kengic.Was.Operator.Common.Repositorys
{
    public class WorkTaskRepository<TKey, TValue> : RepositoryForOnlyDb<TKey, TValue>
        where TValue : WorkTask<TKey>, new()
    {
        public WorkTaskRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }

        public Tuple<bool, string> CreateWorkTask(TValue value)
        {
            if ((value == null) || EqualityComparer<TKey>.Default.Equals(value.Id, default(TKey)))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            var messageInformation = value.ToShortString();

            try
            {
                value.Status = WorkTaskStatus.Create;
                value.CreateTime = DateTime.Now;
                var rtnValue = Create(value);

                LogRepository.WriteErrorLog(LogName, rtnValue.Item2, messageInformation);
                return rtnValue;
            }
            catch (Exception ex)
            {
                const string messageCode = StaticParameterForMessage.CreateException;
                LogRepository.WriteErrorLog(LogName, messageCode, ex.ToString());
                return new Tuple<bool, string>(false, messageCode);
            }
        }

        public Tuple<bool, string> UpdateWorkTask(TValue value, string successMessage,
            string exceptionMessage)
        {
            if ((value == null) || EqualityComparer<TKey>.Default.Equals(value.Id, default(TKey)))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            var messageInformation = value.ToShortString();

            try
            {
                Update(value);

                LogRepository.WriteErrorLog(LogName, successMessage, messageInformation);

                return new Tuple<bool, string>(true, successMessage);
            }
            catch (Exception ex)
            {
                LogRepository.WriteErrorLog(LogName, exceptionMessage, ex.ToString());
                return new Tuple<bool, string>(false, exceptionMessage);
            }
        }

        public Tuple<bool, string> RenewWorkTask(TValue value, string readyBy, string reason)
        {
            value.SourceStatus = value.Status;
            value.Status = WorkTaskStatus.Ready;
            value.ReadyTime = DateTime.Now;
            value.ReadyBy = readyBy;
            value.Comments = reason;
            return UpdateWorkTask(value, StaticParameterForMessage.RenewSuccess,
                StaticParameterForMessage.RenewException);
        }

        public Tuple<bool, string> ReleaseWorkTask(TValue value, string releaseBy)
        {
            value.SourceStatus = value.Status;
            value.Status = WorkTaskStatus.Release;
            value.ReleaseTime = DateTime.Now;
            value.ReleaseBy = releaseBy;
            return UpdateWorkTask(value, StaticParameterForMessage.ReleaseSuccess,
                StaticParameterForMessage.ReleaseException);
        }

        public Tuple<bool, string> ActiveWorkTask(TValue value, string activeBy)
        {
            value.SourceStatus = value.Status;
            value.Status = WorkTaskStatus.Active;
            value.ActiveTime = DateTime.Now;
            value.ActiveBy = activeBy;
            return UpdateWorkTask(value, StaticParameterForMessage.ActiveSuccess,
                StaticParameterForMessage.ActiveException);
        }

        public Tuple<bool, string> SuspendWorkTask(TValue value, string suspendedBy, string reason)
        {
            value.SourceStatus = value.Status;
            value.Status = WorkTaskStatus.Suspended;
            value.SuspendedTime = DateTime.Now;
            value.SuspendedBy = suspendedBy;
            value.Comments = reason;
            return UpdateWorkTask(value, StaticParameterForMessage.SuspendSuccess,
                StaticParameterForMessage.SuspendException);
        }

        public Tuple<bool, string> FinishWorkTask(TValue value, string finishBy)
        {
            value.SourceStatus = value.Status;
            value.Status = WorkTaskStatus.Completed;
            value.CompleteTime = DateTime.Now;
            value.UpdateTime = DateTime.Now;
            value.UpdateBy = finishBy;
            return UpdateWorkTask(value, StaticParameterForMessage.FinishSuccess,
                StaticParameterForMessage.FinishException);
        }

        public Tuple<bool, string> TerminateWorkTask(TValue value, string terminatedBy, string reason)
        {
            value.SourceStatus = value.Status;
            value.Status = WorkTaskStatus.Terminated;
            value.TerminatedTime = DateTime.Now;
            value.TerminatedBy = terminatedBy;
            value.Comments = reason;
            return UpdateWorkTask(value, StaticParameterForMessage.TerminateSuccess,
                StaticParameterForMessage.TerminateException);
        }

        public Tuple<bool, string> CancelWorkTask(TValue value, string cancelBy, string reason)
        {
            value.SourceStatus = value.Status;
            value.Status = WorkTaskStatus.Cancelled;
            value.CancelledTime = DateTime.Now;
            value.CancelledBy = cancelBy;
            value.Comments = reason;
            return UpdateWorkTask(value, StaticParameterForMessage.CancelSuccess,
                StaticParameterForMessage.CancelException);
        }

        public Tuple<bool, string> IsExistWorkTaskByObject(string objecctToHandle) => null;
        public TValue GetWorkTaskByObject(string objecctToHandle) => null;
    }
}