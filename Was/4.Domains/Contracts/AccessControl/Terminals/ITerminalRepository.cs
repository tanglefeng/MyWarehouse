using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.AccessControl.Terminals
{
    public interface ITerminalRepository : IRepositoryForOnlyDb<string, Terminal>
    {
    }
}