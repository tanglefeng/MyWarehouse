using System;
using System.Diagnostics.Tracing;

namespace Kengic.Was.CrossCutting.Logging
{
    [EventSource(Name = "Kengc-Was-BaseEventSource")]
    public sealed class BaseEventSource : EventSource
    {
        private static readonly Lazy<BaseEventSource> Instance = new Lazy<BaseEventSource>(() => new BaseEventSource());

        private BaseEventSource()
        {
        }

        public static BaseEventSource Log => Instance.Value;

        [Event(101, Message = "{0}", Level = Levels.Critical)]
        public void Critical(string logName, string critical) => WriteEvent(101, logName, critical);

        [Event(102, Message = "{0}", Level = Levels.Error)]
        public void Error(string logName, string error) => WriteEvent(102, logName, error);

        [Event(103, Message = "{0}", Level = Levels.Warning)]
        public void Warning(string logName, string warning) => WriteEvent(103, logName, warning);

        [Event(104, Message = "{0}", Level = Levels.Informational)]
        public void Infomation(string logName, string informational) => WriteEvent(104, logName, informational);

        [Event(105, Message = "{0}", Level = Levels.Verbose)]
        public void Verbose(string logName, string verbose) => WriteEvent(105, logName, verbose);

        [Event(106, Message = "{0}", Level = Levels.Event)]
        public void Event(string logName, string evnet) => WriteEvent(106, logName, evnet);

        [Event(107, Message = "{0}", Level = Levels.Exception)]
        public void Exception(string logName, string exception) => WriteEvent(107, logName, exception);

        public static class Keywords
        {
            public const EventKeywords Application = (EventKeywords) 1L;
            public const EventKeywords DataAccess = (EventKeywords) 2L;
            public const EventKeywords UserInterface = (EventKeywords) 4L;
            public const EventKeywords General = (EventKeywords) 8L;
        }

        public static class Tasks
        {
            public const EventTask LoadUser = (EventTask) 1;
            public const EventTask LoadExpenses = (EventTask) 2;
            public const EventTask LoadAllExpenses = (EventTask) 3;
            public const EventTask SaveExpense = (EventTask) 4;
            public const EventTask GetExpensesForApproval = (EventTask) 5;
            public const EventTask Initialize = (EventTask) 6;
            public const EventTask Tracing = (EventTask) 7;
            public const EventTask ApproveExpense = (EventTask) 8;
        }

        public static class Opcodes
        {
            public const EventOpcode Start = (EventOpcode) 20;
            public const EventOpcode Finish = (EventOpcode) 21;
            public const EventOpcode Error = (EventOpcode) 22;
            public const EventOpcode Starting = (EventOpcode) 23;
            public const EventOpcode QueryStart = (EventOpcode) 30;
            public const EventOpcode QueryFinish = (EventOpcode) 31;
            public const EventOpcode QueryNoResults = (EventOpcode) 32;
            public const EventOpcode CacheQuery = (EventOpcode) 40;
            public const EventOpcode CacheUpdate = (EventOpcode) 41;
            public const EventOpcode CacheHit = (EventOpcode) 42;
            public const EventOpcode CacheMiss = (EventOpcode) 43;
        }

        public static class Levels
        {
            public const EventLevel LogAlways = 0L;
            public const EventLevel Critical = (EventLevel) 1L;
            public const EventLevel Error = (EventLevel) 2L;
            public const EventLevel Warning = (EventLevel) 3L;
            public const EventLevel Informational = (EventLevel) 4L;
            public const EventLevel Verbose = (EventLevel) 5L;
            public const EventLevel Event = (EventLevel) 6L;
            public const EventLevel Exception = (EventLevel) 7L;
        }
    }
}