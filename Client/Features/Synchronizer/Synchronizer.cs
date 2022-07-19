using System.Net.Http.Json;
using Couple.Client.Features.Issue.States;
using Couple.Client.Shared.Extensions;
using Couple.Shared.Models.Change;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Features.Synchronizer;

public class Synchronizer
{
    private readonly DbContextProvider _dbContextProvider;

    private readonly HttpClient _httpClient;

    private readonly IssueStateContainer _issueStateContainer;

    public Synchronizer(HttpClient httpClient,
        IssueStateContainer issueStateContainer,
        DbContextProvider dbContextProvider)
    {
        _httpClient = httpClient;
        _issueStateContainer = issueStateContainer;
        _dbContextProvider = dbContextProvider;
    }

    public async Task SynchronizeAsync()
    {
        var toSynchronize = (await _httpClient.GetFromJsonAsync<List<ChangeDto>>("api/Change"))!;
        var parser = new CommandParser();

        foreach (var change in toSynchronize)
        {
            // This implementation creates a new DbContext per change. An alternative implementation is to
            // call db.ChangeTracker.Clear(), but it's slower. Synchronizing 10 images & 1 issue with 4 tasks
            // take about 3 seconds, while the former implementation only takes 1.5 seconds. This implementation
            // is also preferred as based on https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.changetracking.changetracker.clear?view=efcore-6.0
            // "DbContext is designed to have a short lifetime where a new instance is created for each unit-of-work."
            await using var db = await _dbContextProvider.GetPreparedDbContextAsync();
            var command = parser.Parse(change, db);
            await command.Execute();
        }

        var idsToDelete = new DeleteChangesDto(toSynchronize
            .Select(change => change.Id)
            .ToList());

        if (idsToDelete.Guids.Any())
        {
            await _httpClient.DeleteAsJsonAsync("api/Change", idsToDelete);
        }

        await using var anotherDb = await _dbContextProvider.GetPreparedDbContextAsync();
        _issueStateContainer.Issues = await anotherDb.Issues
            .ToListAsync();
    }
}
