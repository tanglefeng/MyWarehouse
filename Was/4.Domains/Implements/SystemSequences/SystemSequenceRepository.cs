using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.SystemSequence;

namespace Kengic.Was.Domain.SystemSequences
{
    public class SystemSequenceRepository : RepositoryForSyncDb<string, SystemSequence>, ISystemSequenceRepository
    {
        public SystemSequenceRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public string GetNextValue(string keyId, string formatProvider)
        {
            var value = TryGetValue(keyId);
            if (value != null)
            {
                var increateValue = value.Value + value.IncreaseRate;
                if ((increateValue >= value.MinValue) && (increateValue <= value.MaxValue))
                {
                    return value.Prefix + increateValue.ToString(formatProvider);
                }
            }
            return null;
        }
    }
}