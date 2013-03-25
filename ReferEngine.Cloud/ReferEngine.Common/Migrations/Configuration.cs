using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using ReferEngine.Common.Data;
using ReferEngine.Common.Models;

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
