using Couple.Api.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace Couple.Api.Shared.Data;

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

        modelBuilder.Entity<Change>(builder =>
        {
            builder.ToContainer("change");
            
            builder.Property(change => change.Id)
                .ToJsonProperty("id")
                .HasConversion<string>();
            
            builder.Property(change => change.Ttl)
                .ToJsonProperty("ttl");
        });
    }
}
