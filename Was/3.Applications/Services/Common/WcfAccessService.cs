using System.ServiceModel;

namespace Kengic.Was.Application.Services.Common
{
    public class WcfAccessService
    {
        static WcfAccessService()
        {
            ConnectionString = "net.tcp://localhost:20002/WasService/{0}";
        }

        public static string ConnectionString { get; set; }

        public static T GetService<T>(string service)
        {
            var address = string.Format(ConnectionString, service);
            var endPointAddress = new EndpointAddress(address);
            var netTcpBinding = new NetTcpBinding
            {
                Security = {Mode = SecurityMode.None},
                MaxReceivedMessageSize = int.MaxValue
            };
            var channelFactory = new ChannelFactory<T>(netTcpBinding);
            var client = channelFactory.CreateChannel(endPointAddress);
            if (client == null)
            {
                throw new EndpointNotFoundException();
            }
            return client;
        }
    }
}