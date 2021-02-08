using Couple.Shared.Model;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class EventContext : DbContext
    {
        public DbSet<Change> Changes { get; set; }

        public EventContext(DbContextOptions<EventContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Change>()
                .ToContainer("change")
                .HasPartitionKey(change => change.UserId)
                .HasNoDiscriminator();

            modelBuilder.Entity<Change>()
                .Property(change => change.Id)
                .ToJsonProperty("id")
                .HasConversion<string>();
        }
    }
}