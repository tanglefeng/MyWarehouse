using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.SystemSequence
{
    public interface ISystemSequenceRepository : IRepositoryForSyncDb<string, SystemSequence>
    {
        string GetNextValue(string keyValue, string formatProvider);
    }
}