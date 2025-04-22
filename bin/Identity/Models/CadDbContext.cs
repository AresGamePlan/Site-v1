using Microsoft.EntityFrameworkCore;

namespace Identity.Models
{
    public class CadDbContext : DbContext
    {
        public CadDbContext(DbContextOptions<CadDbContext> options) : base(options) { }
        public CadDbContext()
        {
        }

        public DbSet<CADFile> CADFiles { get; set; }
    }
}
