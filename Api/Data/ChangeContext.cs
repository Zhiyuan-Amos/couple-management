using Couple.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Couple.Api.Data;

public class ChangeContext : DbContext
{
    public ChangeContext(DbContextOptions<ChangeContext> options) : base(options) { }

    // DbSet of sub-classes of Change are required to persist sub-class specific information
    public DbSet<Change> Changes { get; set; } = default!;
    public DbSet<CachedChange> CachedChanges { get; set; } = default!;
    public DbSet<HyperlinkChange> HyperlinkChanges { get; set; } = default!;

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
