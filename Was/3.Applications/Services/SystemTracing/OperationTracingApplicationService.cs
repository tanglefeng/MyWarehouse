using System;
using System.Linq;
using Kengic.Was.Domain.Entity.SystemTracing;

namespace Kengic.Was.Application.Services.SystemTracing
{
    public class OperationTracingApplicationService : IOperationTracingApplicationService
    {
        private readonly IOperationTracingRepository _theRepository;

        public OperationTracingApplicationService(IOperationTracingRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(OperationTracing value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(OperationTracing value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(OperationTracing value) => _theRepository.Remove(value);
        public IQueryable<OperationTracing> GetAll() => _theRepository.GetAll();
    }
}