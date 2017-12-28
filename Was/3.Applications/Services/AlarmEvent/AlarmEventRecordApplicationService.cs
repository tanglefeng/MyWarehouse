using System;
using System.Collections.Generic;
using System.Linq;
using Kengic.Was.Connector.Common;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.AlarmEvent;

namespace Kengic.Was.Application.Services.AlarmEvent
{
    public class AlarmEventRecordApplicationService : IAlarmEventRecordApplicationService
    {
        private readonly IAlarmEventRecordRepository _theRepository;

        public AlarmEventRecordApplicationService(IAlarmEventRecordRepository theRepository)
        {
            if (theRepository == null)
            {
                throw new ArgumentNullException(nameof(theRepository));
            }
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(AlarmEventRecord value)
        {
            value.Status = AlarmEventStatus.Create;
            return _theRepository.Create(value);
        }

        public Tuple<bool, string> Update(AlarmEventRecord value)
        {
            var connector = ConnectorsRepository.GetConnectorInstance(value.Object);
            if ((connector != null) && (value.Code == StaticParameterForMessage.Disconnect) &&
                (value.Status == AlarmEventStatus.Removed))
            {
                connector.AlarmActiveStatus = false;
            }
            return _theRepository.Update(value);
        }

        public Tuple<bool, string> Remove(AlarmEventRecord value)
        {
            var connector = ConnectorsRepository.GetConnectorInstance(value.Object);
            if ((connector != null) && (value.Code == StaticParameterForMessage.Disconnect))
            {
                connector.AlarmActiveStatus = false;
            }

            return _theRepository.Remove(value);
        }

        public IEnumerable<AlarmEventRecord> GetWithStatus() => _theRepository.GetWithStatus();
        public int GetDataActiveAlarmNumber() => _theRepository.GetWithStatus().Count();
        public IQueryable<AlarmEventRecord> GetAll() => _theRepository.GetAll();
    }
}