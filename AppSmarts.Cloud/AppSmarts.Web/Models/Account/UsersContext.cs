using System.Data.Entity;
using AppSmarts.Common.Data;

namespace AppSmarts.Web.Models.Account
{
    public class UsersContext : DbContext
    {
        public UsersContext()
            : base(DbConnector.GetCurretConnectionStringName())
        {
        }

        public DbSet<UserProfile> UserProfiles { get; set; }
    }
}
