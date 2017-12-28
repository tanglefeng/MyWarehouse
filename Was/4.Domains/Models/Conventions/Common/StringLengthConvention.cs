namespace Kengic.Was.Doamin.Model.Convention.Common
{
    public class StringLengthConvention : System.Data.Entity.ModelConfiguration.Conventions.Convention
    {
        public StringLengthConvention()
        {
            Properties<string>().Configure(c => c.HasMaxLength(256));
        }
    }
}