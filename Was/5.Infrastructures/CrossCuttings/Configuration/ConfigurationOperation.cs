using System.Configuration;
using System.Diagnostics;
using System.IO;

namespace Kengic.Was.CrossCutting.Configuration
{
    public static class ConfigurationOperation<T> where T : ConfigurationSection, new()
    {
        public static void CreateConfigurationFile(string executeFilePath, string section,
            ConfigurationUserLevel configurationUserLevel = ConfigurationUserLevel.None,
            ConfigurationSaveMode configurationSaveMode = ConfigurationSaveMode.Full, bool forceSave = false)
        {
            try
            {
                var configurationSection = new T();
                var fileMap = new ExeConfigurationFileMap {ExeConfigFilename = executeFilePath};
                var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, configurationUserLevel);

                if (config.Sections[section] == null)
                {
                    config.Sections.Add(section, configurationSection);
                }
                configurationSection.SectionInformation.ForceSave = true;
                config.Save(configurationSaveMode, forceSave);
            }
            catch (ConfigurationErrorsException err)
            {
                Trace.WriteLine($"CreateConfigurationFile: {err}");
            }
        }

        public static T GetCustomSection(string executeFilePath, string section,
            ConfigurationUserLevel configurationUserLevel = ConfigurationUserLevel.None)
        {
            T customSection = null;
            try
            {
                var filePath = Path.Combine(FilePathExtension.ProfileDirectory, executeFilePath);
                var fileMap = new ExeConfigurationFileMap {ExeConfigFilename = filePath};
                var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, configurationUserLevel);
                if (config.HasFile)
                {
                    customSection = config.GetSection(section) as T;
                }
                else
                {
                    throw new FileNotFoundException(filePath);
                }
            }
            catch (ConfigurationErrorsException err)
            {
                Trace.WriteLine($"Using GetSection(string): {err}");
            }
            return customSection;
        }

        public static void SaveConfigurationFile(string executeFilePath,
            ConfigurationUserLevel configurationUserLevel = ConfigurationUserLevel.None,
            ConfigurationSaveMode configurationSaveMode = ConfigurationSaveMode.Modified, bool forceSave = false)
        {
            try
            {
                var fileMap = new ExeConfigurationFileMap {ExeConfigFilename = executeFilePath};
                var config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, configurationUserLevel);
                config.Save(configurationSaveMode, forceSave);
            }
            catch (ConfigurationErrorsException err)
            {
                Trace.WriteLine($"SaveConfigurationFile: {err}");
            }
        }
    }
}