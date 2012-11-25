namespace ReferEngine.Migrations
{
    using System.Data.Entity.Migrations;
    using ReferEngine.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ReferDb>
    {
        public Configuration()
        {
            // TODO: Set this to false 
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(ReferDb context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
