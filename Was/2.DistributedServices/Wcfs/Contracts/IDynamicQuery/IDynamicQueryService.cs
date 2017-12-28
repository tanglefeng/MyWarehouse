using System.ServiceModel;
using Kengic.Was.CrossCutting.LinqToWcf.Communications;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;
using Kengic.Was.DistributedServices.Common.ServiceKnowTypes;

namespace Kengic.Was.Wcf.IDynamicQuery
{
    [ServiceContract]
    [StandardFaults]
    [ServiceKnownType("GetKnownTypes", typeof (KnownTypesExtensions))]
    public interface IDynamicQueryService : IQueryRemoteHandler
    {
    }
}