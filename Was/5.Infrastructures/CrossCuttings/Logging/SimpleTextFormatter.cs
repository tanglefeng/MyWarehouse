using System;
using System.IO;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging;
using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;

namespace Kengic.Was.CrossCutting.Logging
{
    public class SimpleTextFormatter : IEventTextFormatter
    {
        public SimpleTextFormatter(string header, string footer, string dateTimeFormat)
        {
            Header = header;
            Footer = footer;
            DateTimeFormat = dateTimeFormat;
        }

        public string Header { get; set; }
        public string Footer { get; set; }
        public string DateTimeFormat { get; set; }

        public void WriteEvent(EventEntry eventEntry, TextWriter writer)
        {
            // Write header
            if (!string.IsNullOrWhiteSpace(Header))
            {
                writer.WriteLine(Header);
            }
            // Write properties
            writer.WriteLine("Timestamp : {0}", DateTime.Now.ToString(DateTimeFormat));
            //writer.WriteLine("SourceId : {0}", eventEntry.ProviderId);
            //writer.WriteLine("EventId : {0}", eventEntry.EventId);
            //writer.WriteLine("Keywords : {0}", eventEntry.Schema.Keywords);
            writer.WriteLine("Level : {0}", eventEntry.Schema.Level);
            //writer.WriteLine("Message : {0}", eventEntry.FormattedMessage);
            //writer.WriteLine("Opcode : {0}", eventEntry.Schema.Opcode);
            //writer.WriteLine("Task : {0} {1}", eventEntry.Schema.Task, eventEntry.Schema.TaskName);
            //writer.WriteLine("Version : {0}", eventEntry.Schema.Version);
            writer.WriteLine("Payload :{0}", FormatPayload(eventEntry));

            // Write footer
            if (!string.IsNullOrWhiteSpace(Footer))
            {
                writer.WriteLine(Footer);
            }
            writer.WriteLine();
        }

        private static string FormatPayload(EventEntry entry)
        {
            var eventSchema = entry.Schema;
            var sb = new StringBuilder();
            for (var i = 0; i < entry.Payload.Count; i++)
            {
                // Any errors will be handled in the sink 
                sb.AppendFormat("{0} : {1}", eventSchema.Payload[i], entry.Payload[i]);
            }
            return sb.ToString();
        }
    }
}