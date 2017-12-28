using System;
using Kengic.Was.Connector.Common;
using Kengic.Was.Presentation.Server.Module.Common.ViewModels;

namespace Kengic.Was.Presentation.Server.Module.Connectors.ViewModels
{
    public class Connector : SeviceBase
    {
        private readonly IConnector _connector;

        public Connector(IConnector tConnector)
        {
            _connector = tConnector;
        }

        public override void Start()
        {
            try
            {
                Status = StatusType.StartPending;
                if (!ConnectorsRepository.InitializeConnector(_connector))
                {
                    Status = StatusType.Faulted;
                    return;
                }
                if (!ConnectorsRepository.StartConnector(_connector))
                {
                    Status = StatusType.Faulted;
                    return;
                }
                Status = ConnectorsRepository.GetConnectorInstance(_connector.Id).ConnectStatus
                    ? StatusType.Connected
                    : StatusType.Running;
            }
            catch (Exception)
            {
                Status = StatusType.Faulted;
                throw;
            }
        }

        public override void Stop()
        {
            Status = StatusType.StopPending;
            ConnectorsRepository.CloseConnector(_connector);
            Status = StatusType.Stopped;
        }
    }
}