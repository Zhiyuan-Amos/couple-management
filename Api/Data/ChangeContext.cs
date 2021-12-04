using Couple.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Couple.Api.Data
{
    public class ChangeContext : DbContext
    {
        // DbSet of sub-classes of Change are required to persist sub-class specific information
        public DbSet<Change> Changes { get; set; }
        public DbSet<CachedChange> CachedChanges { get; set; }

        public ChangeContext(DbContextOptions<ChangeContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Change>()
                .ToContainer("change");

            modelBuilder.Entity<Change>()
                .Property(change => change.Id)
                .ToJsonProperty("id")
                .HasConversion<string>();

            modelBuilder.Entity<Change>()
                .Property(change => change.Ttl)
                .ToJsonProperty("ttl");
        }
    }
}
