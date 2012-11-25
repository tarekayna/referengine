using ReferLib;
using System.Data.Entity;

namespace ReferEngine.Models
{
    public class ReferDb : DbContext
    {
        public ReferDb()
            : base("DefaultConnection") { }

        public DbSet<Developer> Developers { get; set; }
        public DbSet<App> Apps { get; set; }
    }
}