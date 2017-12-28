using Kengic.Was.Presentation.Server.Module.WcfServices.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Modularity;
using Prism.Regions;

namespace Kengic.Was.Presentation.Server.Module.WcfServices
{
    public class WcfServicesModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public WcfServicesModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
            => _regionManager.Regions["TabRegion"].Add(ServiceLocator.Current.GetInstance<WcfServicesView>());
    }
}