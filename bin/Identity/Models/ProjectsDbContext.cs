using Microsoft.EntityFrameworkCore;

namespace Identity.Models
{
    public class ProjectsDbContext: DbContext
    {
        public ProjectsDbContext(DbContextOptions<ProjectsDbContext> options) : base(options) { }
        public ProjectsDbContext() { }
        public DbSet<Project> Projects { get; set; }
    }
}
