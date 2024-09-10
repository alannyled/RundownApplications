using Microsoft.EntityFrameworkCore;

namespace RundownDbService.Data
{
    public class RundownDbContext : DbContext
    {
        public RundownDbContext(DbContextOptions<RundownDbContext> options) : base(options)
        {
        }

        public DbSet<Template> Templates { get; set; } 
        public DbSet<Video> Videos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Many-to-many relation mellem Template og Video
            modelBuilder.Entity<Template>()
                .HasMany(t => t.VideoObjects)
                .WithMany(v => v.Templates)
                .UsingEntity(j => j.ToTable("TemplateVideos"));
        }
    }
}
