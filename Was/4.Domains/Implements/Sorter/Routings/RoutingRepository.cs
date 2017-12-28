using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Domain.Common;
using Kengic.Was.Domain.Entity.Sorter.Routings;

namespace Kengic.Was.Domain.Sorter.Routings
{
    public class RoutingRepository : RepositoryForSyncDb<string, Routing>, IRoutingRepository
    {
        public RoutingRepository(IQueryableUnitOfWork queryableUnitOfWork) : base(queryableUnitOfWork)
        {
        }

        public List<Routing> GetPhysicalShuteByLogicalShute(string logicalDestination, string sorterPlan)
            => ValueQueue.Values.Where(
                e => (e.LogicalDestination == logicalDestination) && (e.SorterPlan == sorterPlan))
                .OrderBy(e => e.InternalPriority)
                .ThenBy(e => e.PhycialShute)
                .ToList();

        public List<Routing> GetLogicalDestinationByPhysicalShute(string physicalShute, string sorterPlan)
            => ValueQueue.Values.Where(e => (e.PhycialShute == physicalShute) && (e.SorterPlan == sorterPlan))
                .ToList();

        public List<Routing> GetPhysicalShute(string value)
            => ValueQueue.Values.Where(e => e.PhycialShute == value).ToList();

        public List<Routing> GetValueByLogicalDestination(string value)
            => ValueQueue?.Values?.Where(e => e.LogicalDestination == value).ToList();

        public List<Routing> GetValueBySorterPlan(string value)
            => ValueQueue.Values.Where(e => e.SorterPlan == value).ToList();
    }
}