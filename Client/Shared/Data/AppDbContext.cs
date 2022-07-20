using System.Reflection;
using Couple.Client.Features.Done.Models;
using Couple.Client.Features.Image.Models;
using Couple.Client.Features.Issue.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Couple.Client.Shared.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) =>
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

    public DbSet<IssueModel> Issues { get; set; } = default!;
    public DbSet<ImageModel> Images { get; set; } = default!;
    public DbSet<DoneIssueModel> DoneIssues { get; set; } = default!;
    public DbSet<DoneModel> Done { get; set; } = default!;

    public async Task<List<IDone>> GetIDone()
    {
        var images = (await Images.ToListAsync())
            .Cast<IDone>()
            .ToList();

        var doneIssues = (await DoneIssues.ToListAsync())
            .Cast<IDone>()
            .ToList();

        return images
            .Concat(doneIssues)
            .ToList();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        foreach (var created in ChangeTracker.Entries()
                     .Where(e => e.Entity is IDone && e.State is EntityState.Added)
                     .Select(e => (e.Entity as IDone)!)
                     .ToList())
        {
            await HandleCreate(created);
        }

        foreach (var updated in ChangeTracker.Entries()
                     .Where(e => e.Entity is IDone && e.State is EntityState.Modified)
                     .ToList())
        {
            await HandleUpdate(updated);
        }

        foreach (var deleted in ChangeTracker.Entries()
                     .Where(e => e.Entity is IDone && e.State is EntityState.Deleted)
                     .Select(e => (e.Entity as IDone)!)
                     .ToList())
        {
            await HandleDelete(deleted);
        }

        return await base.SaveChangesAsync(true, cancellationToken);

        async Task HandleCreate(IDone created)
        {
            int order;
            var done = await Done.FindAsync(created.DoneDate);
            if (done == null)
            {
                order = 0;
                done = new(created.DoneDate, 0, 1);
                Done.Add(done);
            }
            else
            {
                Attach(done);
                order = ++done.LargestOrder;
                done.Count++;
            }

            created.Order = order;
        }

        async Task HandleUpdate(EntityEntry entityEntry)
        {
            var updated = (entityEntry.Entity as IDone)!;

            var oldDate = (entityEntry.OriginalValues[updated.DoneDatePropertyName] as DateOnly?)!.Value;
            var newDate = (entityEntry.CurrentValues[updated.DoneDatePropertyName] as DateOnly?)!.Value;
            if (oldDate == newDate)
            {
                return;
            }

            var oldDone = (await Done
                .FindAsync(oldDate))!;
            if (oldDone.Count == 1)
            {
                Done.Remove(oldDone);
            }
            else
            {
                Attach(oldDone);
                oldDone.Count--;
            }

            var newDone = await Done
                .FindAsync(newDate);

            int order;
            if (newDone == null)
            {
                order = 0;
                newDone = new(newDate, 0, 1);
                Done.Add(newDone);
            }
            else
            {
                Attach(newDone);
                order = ++newDone.LargestOrder;
                newDone.Count++;
            }

            updated.Order = order;
        }

        async Task HandleDelete(IDone deleted)
        {
            var done = (await Done
                .FindAsync(deleted.DoneDate))!;
            if (done.Count == 1)
            {
                Done.Remove(done);
            }
            else
            {
                Attach(done);
                done.Count--;
            }
        }
    }
}
