using System;
using System.Linq;
using System.Xml.Linq;
using Kengic.Was.Domain.Entity.SystemParameter;

namespace Kengic.Was.Application.Services.SystemParameters
{
    public class SystemParamtersApplicationService : ISystemParamtersApplicationService
    {
        private readonly ISystemParameterRepository _theRepository;

        public SystemParamtersApplicationService(ISystemParameterRepository theRepository)
        {
            _theRepository = theRepository;

            //var initializeData = ConfigurationManager.AppSettings[StaticParameterForMessage.InitializeData];
            //if (initializeData == "1")
            //{
            //    InitializeSystemParameter();
            //}
        }

        public Tuple<bool, string> Create(SystemParameter value) => _theRepository.Create(value);
        public Tuple<bool, string> Update(SystemParameter value) => _theRepository.Update(value);
        public Tuple<bool, string> Remove(SystemParameter value) => _theRepository.Remove(value);
        public IQueryable<SystemParameter> GetAll() => _theRepository.GetAll();

        internal void InitializeSystemParameter()
        {
            var root = XElement.Load(@"./Configs/Sds/Initialization/SystemParameter.config");
            var ns = root.GetDefaultNamespace();
            var systemParametersList = root.Elements(ns + "SystemParameters").Elements(ns + "SystemParameter");
            foreach (var theSystemParameter in systemParametersList)
            {
                var systemParameter = new SystemParameter
                {
                    Id = theSystemParameter.Attribute("Id")?.Value,
                    Code = theSystemParameter.Attribute("Code")?.Value,
                    Name = theSystemParameter.Attribute("Name")?.Value,
                    Value = theSystemParameter.Attribute("Value")?.Value,
                    Template = theSystemParameter.Attribute("Template")?.Value,
                    Description = theSystemParameter.Attribute("Description")?.Value,
                    CreateTime = DateTime.Now,
                    CreateBy = GetType().Name
                };

                var oldSystemParameter = _theRepository.TryGetValue(systemParameter.Id);
                if (oldSystemParameter == null)
                {
                    Create(systemParameter);
                }
                else
                {
                    Update(systemParameter);
                }
            }
        }
    }
}