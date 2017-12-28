using System;
using System.Configuration;
using System.IO;

namespace Kengic.Was.CrossCutting.Configuration
{
    public static class FilePathExtension
    {
        public static string ProfileDirectory
            =>
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    ConfigurationManager.AppSettings["ProfileDirectory"] ?? @"Configs\Defaults");

        public static string WcfServicePath
            => ConfigurationManager.AppSettings["WcfServicePath"] ?? @"WcfService.config";

        public static string ConnectorPath => ConfigurationManager.AppSettings["ConnectorPath"] ?? @"Connector.config"
            ;

        public static string OperatorPath => ConfigurationManager.AppSettings["OperatorPath"] ?? @"Operator.config";

        public static string ExceptionHandingPath
            => ConfigurationManager.AppSettings["ExceptionHandingPath"] ?? @"ExceptionHanding.config";

        public static string UnityPath => ConfigurationManager.AppSettings["UnityPath"] ?? @"Unity.config";
        public static string LogPath => ConfigurationManager.AppSettings["LogPath"] ?? @"Log.config";
        public static string MessagePath => ConfigurationManager.AppSettings["MessagePath"] ?? @"Message.config";

        public static string ActivityContractPath
            => ConfigurationManager.AppSettings["ActivityContractPath"] ?? @"ActivityContract.config";

        public static string ModulePath
            => ConfigurationManager.AppSettings["ModulePath"] ?? @"Module.config";

        public static string TypeConfigurationPath
            => ConfigurationManager.AppSettings["TypeConfigurationPath"] ?? @"TypeConfiguration.config";

        public static string ConventionPath
            => ConfigurationManager.AppSettings["ConventionPath"] ?? @"Convention.config";

        public static string TypeFormatterPath
            => ConfigurationManager.AppSettings["TypeFormatterPath"] ?? @"TypeFormatter.config";

        public static string DataInitializationPath
            => ConfigurationManager.AppSettings["DataInitializationPath"] ?? @"DataInitialization.json";
    }
}