using Microsoft.EntityFrameworkCore;

namespace RundownDbService.Data
{
    public class RundownDbContext : DbContext
    {
        public RundownDbContext(DbContextOptions<RundownDbContext> options) : base(options)
        {
        }

        public DbSet<Template> Templates { get; set; } // Repræsenterer en tabel i databasen
    }
}
