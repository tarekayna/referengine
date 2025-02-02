﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity;
using AppSmarts.Common.Models;

namespace AppSmarts.Common.Data
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() { }

        public DatabaseContext(string connection) : base(connection)
        {
            this.Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<App> Apps { get; set; }
        public DbSet<AppRecommendation> AppRecommendations { get; set; }
        public DbSet<AppReceipt> AppReceipts { get; set; }
        public DbSet<RecommendationPageView> RecommendationPageViews { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PrivateBetaSignup> PrivateBetaSignups { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<OAuthMembership> OAuthMemberships { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UsersInRoles { get; set; }
        public DbSet<IpAddressLocation> IpAddressLocations { get; set; }
        public DbSet<AppAuthorization> AppAuthorizations { get; set; }
        public DbSet<AppAutoShowOptions> AppAutoShowOptions { get; set; }
        public DbSet<AppRewardPlan> AppRewardPlans { get; set; }
        public DbSet<FacebookPageView> FacebookPageViews { get; set; }
        public DbSet<Invite> Invites { get; set; }
        public DbSet<WindowsAppStoreLink> WindowsAppStoreLinks { get; set; }
        public DbSet<WindowsAppStoreInfo> WindowsAppStoreInfos { get; set; }
        public DbSet<WindowsAppStoreCategory> WindowsAppStoreCategories { get; set; }
        public DbSet<CloudinaryImage> CloudinaryImages { get; set; }
        public DbSet<AppScreenshot> AppScreenshots { get; set; }

        protected override void OnModelCreating(DbModelBuilder mb)
        {
            mb.Entity<App>().Property(a => a.AppStoreLink).IsRequired();
            mb.Entity<App>().Property(a => a.Description).IsRequired();
            mb.Entity<App>().Property(a => a.UserId).IsRequired();
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
            mb.Entity<AppReceipt>().Property(r => r.Timestamp).IsRequired();

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

            mb.Entity<PrivateBetaSignup>().HasKey(s => s.Email);
            mb.Entity<PrivateBetaSignup>().Property(s => s.Email)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<AppAuthorization>().HasKey(s => s.Token);
            mb.Entity<AppAuthorization>().Property(s => s.Token)
                        .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Entity<WindowsAppStoreLink>().HasKey(l => l.Link);

            mb.Entity<WindowsAppStoreInfo>().HasKey(i => i.MsAppId).HasMany(i => i.AppScreenshots);

            mb.Entity<Invite>().HasKey(i => i.Email);

            mb.Entity<AppAutoShowOptions>().HasKey(o => o.AppId);
            mb.Entity<AppAutoShowOptions>().Property(s => s.AppId).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            mb.Configurations.Add(new UserConfiguration());
            mb.Configurations.Add(new MembershipConfiguration());
            mb.Configurations.Add(new OAuthMembershipConfiguration());
            mb.Configurations.Add(new RoleConfiguration());
            mb.Configurations.Add(new UserInRoleConfiguration());
            mb.Configurations.Add(new IpAddressLocationConfiguration());

            base.OnModelCreating(mb);
        }

        public class UserConfiguration : EntityTypeConfiguration<User>
        {
            public UserConfiguration()
            {
                ToTable("Users");
                HasKey(u => u.Id);
                Property(u => u.Id)
                    .HasColumnName("Id")
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                    .IsRequired();
                Property(u => u.Email)
                    .HasColumnName("Email")
                    .IsRequired();
                Property(u => u.FirstName)
                    .HasColumnName("FirstName")
                    .IsRequired();
                Property(u => u.LastName)
                    .HasColumnName("LastName");
                Property(u => u.TimeStamp)
                    .HasColumnName("Timestamp")
                    .IsRequired();
            }
        }

        public class MembershipConfiguration : EntityTypeConfiguration<Membership>
        {
            public MembershipConfiguration()
            {
                ToTable("webpages_Membership");
                HasKey(u => u.UserId);
                Property(u => u.UserId)
                    .HasColumnName("UserId")
                    .IsRequired();
                Property(m => m.CreateDate)
                    .HasColumnName("CreateDate");
                Property(m => m.ConfirmationToken)
                    .HasColumnName("ConfirmationToken");
                Property(m => m.IsConfirmed)
                    .HasColumnName("IsConfirmed");
                Property(m => m.LastPasswordFailureDate)
                    .HasColumnName("LastPasswordFailureDate");
                Property(m => m.PasswordFailuresSinceLastSuccess)
                    .HasColumnName("PasswordFailuresSinceLastSuccess");
                Property(m => m.Password)
                    .HasColumnName("Password");
                Property(m => m.PasswordChangedDate)
                    .HasColumnName("PasswordChangedDate");
                Property(m => m.PasswordSalt)
                    .HasColumnName("PasswordSalt");
                Property(m => m.PasswordVerificationToken)
                    .HasColumnName("PasswordVerificationToken");
                Property(m => m.PasswordVerificationTokenExpirationDate)
                    .HasColumnName("PasswordVerificationTokenExpirationDate")
                    .IsOptional();
            }
        }

        public class OAuthMembershipConfiguration : EntityTypeConfiguration<OAuthMembership>
        {
            public OAuthMembershipConfiguration()
            {
                ToTable("webpages_OAuthMembership");
                HasKey(u => u.UserId);
                Property(u => u.UserId)
                    .HasColumnName("UserId")
                    .IsRequired();
                Property(m => m.Provider)
                    .HasColumnName("Provider")
                    .IsRequired();
                Property(m => m.ProviderUserId)
                    .HasColumnName("ProviderUserId")
                    .IsRequired();
            }
        }

        public class RoleConfiguration : EntityTypeConfiguration<Role>
        {
            public RoleConfiguration()
            {
                ToTable("webpages_Roles");
                HasKey(u => u.RoleId);
                Property(u => u.RoleId)
                    .HasColumnName("RoleId")
                    .IsRequired();
                Property(u => u.RoleName)
                    .HasColumnName("RoleName")
                    .IsRequired();
            }
        }

        public class UserInRoleConfiguration : EntityTypeConfiguration<UserInRole>
        {
            public UserInRoleConfiguration()
            {
                ToTable("webpages_UsersInRoles");
                HasKey(u => new { u.RoleId, u.UserId });
                Property(u => u.RoleId)
                    .HasColumnName("RoleId")
                    .IsRequired();
                Property(u => u.UserId)
                    .HasColumnName("UserId")
                    .IsRequired();
            }
        }

        public class IpAddressLocationConfiguration : EntityTypeConfiguration<IpAddressLocation>
        {
            public IpAddressLocationConfiguration()
            {
                HasKey(i => i.IpAddress);
                Property(i => i.IpAddress)
                    .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            }
        }
    }
}