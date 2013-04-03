using System.Data.Entity;
using ReferEngine.Common.Data;

namespace ReferEngine.Web.Models.Account
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
