using Kengic.Was.CrossCutting.Configuration;
using Prism.Modularity;

namespace Kengic.Was.Presentation.Server.Shell
{
    public class ConfigurationFileStore : IConfigurationStore
    {
        /// <summary>
        ///     Gets the module configuration data.
        /// </summary>
        /// <returns>
        ///     A <see cref="T:Prism.Modularity.ModulesConfigurationSection" /> instance.
        /// </returns>
        public ModulesConfigurationSection RetrieveModuleConfigurationSection()
            => ConfigurationOperation<ModulesConfigurationSection>.GetCustomSection(FilePathExtension.ModulePath,
                "modules");
    }
}