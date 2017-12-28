using System;
using System.Linq;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.FileConfigs;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Kengic.Was.CrossCutting.Unity
{
    public static class UnityBootstrapper
    {
        public static void Run()
        {
            var configurations =
                ConfigurationOperation<FileConfigSection>.GetCustomSection(FilePathExtension.UnityPath,
                    "fileConfigSection");
            if (configurations == null)
            {
                throw new NullReferenceException("FileConfigSection");
            }
            var container = new UnityContainer();
            var locator = new UnityServiceLocator(container);
            ServiceLocator.SetLocatorProvider(() => locator);
            foreach (
                var unityConfigurationSection in from FileConfigElement configFileElement in configurations.FileConfigs
                    select
                        ConfigurationOperation<UnityConfigurationSection>.GetCustomSection(configFileElement.FilePath,
                            configFileElement.SectionName))
            {
                if (unityConfigurationSection == null)
                {
                    throw new NullReferenceException("UnityConfigurationSection");
                }
                container.LoadConfiguration(unityConfigurationSection);
            }
        }
    }
}