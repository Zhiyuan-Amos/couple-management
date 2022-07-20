using Couple.Client.Features.Done.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Couple.Client.Shared.Data;

public class DoneEntityTypeConfiguration : IEntityTypeConfiguration<DoneModel>
{
    public void Configure(EntityTypeBuilder<DoneModel> builder)
    {
        builder.HasKey(d => d.Date);
    }    
}
