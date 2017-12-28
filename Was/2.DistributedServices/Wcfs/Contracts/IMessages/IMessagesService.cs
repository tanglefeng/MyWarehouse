using System.ServiceModel;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.IMessages
{
    [ServiceContract]
    [StandardFaults]
    public interface IMessagesService
    {
        [OperationContract]
        string GetValueByLanguage(string language, string messageCode);

        [OperationContract]
        string GetValue(string messageCode);
    }
}