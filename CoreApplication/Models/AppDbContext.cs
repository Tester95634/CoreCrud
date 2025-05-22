using Microsoft.EntityFrameworkCore;

namespace CoreApplication.Models
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<USERDATA> userdata { get; set; }
        public DbSet<tbl_Gender> tbl_Gender { get; set; }
    }
}
