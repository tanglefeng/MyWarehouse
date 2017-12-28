using Kengic.Was.Domain.Entity.SystemParameter;
using Kengic.Was.Model.TypeConfiguration.Commons;

namespace Kengic.Was.Model.TypeConfiguration.SystemParameters
{
    internal class SystemParameterEntityTypeConfiguration
        : EntityForTimeEntityTypeConfiguration<SystemParameter>
    {
        public SystemParameterEntityTypeConfiguration()
        {
            Property(t => t.Value).HasMaxLength(256);
            Property(t => t.Template).HasMaxLength(256);
        }
    }
}