using ReferEngine.Common.Models;
using ReferEngine.Common.Models.iOS;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ReferEngine.Common.Data.iOS
{
    public class iOSDatabaseContext : DbContext
    {
        public iOSDatabaseContext() { }

        public iOSDatabaseContext(string connection) : base(connection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<iOSApp> iOSApps { get; set; }
        public DbSet<iOSAppArtist> iOSAppArtists { get; set; }
        public DbSet<iOSAppDetail> iOSAppDetails { get; set; }
        public DbSet<iOSAppDeviceType> iOSAppDeviceTypes { get; set; }
        public DbSet<iOSAppGenre> iOSAppGenres { get; set; }
        public DbSet<iOSAppPopularityPerGenre> iOSAppPopularityPerGenres { get; set; }
        public DbSet<iOSAppPrice> iOSAppPrices { get; set; }
        public DbSet<iOSArtist> iOSArtists { get; set; }
        public DbSet<iOSArtistType> iOSArtistTypes { get; set; }
        public DbSet<iOSDeviceType> iOSDeviceTypes { get; set; }
        public DbSet<iOSGenre> iOSGenres { get; set; }
        public DbSet<iOSMediaType> iOSMediaTypes { get; set; }
        public DbSet<iOSStorefront> iOSStorefronts { get; set; }
        public DbSet<AppScreenshot> AppScreenshots { get; set; }
        public DbSet<CloudinaryImage> CloudinaryImages { get; set; }
        public DbSet<iOSDataImportStep> DataImportSteps { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Entity<iOSApp>().HasKey(a => a.Id);
            mb.Entity<iOSApp>().Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<iOSArtist>().HasKey(a => a.Id );
            mb.Entity<iOSArtist>().Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<iOSArtistType>().HasKey(a => a.Id);
            mb.Entity<iOSArtistType>().Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<iOSDeviceType>().HasKey(a => a.Id);
            mb.Entity<iOSDeviceType>().Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<iOSGenre>().HasKey(a => a.Id);
            mb.Entity<iOSGenre>().Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<iOSMediaType>().HasKey(a => a.Id);
            mb.Entity<iOSMediaType>().Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<iOSStorefront>().HasKey(a => a.Id);
            mb.Entity<iOSStorefront>().Property(a => a.Id).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            base.OnModelCreating(mb);
        }
    }
}