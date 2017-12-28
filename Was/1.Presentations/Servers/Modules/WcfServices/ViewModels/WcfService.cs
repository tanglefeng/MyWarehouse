using System;
using System.ServiceModel;
using Kengic.Was.CrossCutting.ConfigurationSection.WcfServices;
using Kengic.Was.Presentation.Server.Module.Common.ViewModels;

namespace Kengic.Was.Presentation.Server.Module.WcfServices.ViewModels
{
    public class WcfService : SeviceBase, IDisposable
    {
        private readonly WcfServiceElement _wcfServiceElement;
        private ServiceHostBase _serviceHostBase;
        private StatusType _statusType;

        public WcfService(WcfServiceElement wcfServiceElement)
        {
            _wcfServiceElement = wcfServiceElement;
            Id = wcfServiceElement.Id;
            Name = wcfServiceElement.Name;
            Description = wcfServiceElement.Description;
            StartupType = wcfServiceElement.StartupType;
        }

        public override StatusType Status
        {
            get
            {
                if (_serviceHostBase == null)
                {
                    return StatusType.Empty;
                }
                switch (_serviceHostBase.State)
                {
                    case CommunicationState.Created:
                        _statusType = StatusType.Empty;
                        break;
                    case CommunicationState.Opening:
                        _statusType = StatusType.StartPending;
                        break;
                    case CommunicationState.Opened:
                        _statusType = StatusType.Running;
                        break;
                    case CommunicationState.Closing:
                        _statusType = StatusType.StopPending;
                        break;
                    case CommunicationState.Closed:
                        _statusType = StatusType.Stopped;
                        break;
                    case CommunicationState.Faulted:
                        _statusType = StatusType.Faulted;
                        break;
                    default:
                        return StatusType.Empty;
                }
                return _statusType;
            }
            set { }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_serviceHostBase != null)
                {
                    _serviceHostBase.Close();
                    _serviceHostBase = null;
                }
            }
        }

        private ServiceHostBase NewServiceHostBase()
        {
            var configurationSeviceHost = new ConfigurationServiceHost(_wcfServiceElement.ServiceType,
                _wcfServiceElement.FilePath);
            configurationSeviceHost.Closing += (sender, args) => OnPropertyChanged("Status");
            configurationSeviceHost.Closed += (sender, args) => OnPropertyChanged("Status");
            configurationSeviceHost.Opening += (sender, args) => OnPropertyChanged("Status");
            configurationSeviceHost.Opened += (sender, args) => OnPropertyChanged("Status");
            configurationSeviceHost.Faulted += (sender, args) => OnPropertyChanged("Status");
            return configurationSeviceHost;
        }

        public override void Start()
        {
            if ((_serviceHostBase == null) || (_serviceHostBase.State != CommunicationState.Opened))
            {
                _serviceHostBase = NewServiceHostBase();
                _serviceHostBase.Open();
            }
        }

        public override void Stop()
        {
            if ((_serviceHostBase != null) && (_serviceHostBase.State == CommunicationState.Opened))
            {
                _serviceHostBase.Close();
            }
        }
    }
}