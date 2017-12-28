using System;

namespace Kengic.Was.Doamin.Model.Convention.SqlServer
{
    public class DateTime2Convention : System.Data.Entity.ModelConfiguration.Conventions.Convention
    {
        public DateTime2Convention()
        {
            Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));
        }
    }
}