using System.Text.Json;
using Couple.Client.Features.Done.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Couple.Client.Shared.Data;

public class DoneIssueEntityTypeConfiguration : IEntityTypeConfiguration<DoneIssueModel>
{
    public void Configure(EntityTypeBuilder<DoneIssueModel> builder)
    {
        builder.HasKey(di => di.Id);

        builder
            .Property(di => di.Tasks)
            .HasConversion(
                dt => JsonSerializer.Serialize(dt, (JsonSerializerOptions)default!),
                dt => JsonSerializer.Deserialize<List<DoneTaskModel>>(dt, (JsonSerializerOptions)default!)!,
                new ValueComparer<List<DoneTaskModel>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
    }    
}
