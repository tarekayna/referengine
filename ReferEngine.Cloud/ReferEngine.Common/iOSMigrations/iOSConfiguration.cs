using ReferEngine.Common.Data.iOS;
using System.Data.Entity.Migrations;

namespace ReferEngine.Common.iOSMigrations
{
    internal sealed class iOSConfiguration : DbMigrationsConfiguration<iOSDatabaseContext>
    {
        public iOSConfiguration()
        {
            AutomaticMigrationsEnabled = true;
            MigrationsDirectory = "iOSMigrations";
        }
    }
}
