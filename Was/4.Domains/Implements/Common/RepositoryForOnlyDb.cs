using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Common;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.Domain.Common
{
    /// <summary>
    ///     Repository base class
    /// </summary>
    /// <typeparam name="TEntity">
    ///     The type of underlying entity in this repository
    /// </typeparam>
    /// <typeparam name="TKey"></typeparam>
    public class RepositoryForOnlyDb<TKey, TEntity> : IRepositoryForOnlyDb<TKey, TEntity>
        where TEntity : EntityForTime<TKey>
    {
        private IQueryableUnitOfWork _unitOfWork;

        public RepositoryForOnlyDb(IQueryableUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            _unitOfWork = unitOfWork;

            LogName = GetType().Name;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IUnitOfWork UnitOfWork => _unitOfWork;
        public string LogName { get; set; }

        public virtual Tuple<bool, string> Create(TEntity item)
        {
            if ((item == null) || EqualityComparer<TKey>.Default.Equals(item.Id, default(TKey)))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }
            try
            {
                using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
                {
                    _unitOfWork.AutoDetectChangesEnabled = false;
                    item.CreateTime = DateTime.Now;
                    _unitOfWork.CreateSet<TEntity>().Add(item);
                    _unitOfWork.Commit();
                    _unitOfWork.AutoDetectChangesEnabled = true;
                }

                const string messageCode = StaticParameterForMessage.CreateSuccess;
                return new Tuple<bool, string>(true, messageCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        public virtual Tuple<bool, string> Update(TEntity item)
        {
            if ((item == null) || EqualityComparer<TKey>.Default.Equals(item.Id, default(TKey)))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            try
            {
                item.UpdateTime = DateTime.Now;
                using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
                {
                    _unitOfWork.SetModified(item);
                    _unitOfWork.Commit();
                }

                const string messageCode = StaticParameterForMessage.UpdateSuccess;
                return new Tuple<bool, string>(true, messageCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        public virtual Tuple<bool, string> Remove(TEntity item)
        {
            if ((item == null) || EqualityComparer<TKey>.Default.Equals(item.Id, default(TKey)))
            {
                return new Tuple<bool, string>(false, StaticParameterForMessage.ObjectIsNotExist);
            }

            try
            {
                using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
                {
                    var persisted = _unitOfWork.CreateSet<TEntity>().Find(item.Id);
                    _unitOfWork.Attach(persisted);
                    _unitOfWork.CreateSet<TEntity>().Remove(persisted);
                    _unitOfWork.Commit();
                }
                const string messageCode = StaticParameterForMessage.RemoveSuccess;
                return new Tuple<bool, string>(true, messageCode);
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.ToString());
            }
        }

        public virtual void TrackItem(TEntity item)
        {
            _unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>();
            _unitOfWork.Attach(item);
        }


        public TEntity TryGetValue(TKey key)
        {
            TEntity result;
            if (EqualityComparer<TKey>.Default.Equals(key, default(TKey)))
            {
                return null;
            }
            using (_unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>())
            {
                result = _unitOfWork.CreateSet<TEntity>().Find(key);
            }
            return result;
        }

        public virtual IQueryable<TEntity> GetAll()
        {
            _unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>();
            return _unitOfWork.CreateSet<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetFiltered(Expression<Func<TEntity, bool>> filter)
        {
            _unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>();
            return _unitOfWork.CreateSet<TEntity>().Where(filter);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }
            _unitOfWork?.Dispose();
        }
        protected IDbSet<TEntity> GetSet()
        {
            _unitOfWork = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>();
            return _unitOfWork.CreateSet<TEntity>();
        }
    }
}