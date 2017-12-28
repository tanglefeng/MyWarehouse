using Kengic.Was.Presentation.Server.Module.Operators.Views;
using Microsoft.Practices.ServiceLocation;
using Prism.Modularity;
using Prism.Regions;

namespace Kengic.Was.Presentation.Server.Module.Operators
{
    public class OperatorsModule : IModule
    {
        private readonly IRegionManager _regionManager;

        public OperatorsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
            => _regionManager.Regions["TabRegion"].Add(ServiceLocator.Current.GetInstance<OperatorsView>());
    }
}