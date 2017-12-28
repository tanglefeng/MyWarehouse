using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.AccessControl.Terminals;

namespace Kengic.Was.Domain.AccessControl.Terminals
{
    public class TerminalRepository : RepositoryForOnlyDb<string, Terminal>, ITerminalRepository
    {
        public TerminalRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}