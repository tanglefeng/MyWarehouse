using System;
using System.Collections.Concurrent;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.ActivityContracts;
using Kengic.Was.CrossCutting.ConfigurationSection.FileConfigs;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;

namespace Kengic.Was.Systems.ActivityContracts
{
    public class ActivityContractRepository
    {
        private const string ActivityContractRepositoryName = "ActivityContractRepository";
        private const string ActivityContractSection = "fileConfigSection";

        private static readonly
            ConcurrentDictionary<string, ActivityContractElement>
            ActivityContractQueue = new ConcurrentDictionary<string, ActivityContractElement>();

        public static void LoadActivityContractConfiguration(string fileName)
        {
            var configurations = ConfigurationOperation<FileConfigSection>.GetCustomSection(fileName,
                ActivityContractSection);

            if (configurations == null)
            {
                LogRepository.WriteErrorLog(ActivityContractRepositoryName,
                    StaticParameterForMessage.NoSection, ActivityContractSection);
                return;
            }

            foreach (FileConfigElement configFileElement in configurations.FileConfigs)
            {
                var activityContractProperties =
                    ConfigurationOperation<ActivityContractSection>.GetCustomSection(
                        configFileElement.FilePath,
                        configFileElement.SectionName);
                if (activityContractProperties == null)
                {
                    throw new Exception(
                        $"File {configFileElement.FilePath} not found or file format {configFileElement.SectionName} is not correct");
                }
                foreach (
                    ActivityContractElement activityContractProperty in
                        activityContractProperties.ActivityContracts)
                {
                    ActivityContractQueue.TryAdd(activityContractProperty.Id, activityContractProperty);
                }
            }
        }

        public static bool IsExistActivityContract(string activityContractId)
            => ActivityContractQueue.ContainsKey(activityContractId);

        public static ActivityContractElement GetActivityContract(
            string activityContractId)
        {
            if (!IsExistActivityContract(activityContractId))
            {
                return null;
            }

            ActivityContractElement activityContractElement;
            return ActivityContractQueue.TryGetValue(activityContractId, out activityContractElement)
                ? activityContractElement
                : null;
        }
    }
}