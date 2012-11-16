namespace eManager.Web.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<eManager.Web.Intrastructure.DepartmentDb>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(eManager.Web.Intrastructure.DepartmentDb context)
        {
            context.Departments.AddOrUpdate(d => d.Name, 
                    new Department() { Name="Engineering" },
                    new Department() { Name = "Sales" },
                    new Department() { Name = "Shipping" },
                    new Department() { Name = "Human Resources" }
                );
        }
    }
}
