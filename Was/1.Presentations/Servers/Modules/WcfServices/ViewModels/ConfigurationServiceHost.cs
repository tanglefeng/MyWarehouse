using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Kengic.Was.CrossCutting.Configuration;

namespace Kengic.Was.Presentation.Server.Module.WcfServices.ViewModels
{
    public class ConfigurationServiceHost : ServiceHost
    {
        private readonly string _configurationFileName;

        public ConfigurationServiceHost(Type serviceType, string configurationFileName, params Uri[] baseAddresses)
        {
            _configurationFileName = Path.Combine(FilePathExtension.ProfileDirectory, configurationFileName);
            var collection = new UriSchemeKeyedCollection(baseAddresses);
            InitializeDescription(serviceType, collection);
        }

        protected override void ApplyConfiguration()
        {
            if (string.IsNullOrEmpty(_configurationFileName) || !File.Exists(_configurationFileName))
            {
                throw new Exception($"Can not find configuration file{_configurationFileName}");
            }
            LoadConfigFromCustomLocation(_configurationFileName);
        }

        private void LoadConfigFromCustomLocation(string configFilename)
        {
            var filemap = new ExeConfigurationFileMap {ExeConfigFilename = configFilename};

            var config =
                ConfigurationManager.OpenMappedExeConfiguration
                    (filemap,
                        ConfigurationUserLevel.None);

            var serviceModel = ServiceModelSectionGroup.GetSectionGroup(config);

            if (serviceModel != null)
            {
                var serviceElement =
                    serviceModel.Services.Services.OfType<ServiceElement>().FirstOrDefault();

                if (serviceElement != null)
                {
                    LoadConfigurationSection(serviceElement);
                }
                else
                {
                    throw new ArgumentException("ServiceElement doesn't exist");
                }
            }
        }
    }
}