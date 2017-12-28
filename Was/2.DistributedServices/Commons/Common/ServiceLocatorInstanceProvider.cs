using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using Microsoft.Practices.ServiceLocation;

namespace Kengic.Was.DistributedServices.Common
{
    /// <summary>
    ///     The unity instance provider. This class provides an extensibility point
    ///     for creating instances of wcf service.
    ///     <remarks>
    ///         The goal is to inject dependencies from the inception point
    ///     </remarks>
    /// </summary>
    public class ServiceLocatorInstanceProvider : IInstanceProvider
    {
        private readonly Type _serviceType;

        /// <summary>
        ///     Create a new instance of unity instance provider
        /// </summary>
        /// <param name="serviceType">
        ///     The service where we apply the instance provider
        /// </param>
        public ServiceLocatorInstanceProvider(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            _serviceType = serviceType;
        }

        /// <summary>
        ///     <see cref="IInstanceProvider" />
        /// </summary>
        /// <param name="instanceContext">
        ///     <see cref="IInstanceProvider" />
        /// </param>
        /// <param name="message">
        ///     <see cref="IInstanceProvider" />
        /// </param>
        /// <returns>
        ///     <see cref="IInstanceProvider" />
        /// </returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
            => ServiceLocator.Current.GetInstance(_serviceType);

        /// <summary>
        ///     <see cref="IInstanceProvider" />
        /// </summary>
        /// <param name="instanceContext">
        ///     <see cref="IInstanceProvider" />
        /// </param>
        /// <returns>
        ///     <see cref="IInstanceProvider" />
        /// </returns>
        public object GetInstance(InstanceContext instanceContext) => ServiceLocator.Current.GetInstance(_serviceType);

        /// <summary>
        ///     <see cref="IInstanceProvider" />
        /// </summary>
        /// <param name="instanceContext">
        ///     <see cref="IInstanceProvider" />
        /// </param>
        /// <param name="instance">
        ///     <see cref="System.ServiceModel.Dispatcher.InstanceProvider" />
        /// </param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            var disposable = instance as IDisposable;
            disposable?.Dispose();
        }
    }
}