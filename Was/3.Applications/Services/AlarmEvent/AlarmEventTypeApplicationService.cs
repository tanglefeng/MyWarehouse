using System;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AlarmEvent;

namespace Kengic.Was.Application.Services.AlarmEvent
{
    public class AlarmEventTypeApplicationService : IAlarmEventTypeApplicationService
    {
        private readonly IAlarmEventTypeRepository _theRepository;

        public AlarmEventTypeApplicationService(
            IAlarmEventTypeRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }

            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(AlarmEventType value) => _theRepository.Create(value);
        public Tuple<bool, string> Remove(AlarmEventType value) => _theRepository.Remove(value);
        public Tuple<bool, string> Update(AlarmEventType value) => _theRepository.Update(value);
        public IQueryable<AlarmEventType> GetAll() => _theRepository.GetAll();
    }
}