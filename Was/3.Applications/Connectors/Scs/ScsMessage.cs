using System;
using System.Collections.Generic;

namespace Kengic.Was.Connector.Scs
{
    public class ScsServerMessage
    {
        public List<ScsBody> ScsBodyList = new List<ScsBody>();

        public ScsServerMessage(string id)
        {
            Id = id;
        }

        public ScsServerHeader ScsHeader { get; set; }
        public DateTime MessageTime { get; set; }
        public string Id { get; set; }
    }

    public class ScsClientMessage
    {
        public List<ScsBody> ScsBodyList = new List<ScsBody>();

        public ScsClientMessage(string id)
        {
            Id = id;
        }

        public ScsClientHeader ScsHeader { get; set; }
        public DateTime MessageTime { get; set; }
        public string Id { get; set; }
    }

    public class ScsClientHeader
    {
        public string StartChar { get; set; }
        public string SourceId { get; set; }
        public string Index { get; set; }
        public string OperationId { get; set; }
        public string NumOfDataBytes { get; set; }
    }

    public class ScsServerHeader
    {
        public string OperationId { get; set; }
        public string NumOfDataBytes { get; set; }
    }

    public class ScsBody
    {
        public List<ScsBodyProproty> ScsBodyFieldList = new List<ScsBodyProproty>();
        public string OperationId { get; set; }
        public int Length { get; set; }
    }

    public class ScsBodyProproty
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public int SequenceNo { get; set; }
        public string MapName { get; set; }
        public int Length { get; set; }
    }
}