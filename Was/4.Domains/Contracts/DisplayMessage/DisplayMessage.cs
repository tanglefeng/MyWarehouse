using System;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.DisplayMessage
{
    public class DisplayMessage : EntityForTime<string>
    {
        public string ObjectToHandle { get; set; }
        public string Source { get; set; }
        public string SourceAddress { get; set; }
        public string DestinationAddress { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public string Comments { get; set; }
        public DisplayMessageStatus Status { get; set; }
        public string ReadyBy { get; set; }
        public DateTime? ReadyTime { get; set; }
        public string ReleaseBy { get; set; }
        public DateTime? ReleaseTime { get; set; }
        public string ActiveBy { get; set; }
        public DateTime? ActiveTime { get; set; }
        public string CancelledBy { get; set; }
        public DateTime? CancelledTime { get; set; }
        public string TerminatedBy { get; set; }
        public DateTime? TerminatedTime { get; set; }
        public string SuspendedBy { get; set; }
        public DateTime? SuspendedTime { get; set; }
        public DateTime? CompleteTime { get; set; }
        public DateTime? FaultTime { get; set; }
    }

    public enum DisplayMessageStatus
    {
        Default = 0,
        Create = 10,
        Ready = 15,
        Release = 20,
        Active = 25,
        Running = 30,
        Suspended = 35,
        Completed = 40,
        Cancelled = 45,
        Terminated = 50,
        Faulted = 55
    }
}