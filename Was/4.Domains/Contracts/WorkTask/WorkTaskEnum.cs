namespace Kengic.Was.Domain.Entity.WorkTask
{
    public enum TransportMessageType
    {
        Default = 0,
        DestinationRequest = 10,
        BlockRequest = 20,
        ApRequest = 30,
        LeftRequest = 40,
        DoubleReceive = 50,
        EmptyRetrieval
    }

    public enum TransportWorkTaskType
    {
        Default = 0,
        Storage = 10,
        Retrival = 20,
        Transport = 30,
        SwapTransport = 35,
        TransportBlock = 40,
        DoubleRec = 50,
        EmptyRetrival = 51,
        RetrivalOccupy = 62
    }

    public enum WorkTaskTriggerMode
    {
        Default = 0,
        Immediately = 10,
        Interval = 20,
        Time = 30,
        Event = 40
    }

    public enum WorkTaskStatus
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
        Faulted = 55,
        Timeout = 60,
        Resume = 65
    }


    public enum WorkTaskSplitMode
    {
        Default = 0,
        Dynamic = 10,
        Overall = 20
    }

    public enum WorkTaskType
    {
        Default = 0,
        Storage = 201,
        Retrival = 202,
        Transport = 203,
        Sorting = 300,
        Complement = 301
    }
}