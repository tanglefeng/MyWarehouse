using System;
using System.ComponentModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Kengic.Was.DistributedServices.Common.ExceptionHandings
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class StandardFaultsAttribute : Attribute, IContractBehavior
    {
        private static readonly Type[] Faults =
        {
            typeof (WcfServiceFault)
        };

        public void AddBindingParameters(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            ClientRuntime clientRuntime)
        {
        }

        public void ApplyDispatchBehavior(
            ContractDescription contractDescription,
            ServiceEndpoint endpoint,
            DispatchRuntime dispatchRuntime)
        {
        }

        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
            foreach (var op in contractDescription.Operations)
            {
                foreach (var fault in Faults)
                {
                    op.Faults.Add(MakeFault(fault));
                }
            }
        }

        private static FaultDescription MakeFault(Type detailType)
        {
            var action = detailType.Name;
            var description = (DescriptionAttribute)
                GetCustomAttribute(detailType, typeof (DescriptionAttribute));
            if (description != null)
                action = description.Description;
            var fd = new FaultDescription(action)
            {
                DetailType = detailType,
                Name = detailType.Name
            };
            return fd;
        }
    }
}