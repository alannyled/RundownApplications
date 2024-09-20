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
        public DbSet<Rundown> Rundowns { get; set; }
        public DbSet<TeleprompterTest> TeleprompterTests { get; set; }
        public DbSet<ControlRoom> ControlRooms { get; set; }
        public DbSet<Hardware> Hardwares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
            modelBuilder.Entity<Rundown>()
                .HasMany(t => t.VideoObjects)
                .WithMany(v => v.Rundowns)
                .UsingEntity(j => j.ToTable("RundownVideos"));

            modelBuilder.Entity<Rundown>()
                .HasOne(r => r.ControlRoom)
                .WithMany(c => c.Rundowns)
                .HasForeignKey(r => r.ControlRoomId);

            modelBuilder.Entity<ControlRoom>()
                .HasMany(t => t.Hardwares)
                .WithMany(v => v.ControlRooms)
                .UsingEntity(j => j.ToTable("ControlRoomHardwares"));

            base.OnModelCreating(modelBuilder);
        }
    }
}
