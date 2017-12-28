using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Kengic.Was.DistributedServices.Common
{
    /// <summary>
    ///     Service behavior for adding Unity instance provider in each endpoint
    ///     dispatcher
    /// </summary>
    public class ServiceLocatorInstanceProviderServiceBehavior : Attribute, IServiceBehavior
    {
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (var dispatcher in serviceHostBase.ChannelDispatchers.Select(item => item as ChannelDispatcher))
            {
                dispatcher?.Endpoints.ToList()
                    .ForEach(
                        endpoint => endpoint.DispatchRuntime.InstanceProvider =
                            new ServiceLocatorInstanceProvider(serviceDescription.ServiceType));
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }
    }

    public class ServiceLocatorInstanceProviderServiceBehaviorElement : BehaviorExtensionElement
    {
        public override Type BehaviorType => typeof (ServiceLocatorInstanceProviderServiceBehavior);
        protected override object CreateBehavior() => new ServiceLocatorInstanceProviderServiceBehavior();
    }
}