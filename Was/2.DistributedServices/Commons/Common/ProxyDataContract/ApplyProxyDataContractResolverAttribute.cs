using System;
using System.Data.Entity.Core.Objects;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Kengic.Was.DistributedServices.Common.ProxyDataContract
{
    public class ApplyProxyDataContractResolverAttribute : Attribute, IOperationBehavior
    {
        public void AddBindingParameters(OperationDescription description, BindingParameterCollection parameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription description, ClientOperation proxy)
        {
            var
                dataContractSerializerOperationBehavior =
                    description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            dataContractSerializerOperationBehavior.DataContractResolver = new ProxyDataContractResolver();
        }

        public void ApplyDispatchBehavior(OperationDescription description, DispatchOperation dispatch)
        {
            var
                dataContractSerializerOperationBehavior =
                    description.Behaviors.Find<DataContractSerializerOperationBehavior>();
            dataContractSerializerOperationBehavior.DataContractResolver = new ProxyDataContractResolver();
        }

        public void Validate(OperationDescription description)
        {
        }
    }
}