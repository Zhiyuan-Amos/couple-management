using Couple.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace Api.Data
{
    public class UserContext : DbContext
    {
        public DbSet<Pair> Pairs { get; set; }

        public UserContext(DbContextOptions<UserContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pair>()
                .ToContainer("pair")
                .HasPartitionKey(pair => pair.EmailOne)
                .HasNoDiscriminator()
                .HasKey(pair => pair.UserIdOne);

            modelBuilder.Entity<Pair>()
                .Property(pair => pair.UserIdOne)
                .ToJsonProperty("id");
        }
    }
}