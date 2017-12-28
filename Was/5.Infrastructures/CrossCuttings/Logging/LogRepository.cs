using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using Kengic.Was.CrossCutting.Common;
using Kengic.Was.CrossCutting.Configuration;
using Kengic.Was.CrossCutting.ConfigurationSection.Logs;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Sinks;

namespace Kengic.Was.CrossCutting.Logging
{
    public class LogRepository
    {
        private const string LogSection = "logSection";
        private const string SimpleTextFormatterKey = "SimpleTextFormatter";

        private static readonly ConcurrentDictionary<string, LogElement> LogConfigQueue =
            new ConcurrentDictionary<string, LogElement>();

        private static readonly Dictionary<string, IEventTextFormatter> LogFormatterDict =
            new Dictionary<string, IEventTextFormatter>();

        private static void LoadLogFomatter()
        {
            var simpleTextFormatter = new SimpleTextFormatter("\r\n", "\r\n", DateTimeHelper.Windowffffff);
            if (!LogFormatterDict.ContainsKey(SimpleTextFormatterKey))
            {
                LogFormatterDict.Add(SimpleTextFormatterKey, simpleTextFormatter);
            }
        }

        public static bool LoadLogConfiguration(string filePath)
        {
            if (filePath == null)
            {
                TraceLogHelper.WriteTraceLog("No log configuration");
                return false;
            }

            try
            {
                LoadLogFomatter();
                var file = Path.Combine(FilePathExtension.ProfileDirectory, filePath);
                var configurations = ConfigurationOperation<LogSection>.GetCustomSection(file, LogSection);
                if (configurations == null)
                {
                    TraceLogHelper.WriteTraceLog($"No Section,[{LogSection}]");
                    return false;
                }

                foreach (LogElement tLogconfig in configurations.Logs)
                {
                    if (LogConfigQueue.ContainsKey(tLogconfig.Id))
                    {
                        continue;
                    }
                    var observableEventListener = new ObservableEventListener();
                    var jsonEventTextFormatter = LogFormatterDict[tLogconfig.TextFormatter];
                    var logMaxSize = FileHelper.ConvertFileSize(tLogconfig.MaxSize, tLogconfig.MaxSizeUnit);

                    var logconfig = tLogconfig;
                    RollFileExistsBehavior rollFileExistsBehavior;
                    var rollFileExistsBehaviorParseResult = Enum.TryParse(tLogconfig.FileExistsBehavior.ToString(),
                        out rollFileExistsBehavior);
                    if (!rollFileExistsBehaviorParseResult)
                    {
                        rollFileExistsBehavior = RollFileExistsBehavior.Increment;
                    }
                    RollInterval rollInterval;
                    var rollIntervalParseResult = Enum.TryParse(tLogconfig.Interval.ToString(), out rollInterval);
                    if (!rollIntervalParseResult)
                    {
                        rollInterval = RollInterval.Minute;
                    }
                    observableEventListener.FilterOnTrigger(r => r.FormattedMessage == logconfig.Id)
                        .LogToRollingFlatFile(@tLogconfig.LogFilePath + logconfig.LogFileName, logMaxSize,
                            tLogconfig.TimeStampPattern, rollFileExistsBehavior
                            , rollInterval, jsonEventTextFormatter,
                            tLogconfig.MaxArchivedFile);
                    observableEventListener.EnableEvents(BaseEventSource.Log, EventLevel.LogAlways, Keywords.All);

                    if (LogConfigQueue.TryAdd(tLogconfig.Id, tLogconfig))
                    {
                        TraceLogHelper.WriteTraceLog($"Load log configuration[{tLogconfig.Id}] successfully");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                TraceLogHelper.WriteTraceLog($"Load log configuration exception:{ex}");
                return false;
            }
        }

        public static void WriteInfomationLog(string logName, string messageCode, string parameter)
            => BaseEventSource.Log.Infomation(logName, messageCode.FormatParameter(parameter));

        public static void WriteErrorLog(string logName, string messageCode, string parameter)
            => BaseEventSource.Log.Error(logName, messageCode.FormatParameter(parameter));

        public static void WriteExceptionLog(string logName, string messageCode, string parameter)
            => BaseEventSource.Log.Exception(logName, messageCode.FormatParameter(parameter));

        public static void WriteWarningLog(string logName, string messageCode, string parameter)
            => BaseEventSource.Log.Warning(logName, messageCode.FormatParameter(parameter));

        public static void WriteEventLog(string logName, string messageCode, string parameter)
            => BaseEventSource.Log.Event(logName, messageCode.FormatParameter(parameter));

        public static void WriteCriticalLog(string logName, string messageCode, string parameter)
            => BaseEventSource.Log.Critical(logName, messageCode.FormatParameter(parameter));

        public static void WriteVerboseLog(string logName, string messageCode, string parameter)
            => BaseEventSource.Log.Verbose(logName, messageCode.FormatParameter(parameter));
    }
}