using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
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
        public DbSet<User> Users { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserInRole> UsersInRoles { get; set; }

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

            mb.Configurations.Add(new UserConfiguration());
            mb.Configurations.Add(new MembershipConfiguration());
            mb.Configurations.Add(new RoleConfiguration());
            mb.Configurations.Add(new UserInRoleConfiguration());

            base.OnModelCreating(mb);
        }

        public class UserConfiguration : EntityTypeConfiguration<User>
        {
            public UserConfiguration()
                : base()
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
                : base()
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
    }
}