using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Complement;

namespace Kengic.Was.Domain.Complement
{
    public class ComplementSourceWorkTaskRepository : RepositoryForOnlyDb<string, ComplementSourceWorkTask>,
        IComplementSourceWorkTaskRepository
    {
        public ComplementSourceWorkTaskRepository(IQueryableUnitOfWork unitOfWork)
            : base(unitOfWork)
        {
        }
    }
}