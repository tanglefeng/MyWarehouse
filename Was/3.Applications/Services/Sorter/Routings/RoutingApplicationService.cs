using System;
using System.Linq;
using Kengic.Was.Domain.Entity.Sorter.Routings;

namespace Kengic.Was.Application.Services.Sorter.Routings
{
    public class RoutingApplicationService : IRoutingApplicationService
    {
        private readonly IRoutingRepository _theRepository;

        public RoutingApplicationService(
            IRoutingRepository theRepository)
        {
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(Routing value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(Routing value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(Routing value) => _theRepository.Remove(value);
        public IQueryable<Routing> GetAll() => _theRepository.GetAll();
    }
}