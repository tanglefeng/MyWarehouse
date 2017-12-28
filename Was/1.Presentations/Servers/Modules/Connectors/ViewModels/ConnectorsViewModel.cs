using System;
using System.Collections.ObjectModel;
using System.Linq;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.Presentation.Server.Module.Common.ViewModels;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.Presentation.Server.Module.Connectors.ViewModels
{
    public class ConnectorsViewModel : CommonViewModel
    {
        public ConnectorsViewModel()
        {
            LoadConnectors();
            AutoStart();
        }

        public ObservableCollection<Connector> Connectors { get; set; } = new ObservableCollection<Connector>();

        public void LoadConnectors()
        {
            ConnectorsRepository.LoadConnectorConfiguration(FilePathExtension.ConnectorPath);
            var connectorSection =
                ConfigurationOperation<ConnectorSection>.GetCustomSection(FilePathExtension.ConnectorPath,
                    "connectorSection");
            if (connectorSection == null)
            {
                throw new Exception("File not found or file format is not correct");
            }
            foreach (ConnectorElement connectorElement in connectorSection.Connectors)
            {
                var iConnector = ServiceLocator.Current.GetInstance<IConnector>(connectorElement.Name);
                iConnector.ConnectorElement = connectorElement;
                iConnector.Id = connectorElement.Id;
                var theConnector = new Connector(iConnector)
                {
                    Id = connectorElement.Id,
                    Name = connectorElement.Name,
                    Description = connectorElement.Description,
                    StartupType = connectorElement.StartupType,
                    Status = StatusType.Empty
                };
                Connectors.Add(theConnector);
            }
        }

        private void AutoStart()
        {
            foreach (var item in Connectors.Where(r => r.StartupType == StartupType.Automatic))
            {
                item.Start();
            }
        }
    }
}