using Couple.Client.Features.Calendar.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Couple.Client.Shared.Data;

public class EventEntityTypeConfiguration : IEntityTypeConfiguration<EventModel>
{
    public void Configure(EntityTypeBuilder<EventModel> builder)
    {
        builder.HasKey(@event => @event.Id);
    }    
}
