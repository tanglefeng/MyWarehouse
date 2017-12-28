using System;
using System.Diagnostics;
using System.Globalization;
using System.Security;
using Kengic.Was.CrossCutting.Common.Loggings;

namespace Kengic.Was.CrossCutting.Logging
{
    /// <summary>
    ///     <para>Implementation of contract <see cref="ILogger" /></para>
    ///     <para>using System.Diagnostics API.</para>
    /// </summary>
    public sealed class TraceSourceLog
        : ILogger
    {
        private readonly TraceSource _source;

        /// <summary>
        ///     Create a new instance of this trace manager
        /// </summary>
        public TraceSourceLog()
        {
            // Create default source
            _source = new TraceSource("Was");
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="message">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="args">
        ///     <see cref="ILogger" />
        /// </param>
        public void LogInfo(string message, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            TraceInternal(TraceEventType.Information, messageToTrace);
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="message">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="args">
        ///     <see cref="ILogger" />
        /// </param>
        public void LogWarning(string message, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            TraceInternal(TraceEventType.Warning, messageToTrace);
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="message">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="args">
        ///     <see cref="ILogger" />
        /// </param>
        public void LogError(string message, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            TraceInternal(TraceEventType.Error, messageToTrace);
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="message">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="exception">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="args">
        ///     <see cref="ILogger" />
        /// </param>
        public void LogError(string message, Exception exception, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message) || (exception == null)) return;
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            var exceptionData = exception.ToString();
            // The ToString() create a string representation of the current exception

            TraceInternal(TraceEventType.Error,
                string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageToTrace, exceptionData));
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="message">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="args">
        ///     <see cref="ILogger" />
        /// </param>
        public void Debug(string message, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            TraceInternal(TraceEventType.Verbose, messageToTrace);
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="message">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="exception">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="args">
        ///     <see cref="ILogger" />
        /// </param>
        public void Debug(string message, Exception exception, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message) || (exception == null)) return;
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            var exceptionData = exception.ToString();
            // The ToString() create a string representation of the current exception

            TraceInternal(TraceEventType.Error,
                string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageToTrace, exceptionData));
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="item">
        ///     <see cref="ILogger" />
        /// </param>
        public void Debug(object item)
        {
            if (item != null)
            {
                TraceInternal(TraceEventType.Verbose, item.ToString());
            }
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="message">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="args">
        ///     <see cref="ILogger" />
        /// </param>
        public void Fatal(string message, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            TraceInternal(TraceEventType.Critical, messageToTrace);
        }

        /// <summary>
        ///     <see cref="ILogger" />
        /// </summary>
        /// <param name="message">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="exception">
        ///     <see cref="ILogger" />
        /// </param>
        /// <param name="args"></param>
        public void Fatal(string message, Exception exception, params object[] args)
        {
            if (string.IsNullOrWhiteSpace(message) || (exception == null)) return;
            var messageToTrace = string.Format(CultureInfo.InvariantCulture, message, args);

            var exceptionData = exception.ToString();
            // The ToString() create a string representation of the current exception

            TraceInternal(TraceEventType.Critical,
                string.Format(CultureInfo.InvariantCulture, "{0} Exception:{1}", messageToTrace, exceptionData));
        }

        /// <summary>
        ///     <see cref="Trace" /> <see langword="internal" />
        ///     <paramref name="message" /> in configured listeners
        /// </summary>
        /// <param name="eventType">Event type to trace</param>
        /// <param name="message">Message of event</param>
        private void TraceInternal(TraceEventType eventType, string message)
        {
            if (_source == null) return;
            try
            {
                _source.TraceEvent(eventType, (int) eventType, message);
            }
            catch (SecurityException)
            {
                //Cannot access to file listener or cannot have
                //privileges to write in event log etc...
            }
        }
    }
}