using System;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.CrossCutting.Logging;
using Kengic.Was.CrossCutting.MessageCodes;
using Kengic.Was.Domain.Entity.SystemParameter;

namespace Kengic.Was.Application.Services.SystemParameters
{
    public class SystemParamtersTemplateApplicationService : ISystemParamtersTemplateApplicationService
    {
        private readonly ISystemParameterRepository _systemParameterRepository;
        private readonly ISystemParameterTemplateRepository _theRepository;

        public SystemParamtersTemplateApplicationService(ISystemParameterTemplateRepository theRepository,
            ISystemParameterRepository systemParameterRepository)
        {
            _theRepository = theRepository;
            _systemParameterRepository = systemParameterRepository;

            //var initializeData = ConfigurationManager.AppSettings[StaticParameterForMessage.InitializeData];
            //if (initializeData == "1")
            //{
            //    InitializeSystemParameterTemplate();
            //}
        }

        public Tuple<bool, string> Create(SystemParameterTemplate value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(SystemParameterTemplate value) => _theRepository.Update(value);

        public Tuple<bool, string> Remove(SystemParameterTemplate value)
        {
            var valueList = _systemParameterRepository.GetValueByTemplate(value.Id);
            if ((valueList != null) && (valueList.Count > 0))
            {
                const string messageCode = StaticParameterForMessage.RelationObjectIsExist;
                LogRepository.WriteInfomationLog(_theRepository.LogName, messageCode, value.Id);
                return new Tuple<bool, string>(false, messageCode);
            }
            return _theRepository.Remove(value);
        }

        public IQueryable<SystemParameterTemplate> GetAll() => _theRepository.GetAll();

        internal void InitializeSystemParameterTemplate()
        {
            var root = XElement.Load(@"./Configs/Sds/Initialization/SystemParameterTemplate.config");
            var ns = root.GetDefaultNamespace();
            var systemParameterTemplatesList =
                root.Elements(ns + "SystemParameterTemplates").Elements(ns + "SystemParameterTemplate");
            foreach (var theSystemParameterTemplate in systemParameterTemplatesList)
            {
                var systemParameterTemplate = new SystemParameterTemplate
                {
                    Id = theSystemParameterTemplate.Attribute("Id")?.Value,
                    Code = theSystemParameterTemplate.Attribute("Code")?.Value,
                    Name = theSystemParameterTemplate.Attribute("Name")?.Value,
                    Description = theSystemParameterTemplate.Attribute("Description")?.Value,
                    CreateTime = DateTime.Now,
                    CreateBy = GetType().Name
                };

                var oldSystemParameterTemplate = _theRepository.TryGetValue(systemParameterTemplate.Id);
                if (oldSystemParameterTemplate == null)
                {
                    Create(systemParameterTemplate);
                }
                else
                {
                    Update(systemParameterTemplate);
                }
            }
        }
    }
}