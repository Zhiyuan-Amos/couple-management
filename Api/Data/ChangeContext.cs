using Couple.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Couple.Api.Data
{
    public class ChangeContext : DbContext
    {
        public DbSet<Change> Changes { get; set; }

        public ChangeContext(DbContextOptions<ChangeContext> options) : base(options) { }

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
