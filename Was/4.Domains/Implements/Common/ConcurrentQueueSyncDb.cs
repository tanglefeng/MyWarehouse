using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Kengic.Was.Domain.Entity.Common;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.Domain.Common
{
    public class ConcurrentQueueSyncDb<TKey, TValue> : IDisposable where TValue : Entity<TKey>, new()
    {
        private readonly Expression<Func<TValue, bool>> _filterExpression;
        private readonly Func<TValue, bool> _filterFunc;
        private Lazy<ConcurrentDictionary<TKey, TValue>> _cDictionary;
        private IQueryableUnitOfWork _unitOfWork;

        public ConcurrentQueueSyncDb(IQueryableUnitOfWork unitOfWork, Expression<Func<TValue, bool>> filter = null)
        {
            _unitOfWork = unitOfWork;
            _filterExpression = filter;
            if (_filterExpression != null)
            {
                _filterFunc = _filterExpression.Compile();
            }
            Load();
        }

        public ICollection<TKey> Keys => _cDictionary.Value.Keys;
        public ICollection<TValue> Values => _cDictionary.Value.Values;

        public IDbSet<TValue> DbSet
        {
            get
            {
                _unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>();
                return _unitOfWork.CreateSet<TValue>();
            }
        }

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
            //if (_unitOfWork == null)
            //{
            //    return;
            //}
            //_unitOfWork.Dispose();
            //_unitOfWork = null;
        }

        public void Load() => _cDictionary = new Lazy<ConcurrentDictionary<TKey, TValue>>(GetDataDictionary);

        private ConcurrentDictionary<TKey, TValue> GetDataDictionary()
        {
            using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
            {
                var entitySet = _unitOfWork.CreateSet<TValue>();
                var dbDictionary = _filterExpression == null
                    ? entitySet.ToDictionary(r => r.Id)
                    : entitySet.Where(_filterExpression).ToDictionary(r => r.Id);
                return new ConcurrentDictionary<TKey, TValue>(dbDictionary);
            }
        }

        /// <summary>
        ///     以实体的Id为Key 操作成功失败只以数据库操作为准,内存操作失败则重新加载数据库到内存,以保持一致
        /// </summary>
        /// <param name="value"></param>
        /// <returns>
        /// </returns>
        public bool TryAdd(TValue value)
        {
            TValue newValue;
            using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
            {
                _unitOfWork.AutoDetectChangesEnabled = false;
                newValue = _unitOfWork.CreateSet<TValue>().Add(value);
                _unitOfWork.Commit();
                _unitOfWork.AutoDetectChangesEnabled = true;
            }
            try
            {
                var result = _cDictionary.Value.TryAdd(newValue.Id, newValue);
                if (!result)
                {
                    Load();
                }
            }
            catch
            {
                Load();
            }
            return true;
        }

        /// <summary>
        ///     先去内存查找,查不到再去数据库查
        /// </summary>
        /// <param name="key"></param>
        /// <returns>
        /// </returns>
        public TValue TryGetValue(TKey key)
        {
            TValue result;
            if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
            {
                return null;
            }
            if (_cDictionary.Value.TryGetValue(key, out result))
            {
                return result;
            }
            using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
            {
                result = _unitOfWork.CreateSet<TValue>().Find(key);
            }
            if (result != null)
            {
                Load();
            }
            return result;
        }

        public bool TryRemove(TKey key)
        {
            using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
            {
                var value = _unitOfWork.CreateSet<TValue>().Find(key);
                _unitOfWork.CreateSet<TValue>().Remove(value);
                _unitOfWork.Commit();
            }

            try
            {
                TValue outValue;
                return _cDictionary.Value.TryRemove(key, out outValue);
            }
            catch
            {
                Load();
            }
            return true;
        }

        public bool TryUpdate(TValue newValue, List<string> properties = null)
        {
            var toFilterValues = true;
            if (_filterFunc != null)
            {
                toFilterValues = _filterFunc(newValue);
            }

            using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
            {
                if (properties == null)
                {
                    _unitOfWork.SetModified(newValue);
                }
                else
                {
                    _unitOfWork.SetPropertiesModified(newValue, properties);
                }

                _unitOfWork.Commit();
                if (!toFilterValues)
                {
                    _unitOfWork.SetDetached(newValue);
                }
            }

            try
            {
                TValue persisted;
                _cDictionary.Value.TryGetValue(newValue.Id, out persisted);
                if (persisted != null)
                {
                    var result = _cDictionary.Value.TryUpdate(newValue.Id, newValue, persisted);
                    if (result)
                    {
                        if (!toFilterValues)
                        {
                            TValue value;
                            _cDictionary.Value.TryRemove(newValue.Id, out value);
                        }
                    }
                }
            }
            catch
            {
                Load();
            }
            return true;
        }

        public bool IsExist(TKey key)
            => !EqualityComparer<TKey>.Default.Equals(key, default(TKey)) && _cDictionary.Value.ContainsKey(key);
    }
}