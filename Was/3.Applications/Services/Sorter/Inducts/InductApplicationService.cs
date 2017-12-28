using System;
using System.Linq;
using Kengic.Was.Domain.Entity.Sorter.Inducts;

namespace Kengic.Was.Application.Services.Sorter.Inducts
{
    public class InductApplicationService : IInductApplicationService
    {
        private readonly IInductRepository _theRepository;

        public InductApplicationService(
            IInductRepository theRepository)
        {
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(Induct value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(Induct value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(Induct value) => _theRepository.Remove(value);
        public IQueryable<Induct> GetAll() => _theRepository.GetAll();
    }
}