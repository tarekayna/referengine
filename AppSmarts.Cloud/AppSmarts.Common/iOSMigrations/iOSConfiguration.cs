using System.Data.Entity.Migrations;
using AppSmarts.Common.Data.iOS;

namespace AppSmarts.Common.iOSMigrations
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
