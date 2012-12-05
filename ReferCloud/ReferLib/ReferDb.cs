using System.ComponentModel.DataAnnotations.Schema;
using ReferLib;
using System.Data.Entity;

namespace ReferLib
{
    public class ReferDb : DbContext
    {
        public ReferDb()
            : base("name=DefaultConnection") { }

        public DbSet<App> Apps { get; set; }
        public DbSet<AppRecommendation> AppRecommendations { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Person> People { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Entity<App>().Property(a => a.AppStoreLink).IsRequired();
            mb.Entity<App>().Property(a => a.Description).IsRequired();
            mb.Entity<App>().Property(a => a.DeveloperId).IsRequired();
            mb.Entity<App>().Property(a => a.ImageLink).IsRequired();
            mb.Entity<App>().Property(a => a.Name).IsRequired();
            mb.Entity<App>().Property(a => a.Platform).IsRequired();
            mb.Entity<App>().Property(a => a.Publisher).IsRequired();
            mb.Entity<App>().Property(a => a.ShortDescription).IsRequired();

            mb.Entity<Developer>().Property(d => d.City).IsRequired();
            mb.Entity<Developer>().Property(d => d.Country).IsRequired();
            mb.Entity<Developer>().Property(d => d.Email).IsRequired();
            mb.Entity<Developer>().Property(d => d.FirstName).IsRequired();
            mb.Entity<Developer>().Property(d => d.LastName).IsRequired();
            mb.Entity<Developer>().Property(d => d.State).IsRequired();
            mb.Entity<Developer>().Property(d => d.ZipCode).IsRequired();

            mb.Entity<Friendship>().Property(f => f.Person1FacebookId).IsRequired();
            mb.Entity<Friendship>().Property(f => f.Person2FacebookId).IsRequired();
            mb.Entity<Friendship>().Property(f => f.UpdatedDateTime).IsRequired();

            mb.Entity<Person>().HasKey(p => p.FacebookId);
            mb.Entity<Person>().Property(p => p.FacebookId)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<AppRecommendation>().HasKey(r => r.FacebookPostId);
            mb.Entity<AppRecommendation>().Property(r => r.FacebookPostId)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            mb.Entity<AppRecommendation>().Property(r => r.AppId).IsRequired();
            mb.Entity<AppRecommendation>().Property(r => r.PersonFacebookId).IsRequired();
            mb.Entity<AppRecommendation>().Property(r => r.DateTime).IsRequired();

            base.OnModelCreating(mb);
        }
    }
}