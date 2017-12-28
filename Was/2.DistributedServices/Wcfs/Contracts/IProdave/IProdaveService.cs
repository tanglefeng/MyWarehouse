using System.ServiceModel;
using Kengic.Was.DistributedServices.Common.ExceptionHandings;

namespace Kengic.Was.Wcf.IProdave
{
    [ServiceContract]
    [StandardFaults]
    public interface IProdaveService
    {
        [OperationContract]
        int ReadInt16(string connectionName, ushort datablock, int startAddress);

        [OperationContract]
        bool WriteInt16(string connectionName, ushort datablock, int startAddress, ushort value);

        [OperationContract]
        string ReadString(string connectionName, ushort datablock, int startAddress, int length);

        [OperationContract]
        bool WriteString(string connectionName, ushort datablock, int startAddress, string value);

        [OperationContract]
        string ReadGroup(string connectionName, string group);

        [OperationContract]
        bool WriteGroup(string connectionName, string group, string value);
    }
}