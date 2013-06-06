using System.Data.Entity.Migrations;
using AppSmarts.Common.Data;

namespace AppSmarts.Common.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    }
}
