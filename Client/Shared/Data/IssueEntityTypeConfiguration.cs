using System.Text.Json;
using Couple.Client.Features.Issue.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Couple.Client.Shared.Data;

public class IssueEntityTypeConfiguration : IEntityTypeConfiguration<IssueModel>
{
    public void Configure(EntityTypeBuilder<IssueModel> builder)
    {
        builder.HasKey(issue => issue.Id);

        builder
            .Property(i => i.Tasks)
            .HasConversion(
                t => JsonSerializer.Serialize(t, (JsonSerializerOptions)default!),
                t => JsonSerializer.Deserialize<List<TaskModel>>(t, (JsonSerializerOptions)default!)!,
                new ValueComparer<List<TaskModel>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));
    }    
}
