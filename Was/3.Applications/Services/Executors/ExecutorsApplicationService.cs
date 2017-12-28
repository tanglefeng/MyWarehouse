using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.Executor;
using Kengic.Was.Domain.Entity.SystemParameter;

namespace Kengic.Was.Application.Services.Executors
{
    public class ExecutorsApplicationService : IExecutorsApplicationService
    {
        //readonly IRouteEdgeRepository _edgeRepository;
        private readonly ISystemParameterRepository _systemParameterRepository;
        private readonly IWasExecutorRepository _theRepository;

        public ExecutorsApplicationService(IWasExecutorRepository theRepository,
            ISystemParameterRepository systemParameterRepository)
        {
            _theRepository = theRepository;
            //_edgeRepository = edgeRepository;
            _systemParameterRepository = systemParameterRepository;
        }

        public Tuple<bool, string> Create(WasExecutor value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(WasExecutor value) => _theRepository.Update(value);

        public Tuple<bool, string> Remove(WasExecutor value)
        {
            //    var systemModel = _systemParameterRepository.TryGetValue(StaticParameterForMessage.SystemModel);
            //    if ((systemModel != null) && (systemModel.Value != StaticParameterForMessage.SdsModel))
            //    {
            //        var valueList = _edgeRepository.GetValueByExecutor(value.Id);
            //        if ((valueList != null) && (valueList.Count > 0))
            //        {
            //            const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
            //            LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
            //            return new Tuple<bool, string>(false, messageCode);
            //        }
            //    }
            return _theRepository.Remove(value);
        }

        public List<WasExecutor> GetValueByOperator(string value) => _theRepository.GetValueByOperator(value);
        public IQueryable<WasExecutor> GetAll() => _theRepository.GetAll();
    }
}