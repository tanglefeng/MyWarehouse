using System.Collections.Concurrent;
using System.Collections.Generic;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;

namespace Kengic.Was.Connector.Common
{
    public interface IConnector
    {
        string Id { get; set; }
        ConcurrentDictionary<string, object> ReceiveDictionary { get; set; }
        bool RecSendMsgStatus { get; set; }
        ConnectorElement ConnectorElement { get; set; }
        bool ConnectStatus { get; set; }
        bool AlarmActiveStatus { get; set; }
        bool InitializeStatus { get; set; }
        bool Initialize();
        bool Connect();
        bool DisConnect();
        bool SendMessage(List<string> messageList);
    }
}