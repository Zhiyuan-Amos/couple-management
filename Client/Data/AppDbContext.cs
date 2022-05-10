using Couple.Client.Model.Done;
using Couple.Client.Model.Image;
using Couple.Client.Model.Issue;

namespace Couple.Client.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

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

        modelBuilder.Entity<IssueModel>()
            .HasKey(issue => issue.Id);

        modelBuilder.Entity<IssueModel>()
            .Property(i => i.Tasks)
            .HasConversion(
                t => JsonSerializer.Serialize(t, (JsonSerializerOptions)default!),
                t => JsonSerializer.Deserialize<List<TaskModel>>(t, (JsonSerializerOptions)default!)!,
                new ValueComparer<List<TaskModel>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        modelBuilder.Entity<ImageModel>()
            .HasKey(image => image.Id);

        modelBuilder.Entity<ImageModel>()
            .Property(image => image.TakenOnDate)
            .HasColumnName("TakenOnDate");

        modelBuilder.Entity<DoneIssueModel>()
            .HasKey(di => di.Id);

        modelBuilder.Entity<DoneIssueModel>()
            .Property(di => di.Tasks)
            .HasConversion(
                dt => JsonSerializer.Serialize(dt, (JsonSerializerOptions)default!),
                dt => JsonSerializer.Deserialize<List<DoneTaskModel>>(dt, (JsonSerializerOptions)default!)!,
                new ValueComparer<List<DoneTaskModel>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

        modelBuilder.Entity<DoneModel>()
            .HasKey(d => d.Date);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default)
    {
        foreach (var entityEntry in ChangeTracker.Entries()
                     .Where(e => e.Entity is IDone && e.State is EntityState.Added)
                     .ToList())
        {
            var iDone = (entityEntry.Entity as IDone)!;
            int order;
            var done = await Done.FindAsync(iDone.DoneDate);
            if (done == null)
            {
                order = 0;
                done = new(iDone.DoneDate, 0, 1);
                Done.Add(done);
            }
            else
            {
                Attach(done);
                order = ++done.LargestOrder;
                done.Count++;
            }

            iDone.Order = order;
        }

        foreach (var entityEntry in ChangeTracker.Entries()
                     .Where(e => e.Entity is IDone && e.State is EntityState.Modified)
                     .ToList())
        {
            var iDone = (entityEntry.Entity as IDone)!;

            var oldDate = (entityEntry.OriginalValues[iDone.DoneDatePropertyName] as DateOnly?)!.Value;
            var newDate = (entityEntry.CurrentValues[iDone.DoneDatePropertyName] as DateOnly?)!.Value;
            if (oldDate == newDate) continue;

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

            iDone.Order = order;
        }

        foreach (var iDone in ChangeTracker.Entries()
                     .Where(e => e.Entity is IDone && e.State is EntityState.Deleted)
                     .Select(e => e.Entity as IDone)
                     .ToList())
        {
            var done = (await Done
                .FindAsync(iDone!.DoneDate))!;
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

        return await base.SaveChangesAsync(true, cancellationToken);
    }
}