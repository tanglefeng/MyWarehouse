using Kengic.Was.CrossCutting.ConfigurationSection.Operators;

namespace Kengic.Was.Operator.Common
{
    public interface IOperator
    {
        string Id { get; set; }
        string Name { get; set; }
        OperatorElement OperatorElement { get; set; }
        bool IsReciveMessage { get; set; }
        string LogName { get; set; }
        OperatorStatus Status { get; set; }
        bool Initialize();
        bool Start();
        bool Close();
    }

    public enum OperatorStatus
    {
        Initialize = 10,
        Strat = 20,
        Active = 30,
        Closing = 40,
        Exception = 50
    }
}