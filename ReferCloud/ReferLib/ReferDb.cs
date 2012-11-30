<<<<<<< HEAD
﻿using ReferLib;
using System.Data.Entity;

namespace ReferLib
{
    public class ReferDb : DbContext
    {
        public ReferDb()
            : base("name=DefaultConnection") { }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonAppReferral> PersonAppReferrals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(p => p.FacebookId);
            modelBuilder.Entity<PersonAppReferral>().HasKey(p => new {p.AppId, p.PersonFacebookId});
            base.OnModelCreating(modelBuilder);
        }
    }
=======
﻿using ReferLib;
using System.Data.Entity;

namespace ReferLib
{
    public class ReferDb : DbContext
    {
        public ReferDb()
            : base("name=DefaultConnection") { }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<App> Apps { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PersonAppReferral> PersonAppReferrals { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Person>().HasKey(p => p.FacebookId);
            modelBuilder.Entity<PersonAppReferral>().HasKey(p => new {p.AppId, p.PersonFacebookId});
            base.OnModelCreating(modelBuilder);
        }
    }
>>>>>>> Facebook Post and Commit to DB
}