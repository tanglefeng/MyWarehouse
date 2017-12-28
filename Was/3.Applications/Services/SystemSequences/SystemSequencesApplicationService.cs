using System;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.SystemSequence;

namespace Kengic.Was.Application.Services.SystemSequences
{
    public class SystemSequencesApplicationService : ISystemSequencesApplicationService
    {
        private readonly ISystemSequenceRepository _theRepository;

        public SystemSequencesApplicationService(ISystemSequenceRepository theRepository)
        {
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(SystemSequence value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(SystemSequence value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(SystemSequence value) => _theRepository.Remove(value);
        public IQueryable<SystemSequence> GetAll() => _theRepository.GetAll();
    }
}