using System;
using System.Linq;
using System.Reflection;
using Kengic.Was.CrossCutting.LinqToWcf.Communications;
using Kengic.Was.Domain.Common;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.DistributedServices.Common.QueryHandlers
{
    public class ApplicationServiceQueryHandler : IQueryHandler
    {
        private static readonly MethodInfo GetNamedQueryMethod;
        private readonly IQueryableUnitOfWork _queryApplicationService = ServiceLocator.Current.GetInstance<IQueryableUnitOfWork>();

        static ApplicationServiceQueryHandler()
        {
            GetNamedQueryMethod = typeof (ApplicationServiceQueryHandler).GetMethod("Get",
                BindingFlags.Instance | BindingFlags.Public, null, new Type[] {}, null);
        }

        public IQueryable Get(Type type)
        {
            var genericGetTableMethod = GetNamedQueryMethod.MakeGenericMethod(type);
            return (IQueryable) genericGetTableMethod.Invoke(this, new object[] {});
        }

        public IQueryable<T> Get<T>() where T : class => _queryApplicationService.CreateSet<T>();
    }
}