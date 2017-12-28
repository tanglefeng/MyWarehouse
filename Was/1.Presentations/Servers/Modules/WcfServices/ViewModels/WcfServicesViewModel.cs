using System;
using System.Collections.ObjectModel;
using System.Linq;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection;
using Kengic.Was.CrossCutting.ConfigurationSection.WcfServices;
using Kengic.Was.Presentation.Server.Module.Common.ViewModels;

namespace Kengic.Was.Presentation.Server.Module.WcfServices.ViewModels
{
    public class WcfServicesViewModel : CommonViewModel
    {
        public WcfServicesViewModel()
        {
            LoadWcfServices();
            AutoStart();
        }

        public ObservableCollection<WcfService> WcfServices { get; set; } = new ObservableCollection<WcfService>();

        public void LoadWcfServices()
        {
            var wcfServiceSection =
                ConfigurationOperation<WcfServiceSection>.GetCustomSection(FilePathExtension.WcfServicePath,
                    "wcfServiceSection");
            if (wcfServiceSection == null)
            {
                throw new Exception("File not found or file format is not correct");
            }
            foreach (
                var tempWcfService in
                    from WcfServiceElement item in wcfServiceSection.WcfServices select new WcfService(item))
            {
                WcfServices.Add(tempWcfService);
            }
        }

        private void AutoStart()
        {
            foreach (var item in WcfServices.Where(r => r.StartupType == StartupType.Automatic))
            {
                item.Start();
            }
        }
    }
}