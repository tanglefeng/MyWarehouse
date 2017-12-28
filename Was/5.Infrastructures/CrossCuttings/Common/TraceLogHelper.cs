using System;
using System.Diagnostics;

namespace Kengic.Was.CrossCutting.Common
{
    public class TraceLogHelper
    {
        public static void WriteTraceLog(string message)
        {
            var logMessage = $"{DateTime.Now.ToString(DateTimeHelper.Windowffffff)}: {message}";
            Trace.WriteLine(logMessage);
        }
    }
}