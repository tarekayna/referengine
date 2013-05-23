using ReferEngine.Common.Data;
using System.Data.Entity.Migrations;

namespace ReferEngine.Common.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
