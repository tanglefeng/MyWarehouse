using System;
using System.Linq;
using Kengic.Was.Connector.Common;
using Kengic.Was.Connector.Prodave;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Sorter.Parameters;

namespace Kengic.Was.Application.Services.Sorter.SorterParameters
{
    public class SorterParameterApplicationService : ISorterParameterApplicationService
    {
        private readonly ISorterParameterRepository _theRepository;

        public SorterParameterApplicationService(ISorterParameterRepository theRepository)
        {
            _theRepository = theRepository;
        }

        public Tuple<bool, string> Create(SorterParameter value)
        {
            var rtnValue = WritePlcMessage(value);
            return rtnValue.Item1 ? _theRepository.Create(value) : rtnValue;
        }

        public Tuple<bool, string> Update(SorterParameter value)
        {
            var rtnValue = WritePlcMessage(value);
            return rtnValue.Item1 ? _theRepository.Update(value) : rtnValue;
        }

        public Tuple<bool, string> Remove(SorterParameter value) => _theRepository.Remove(value);

        public IQueryable<SorterParameter> GetAll() => _theRepository.GetAll();

        private static Tuple<bool, string> WritePlcMessage(SorterParameter value)
        {
            var returnValue = false;
            try
            {
                var connector = ConnectorsRepository.GetConnectorInstance(value.ConnectionName);
                if (connector == null)
                {
                    return new Tuple<bool, string>(false, StaticParameterForMessage.ConnectorIsNotExist);
                }

                switch (value.ValueType.ToUpper())
                {
                    case "INT":
                        returnValue = ((ProdaveClient) connector).WriteInt16((ushort) value.StorageDb,
                            value.StartAddress,
                            Convert.ToUInt16(value.Value));
                        break;
                    case "STRING":

                        returnValue = ((ProdaveClient) connector).WriteString((ushort) value.StorageDb,
                            value.StartAddress,
                            value.Value);

                        break;
                }

                LogRepository.WriteInfomationLog(value.ConnectionName,
                    returnValue ? StaticParameterForMessage.UpdateSuccess : StaticParameterForMessage.UpdateFailure,
                    value.StorageDb + "." + value.StartAddress + ":" + value.Value);

                return returnValue
                    ? new Tuple<bool, string>(true, StaticParameterForMessage.Ok)
                    : new Tuple<bool, string>(false, StaticParameterForMessage.SendMessageFailure);
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, ex.ToString());
            }
        }
    }
}