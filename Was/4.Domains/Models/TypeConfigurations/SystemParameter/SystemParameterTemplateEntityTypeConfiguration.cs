using Kengic.Was.Domain.Entity.SystemParameter;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.SystemParameters
{
    public class SystemParameterTemplateEntityTypeConfiguration
        : EntityForTimeEntityTypeConfiguration<SystemParameterTemplate>
    {
        public SystemParameterTemplateEntityTypeConfiguration()
        {
            Property(t => t.Value).HasMaxLength(256);
        }
    }
}