using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Connectors;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.Connector.Common
{
    public class ConnectorsRepository
    {
        private const string ConnectorSection = "connectorSection";
        private const string ConnectorsRepositoryName = "ConnectorsRepository";

        private static readonly ConcurrentDictionary<string, IConnector> ConnectorDictionary =
            new ConcurrentDictionary<string, IConnector>();

        public static void LoadConnectorConfiguration(string fileName)
        {
            try
            {
                var configurations = ConfigurationOperation<ConnectorSection>.GetCustomSection(fileName,
                    ConnectorSection);
                if (configurations == null)
                {
                    LogRepository.WriteErrorLog(ConnectorsRepositoryName,
                        StaticParameterForMessage.NoSection, fileName);
                    return;
                }

                foreach (ConnectorElement connectorElement in configurations.Connectors)
                {
                    var connectionInstance = ServiceLocator.Current.GetInstance<IConnector>(connectorElement.Name);
                    connectionInstance.Id = connectorElement.Id;
                    connectionInstance.ConnectorElement = connectorElement;
                    ConnectorDictionary.TryAdd(connectorElement.Id, connectionInstance);

                    LogRepository.WriteInfomationLog(ConnectorsRepositoryName,
                        StaticParameterForMessage.LoadConnectSuccess, connectorElement.Id);
                }
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(ConnectorsRepositoryName,
                    StaticParameterForMessage.LoadConnectException, ex.ToString());
            }
        }

        private static List<IConnector> GetConnectorsList() => ConnectorDictionary.Values.ToList();

        private static bool ConnectorExecute(
            Func<IConnector, bool> executeMethod)
        {
            var connectorList = GetConnectorsList();
            if ((connectorList == null) || (connectorList.Count <= 0))
            {
                return false;
            }

            foreach (var connector in connectorList)
            {
                executeMethod(connector);
            }

            return true;
        }

        public static bool InitializeConnector() => ConnectorExecute(InitializeConnector);

        public static bool InitializeConnector(IConnector tConnector)
        {
            try
            {
                var connectorInstance = GetConnectorInstance(tConnector.Id);
                if (connectorInstance == null)
                {
                    LogRepository.WriteErrorLog(ConnectorsRepositoryName,
                        StaticParameterForMessage.CreateInstanceFailure, tConnector.Id);

                    return false;
                }

                if (connectorInstance.Initialize())
                {
                    LogRepository.WriteInfomationLog(ConnectorsRepositoryName,
                        StaticParameterForMessage.InitializeConnectorSuccess, tConnector.Id);

                    return true;
                }
                LogRepository.WriteErrorLog(ConnectorsRepositoryName,
                    StaticParameterForMessage.InitializeConnectorFailure, tConnector.Id);

                return false;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(ConnectorsRepositoryName,
                    StaticParameterForMessage.InitializeConnectorException, ex.ToString());

                return false;
            }
        }

        public static bool StartConnector() => ConnectorExecute(StartConnector);

        public static bool StartConnector(IConnector tConnector)
        {
            try
            {
                var connectInstance = GetConnectorInstance(tConnector.Id);
                if (connectInstance == null)
                {
                    return false;
                }

                if (connectInstance.Connect())
                {
                    LogRepository.WriteInfomationLog(ConnectorsRepositoryName,
                        StaticParameterForMessage.StartConnectorSuccess, tConnector.Id);

                    return true;
                }
                LogRepository.WriteErrorLog(ConnectorsRepositoryName,
                    StaticParameterForMessage.StartConnectorFailure, tConnector.Id);
                return false;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(ConnectorsRepositoryName,
                    StaticParameterForMessage.StartConnectorException, ex.ToString());
                return false;
            }
        }

        public static bool CloseConnector(IConnector tConnector)
        {
            try
            {
                var connectInstance = GetConnectorInstance(tConnector.Id);
                if (connectInstance == null)
                {
                    return false;
                }

                if (connectInstance.DisConnect())
                {
                    connectInstance.RecSendMsgStatus = false;

                    LogRepository.WriteInfomationLog(ConnectorsRepositoryName,
                        StaticParameterForMessage.CloseConnectorSuccess, tConnector.Id);
                    return true;
                }

                LogRepository.WriteErrorLog(ConnectorsRepositoryName,
                    StaticParameterForMessage.CloseConnectorFailure, tConnector.Id);

                return false;
            }
            catch (Exception ex)
            {
                LogRepository.WriteExceptionLog(ConnectorsRepositoryName,
                    StaticParameterForMessage.CloseConnectorException, ex.ToString());
                return false;
            }
        }

        public static bool CloseConnector() => ConnectorExecute(CloseConnector);
        private static bool IsExistConnector(string connectorId) => ConnectorDictionary.ContainsKey(connectorId);

        public static IConnector GetConnectorInstance(string connectorId)
        {
            if (!IsExistConnector(connectorId))
            {
                LogRepository.WriteExceptionLog(ConnectorsRepositoryName,
                    StaticParameterForMessage.ConnectorIsNotExist, connectorId);
                return null;
            }

            IConnector tConnector;
            if (ConnectorDictionary.TryGetValue(connectorId, out tConnector))
            {
                return tConnector;
            }

            LogRepository.WriteExceptionLog(ConnectorsRepositoryName,
                StaticParameterForMessage.ConnectorInstanceIsNotExist, connectorId);
            return null;
        }
    }
}