using AgentRest.Models;

using Microsoft.EntityFrameworkCore;

namespace AgentsRest.Date
{
    public class ApplicationDbContext(IConfiguration configuration) : DbContext
    {
        private readonly IConfiguration _configuration = configuration;


        public DbSet<AgentModel> Agents { get; set; }
        public DbSet<TargetModel> Targets { get; set; }
        public DbSet<MissionModel> Missions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"));
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AgentModel>()
                .Property(x => x.StatusAgent)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<TargetModel>()
                .Property(x => x.StatusTarget)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<MissionModel>()
                .Property(x => x.StatusMission)
                .HasConversion<string>()
                .IsRequired();

            modelBuilder.Entity<MissionModel>()
                .HasOne(x => x.Agent)
                .WithMany()
                .HasForeignKey(x => x.AgentID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MissionModel>()
                .HasOne(x => x.Target)
                .WithMany()
                .HasForeignKey(x => x.TargetId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);

        }
    }
}