﻿using System.ComponentModel.DataAnnotations.Schema;
using ReferEngine.Common.Models;
using System.Data.Entity;

namespace ReferEngine.Common.Data
{
    public class ReferEngineDatabaseContext : DbContext
    {
        public ReferEngineDatabaseContext() : base("name=DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<App> Apps { get; set; }
        public DbSet<AppRecommendation> AppRecommendations { get; set; }
        public DbSet<AppReceipt> AppReceipts { get; set; }
        public DbSet<Developer> Developers { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<AppScreenshot> AppScreenshots { get; set; }
        public DbSet<PrivateBetaSignup> PrivateBetaSignups { get; set; }
        public DbSet<AppWebLink> AppWebLinks { get; set; }
        public DbSet<StoreAppInfo> StoreAppInfos { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Entity<App>().Property(a => a.AppStoreLink).IsRequired();
            mb.Entity<App>().Property(a => a.Description).IsRequired();
            mb.Entity<App>().Property(a => a.DeveloperId).IsRequired();
            mb.Entity<App>().Property(a => a.LogoLink50).IsRequired();
            mb.Entity<App>().Property(a => a.Name).IsRequired();
            mb.Entity<App>().Property(a => a.PackageFamilyName).IsRequired();
            mb.Entity<App>().Property(a => a.Publisher).IsRequired();
            mb.Entity<App>().Property(a => a.ShortDescription).IsRequired();

            mb.Entity<AppReceipt>().Property(r => r.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            mb.Entity<AppReceipt>().HasKey(r => r.Id);
            mb.Entity<AppReceipt>().Property(r => r.AppId).IsRequired();
            mb.Entity<AppReceipt>().Property(r => r.AppPackageFamilyName).IsRequired();
            mb.Entity<AppReceipt>().Property(r => r.LicenseType).IsRequired();
            mb.Entity<AppReceipt>().Property(r => r.PurchaseDate).IsRequired();

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
            mb.Entity<AppRecommendation>().Property(r => r.UserMessage).IsOptional();

            mb.Entity<AppScreenshot>().Property(s => s.Description).IsRequired();
            mb.Entity<AppScreenshot>().Property(s => s.Height).IsRequired();
            mb.Entity<AppScreenshot>().Property(s => s.Width).IsRequired();
            mb.Entity<AppScreenshot>().Property(s => s.Size).IsRequired();

            mb.Entity<PrivateBetaSignup>().HasKey(s => s.Email);
            mb.Entity<PrivateBetaSignup>().Property(s => s.Email)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<AppWebLink>().HasKey(l => l.Link);

            mb.Entity<StoreAppInfo>().HasKey(i => i.MsAppId);

            base.OnModelCreating(mb);
        }
    }
}