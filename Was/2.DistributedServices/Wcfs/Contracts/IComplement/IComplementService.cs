using System;
using System.ServiceModel;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.IComplement
{
    [ServiceContract]
    [StandardFaults]
    public interface IComplementService
    {
        [OperationContract]
        Tuple<bool, string> CreateSourceWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> TerminateSourceWorkTask(string message);

        [OperationContract]
        Tuple<bool, string> CancelSourceWorkTask(string message);
    }
}