using Couple.Client.Features.Image.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Couple.Client.Shared.Data;

public class ImageEntityTypeConfiguration : IEntityTypeConfiguration<ImageModel>
{
    public void Configure(EntityTypeBuilder<ImageModel> builder)
    {
        builder.HasKey(image => image.Id);

        builder
            .Property(image => image.TakenOnDate)
            .HasColumnName("TakenOnDate");
    }    
}
