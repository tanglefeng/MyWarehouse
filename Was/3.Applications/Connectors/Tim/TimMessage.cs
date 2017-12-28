using System;
using System.Collections.Generic;

namespace Kengic.Was.Connector.Tim
{
    public class TimMessage
    {
        public List<TimBody> TimBodyList = new List<TimBody>();

        public TimMessage(string id)
        {
            Id = id;
        }

        public TimHeader TimHeader { get; set; }
        public DateTime MessageTime { get; set; }
        public string Id { get; set; }
    }

    public class TimHeader
    {
        public string Version { get; set; }
        public string Protocol { get; set; }
        public string DatagramCounter { get; set; }
        public string ReturnValue { get; set; }
        public string DatagramLength { get; set; }
        public string SourceNode { get; set; }
        public string DestinationNode { get; set; }
        public string SequenceNumber { get; set; }
        public string FlowControl { get; set; }
        public string SourceService { get; set; }
        public string DestinationService { get; set; }
        public string OperationId { get; set; }
        public string BlockCount { get; set; }
        public string BlockLength { get; set; }
    }

    public class TimBody
    {
        public List<TimBodyProproty> TimBodyFieldList = new List<TimBodyProproty>();
        public string OperationId { get; set; }
        public int Length { get; set; }
    }

    public class TimBodyProproty
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int SequenceNo { get; set; }
        public string MapName { get; set; }
        public int Length { get; set; }
    }
}