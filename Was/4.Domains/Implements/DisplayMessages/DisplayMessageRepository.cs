using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.DisplayMessage;

namespace Kengic.Was.Domain.DisplayMessages
{
    public class DisplayMessageRepository : RepositoryForOnlyDb<string, DisplayMessage>, IDisplayMessageRepository
    {
        public DisplayMessageRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}