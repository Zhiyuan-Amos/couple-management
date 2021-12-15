using Couple.Api.Model;
using Microsoft.EntityFrameworkCore;

namespace Couple.Api.Data;

public class ImageContext : DbContext
{
    public ImageContext(DbContextOptions<ImageContext> options) : base(options)
    {
    }

    public DbSet<Image> Images { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Image>()
            .ToContainer("image")
            .HasNoDiscriminator();

        modelBuilder.Entity<Image>()
            .Property(image => image.Id)
            .ToJsonProperty("id")
            .HasConversion<string>();
    }
}
