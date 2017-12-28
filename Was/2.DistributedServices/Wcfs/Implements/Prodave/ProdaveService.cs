using Kengic.Was.Connector.Common;
using Kengic.Was.Connector.Prodave;
using Kengic.Was.Wcf.IProdave;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF;

namespace Kengic.Was.Wcf.Prodave
{
    [ExceptionShielding("WcfServicePolicy")]
    public class ProdaveService : IProdaveService
    {
        public int ReadInt16(string connectionName, ushort datablock, int startAddress)
        {
            var connector = GetConnection(connectionName);
            return ((ProdaveClient) connector)?.ReadInt16(datablock, startAddress) ?? 0;
        }

        public bool WriteInt16(string connectionName, ushort datablock, int startAddress, ushort value)
        {
            var connector = GetConnection(connectionName);
            return (connector != null) && ((ProdaveClient) connector).WriteInt16(datablock, startAddress, value);
        }

        public string ReadString(string connectionName, ushort datablock, int startAddress, int length)
        {
            var connector = GetConnection(connectionName);
            return ((ProdaveClient) connector)?.Readstring(datablock, startAddress, length);
        }

        public bool WriteString(string connectionName, ushort datablock, int startAddress, string value)
        {
            var connector = GetConnection(connectionName);
            return (connector != null) && ((ProdaveClient) connector).WriteString(datablock, startAddress, value);
        }

        public string ReadGroup(string connectionName, string group)
        {
            var connector = GetConnection(connectionName);
            return ((ProdaveClient) connector)?.ReadGroup(connectionName, @group);
        }

        public bool WriteGroup(string connectionName, string group, string value)
        {
            var connector = GetConnection(connectionName);
            return (connector != null) && ((ProdaveClient) connector).WriteGroup(connectionName, @group, value);
        }

        public IConnector GetConnection(string connectionName)
            => ConnectorsRepository.GetConnectorInstance(connectionName);
    }
}