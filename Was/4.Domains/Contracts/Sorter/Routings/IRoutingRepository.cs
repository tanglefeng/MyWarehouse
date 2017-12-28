using System.Collections.Generic;
using Kengic.Was.Domain.Entity.Common;

namespace Kengic.Was.Domain.Entity.Sorter.Routings
{
    public interface IRoutingRepository : IRepositoryForSyncDb<string, Routing>
    {
        List<Routing> GetPhysicalShuteByLogicalShute(string logicalDestination, string sorterPlan);
        List<Routing> GetLogicalDestinationByPhysicalShute(string physicalShute, string sorterPlan);
        List<Routing> GetPhysicalShute(string value);
        List<Routing> GetValueByLogicalDestination(string value);
        List<Routing> GetValueBySorterPlan(string value);
    }
}