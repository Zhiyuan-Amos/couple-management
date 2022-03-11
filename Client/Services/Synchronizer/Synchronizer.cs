using System.Net.Http.Json;
using Couple.Client.Infrastructure;
using Couple.Client.States.Issue;
using Couple.Shared.Model.Change;
using Microsoft.EntityFrameworkCore;

namespace Couple.Client.Services.Synchronizer;

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
        var toSynchronize = await _httpClient.GetFromJsonAsync<List<ChangeDto>>("api/Synchronize");
        var parser = new CommandParser();

        foreach (var change in toSynchronize)
        {
            await using var db = await _dbContextProvider.GetPreparedDbContextAsync();
            var command = parser.Parse(change, db);
            await command.Execute();
        }

        var idsToDelete = new DeleteChangesDto(toSynchronize
            .Select(change => change.Id)
            .ToList());

        if (idsToDelete.Guids.Any())
        {
            await _httpClient.DeleteAsJsonAsync("api/Changes", idsToDelete);
        }

        await using var anotherDb = await _dbContextProvider.GetPreparedDbContextAsync();
        _issueStateContainer.Issues = await anotherDb.Issues
            .ToListAsync();
    }
}
