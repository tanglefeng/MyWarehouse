using System;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Common
{
    public class RepositoryForSyncDb<TKey, TEntity> : IRepositoryForSyncDb<TKey, TEntity>
        where TEntity : EntityForTime<TKey>, new()
    {
        protected ConcurrentQueueSyncDb<TKey, TEntity> ValueQueue;

        public RepositoryForSyncDb(IQueryableUnitOfWork queryableUnitOfWork)
        {
            ValueQueue = new ConcurrentQueueSyncDb<TKey, TEntity>(queryableUnitOfWork);
            Load();
            LogName = GetType().Name;
        }

        public string LogName { get; set; }
        public void Load() => ValueQueue.Load();
        public IEnumerable<TEntity> GetValuesFromMemory() => ValueQueue.Values;

        public Tuple<bool, string> Create(TEntity item)
        {
            string messageCode;
            if ((item == null) || EqualityComparer<TKey>.Default.Equals(item.Id, default(TKey)))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            try
            {
                item.CreateTime = DateTime.Now;
                var returnValue = ValueQueue.TryAdd(item);

                if (returnValue)
                {
                    messageCode = StaticParameterForMessage.CreateSuccess;
                    LogRepository.WriteInfomationLog(LogName, messageCode, item.ToShortString());
                    return new Tuple<bool, string>(true, messageCode);
                }

                messageCode = StaticParameterForMessage.CreateFailure;
                LogRepository.WriteErrorLog(LogName, messageCode, item.ToShortString());
                return new Tuple<bool, string>(false, messageCode);
            }
            catch (Exception ex)
            {
                messageCode = StaticParameterForMessage.CreateException;
                LogRepository.WriteExceptionLog(LogName, messageCode, item.ToShortString() + ":" + ex);
                return new Tuple<bool, string>(false, messageCode);
            }
        }

        public Tuple<bool, string> Update(TEntity item)
        {
            string messageCode;
            if ((item == null) || EqualityComparer<TKey>.Default.Equals(item.Id, default(TKey)))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            try
            {
                item.UpdateTime = DateTime.Now;
                var returnValue = ValueQueue.TryUpdate(item);
                if (returnValue)
                {
                    messageCode = StaticParameterForMessage.UpdateSuccess;
                    LogRepository.WriteInfomationLog(LogName, messageCode, item.ToShortString());
                    return new Tuple<bool, string>(true, messageCode);
                }

                messageCode = StaticParameterForMessage.UpdateFailure;
                LogRepository.WriteErrorLog(LogName, messageCode, item.ToShortString());
                return new Tuple<bool, string>(false, messageCode);
            }
            catch (Exception ex)
            {
                messageCode = StaticParameterForMessage.UpdateException;
                LogRepository.WriteExceptionLog(LogName, messageCode, item.ToShortString() + ":" + ex);
                return new Tuple<bool, string>(false, messageCode);
            }
        }

        public Tuple<bool, string> Remove(TEntity item)
        {
            string messageCode;
            if ((item == null) || EqualityComparer<TKey>.Default.Equals(item.Id, default(TKey)))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }
            try
            {
                var returnValue = ValueQueue.TryRemove(item.Id);

                if (returnValue)
                {
                    messageCode = StaticParameterForMessage.RemoveSuccess;
                    LogRepository.WriteInfomationLog(LogName, messageCode, item.ToShortString());
                    return new Tuple<bool, string>(true, messageCode);
                }
                messageCode = StaticParameterForMessage.RemoveFailure;
                LogRepository.WriteErrorLog(LogName, messageCode, item.ToShortString());
                return new Tuple<bool, string>(false, messageCode);
            }
            catch (Exception ex)
            {
                messageCode = StaticParameterForMessage.RemoveException;
                LogRepository.WriteExceptionLog(LogName, messageCode, item.ToShortString() + ":" + ex);
                return new Tuple<bool, string>(false, messageCode);
            }
        }

        public TEntity TryGetValue(TKey tKey) => ValueQueue.TryGetValue(tKey);
        public IQueryable<TEntity> GetAll() => ValueQueue.DbSet;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            //if (!disposing)
            //{
            //    return;
            //}
            //if (ValueQueue == null)
            //{
            //    return;
            //}
            //ValueQueue.Dispose();
            //ValueQueue = null;
        }
    }
}