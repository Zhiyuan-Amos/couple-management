﻿using System.Runtime.InteropServices;
using Couple.Client.Shared;
using Couple.Client.Shared.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace Couple.Client.Features.Synchronizer;

public class DbContextProvider
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;
    private readonly Task _firstTimeSetupTask;

    public DbContextProvider(IJSRuntime js, IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
        _firstTimeSetupTask = FirstTimeSetupAsync(js);
    }

    public async Task<AppDbContext> GetPreparedDbContextAsync()
    {
        await _firstTimeSetupTask;
        return await _dbContextFactory.CreateDbContextAsync();
    }

    private async Task FirstTimeSetupAsync(IJSRuntime js)
    {
        var module = await js.InvokeAsync<IJSObjectReference>("import", "./js/dbstorage.js");

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("browser")))
        {
            await module.InvokeVoidAsync("synchronizeFileWithIndexedDb", Constants.DatabaseFileName);
        }

        await using var db = await _dbContextFactory.CreateDbContextAsync();
        await db.Database.EnsureCreatedAsync();
    }
}
