using Kengic.Was.Presentation.Server.Module.Connectors.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Modularity;
using Prism.Regions;

namespace Kengic.Was.Presentation.Server.Module.Connectors
{
    public class ConnectorsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public ConnectorsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize() => _regionManager.Regions["TabRegion"].Add(
            ServiceLocator.Current.GetInstance<ConnectorsView>());
    }
}