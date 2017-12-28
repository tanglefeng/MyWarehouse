using System;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Application.WasModel.Dto;
using Kengic.Was.Domain.Entity.Common;
using Newtonsoft.Json;

namespace Kengic.Was.DistributedServices.Common
{
    public class DynamicQueryDataHelper
    {
        public static string GetData<TKey, TEntity>(IQueryRepository<TKey, TEntity> repository,
            IEnumerable<DynamicQueryDto> dynamicQueryMethods) where TEntity : EntityBase<TKey>
        {
            var dataset = repository.GetAll();
            var queryResult = ExecuteDynamicMehtods(dataset, dynamicQueryMethods);
            var queryResultList = Enumerable.ToList(queryResult);
            var dataResult = JsonConvert.SerializeObject(queryResultList);
            return dataResult;
        }

        public static dynamic ExecuteDynamicMehtods<T>(IQueryable<T> dataset,
            IEnumerable<DynamicQueryDto> dynamicQueryMethods) where T : class
        {
            dynamic result = dataset;
            if (dynamicQueryMethods == null)
            {
                return result;
            }
            foreach (var query in dynamicQueryMethods)
            {
                var parameters = new List<object> {result};
                var methodParameters = query.Parameters;
                parameters.AddRange(methodParameters);

                var type = Type.GetType(query.TypeName);
                if (type == null)
                {
                    continue;
                }
                var method =
                    type.GetMethods().SingleOrDefault(r => r.ToString() == query.Method);
                if (method == null)
                {
                    throw new InvalidOperationException();
                }
                if (method.IsGenericMethod)
                {
                    if (query.MethodTypeName != null)
                    {
                        var methodType = Type.GetType(query.MethodTypeName);
                        method = method.MakeGenericMethod(methodType);
                    }
                    else
                    {
                        method = method.MakeGenericMethod(typeof (T));
                    }
                }
                result = method.Invoke(null, parameters.ToArray());
            }
            return result;
        }
    }
}