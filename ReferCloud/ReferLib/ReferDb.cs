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
        public DbSet<AppReferral> AppReferrals { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<TimelinePost> TimelinePosts { get; set; }

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

            mb.Entity<Person>().HasKey(p => p.FacebookId);
            mb.Entity<Person>().Property(p => p.FacebookId)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<AppReferral>().HasKey(r => r.FacebookPostId);
            mb.Entity<AppReferral>().Property(r => r.FacebookPostId)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            mb.Entity<AppReferral>().Property(r => r.AppId).IsRequired();
            mb.Entity<AppReferral>().Property(r => r.PersonFacebookId).IsRequired();
            mb.Entity<AppReferral>().Property(r => r.DateTime).IsRequired();

            mb.Entity<TimelinePost>().HasKey(p => p.FacebookPostId);
            mb.Entity<TimelinePost>().Property(p => p.FacebookPostId)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            mb.Entity<TimelinePost>().Property(p => p.FriendFacebookId).IsRequired();
            mb.Entity<TimelinePost>().Property(p => p.Message).IsRequired();
            mb.Entity<TimelinePost>().Property(p => p.PersonFacebookId).IsRequired();
            mb.Entity<TimelinePost>().Property(p => p.DateTime).IsRequired();

            base.OnModelCreating(mb);
        }
    }
}