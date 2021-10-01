# Memories

The main purpose of this application is to allow couples to easily manage their schedules and review shared memories.

## Developer Guide

### Set Up

1. Download [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) for development.
2. Download [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash#v2) to run Azure Functions locally.
3. Download [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=cli%2Cssl-netstd21) to run Azure Cosmos DB locally.

    [Cosmos DB does not support migrations](https://github.com/dotnet/efcore/issues/13200), database & containers cannot be created using command line. The alternative method of doing so is to uncomment `[FunctionName("DatabaseInitializerFunction")]` in `DatabaseInitializerFunction` and make a HTTP request to that endpoint (`http://localhost:7071/api/DatabaseInitializer`). Do not commit the uncommented line as that should not be used in production.

    Ideally, this code should have been placed in `Startup.cs` so that the database & containers are created when starting Azure Functions, but [that's not possible](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection#caveats).
4. Download [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio) to run Azure Blob Storage locally.
    
    As the command to start Azurite is [rather lengthy](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio#run-azurite-from-a-command-line), consider adding an alias on PowerShell. To do so:

   1. Launch PowerShell and run `$profile` to print out the value of the `$profile` variable. 
   2. Create the file if it does not exist. 
   3. Add the following code:
      ```
      Function RunAzurite {azurite --silent --location c:\azurite --debug c:\azurite\debug.log
      Set-Alias -Name azr -Value RunAzurite
      ```

   Azurite can now be started by running `azr`.
6. Download [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/#overview) and follow this [tutorial](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio#connect-to-azurite-using-http) to connect to Azurite.

### Running the Application

1. Run Azure Functions by navigating to `Api` folder and run `func start`.
1. Run Blazor WASM application by navigating to `Client` folder and run `dotnet run`.
1. Ensure that `Azure Cosmos DB Emulator` is running.
1. Access the app at `https://localhost`.

### Tech Stack

Application is hosted on Azure Static Web App.

#### Client

1. Blazor WASM on .NET 6
1. Bootstrap
1. IndexedDb

#### Api

1. Azure Functions on .NET Core 3.1. Azure Functions does not support newer versions of .NET Core i.e. .NET 5 and .NET 6 Previews; it will only support .NET 6 in the future.

#### Authentication & Authorisation

1. Azure Active Directory, automatically configured by Azure Static Web App.

### Design Decisions

(To be migrated to ADR)

#### Client

##### General

1. State Management: Within the same page, use `Parameters` or `Cascading Parameters`. Across Pages, use `States` or `Query String Parameters`.
1. Client is a Web application rather than a Mobile application as:
    * End-to-end solution (including authentication) provided by Azure Static Web App
    * Free hosting on Azure Static Web App

    The only con is that [iOS devices](https://stackoverflow.com/a/64576541/8828382) do not support push notifications and background synchronization. However, this is not a big issue.

##### Performance

Telerik & AutoMapper are no longer used as these dependencies [increase initial load time](https://zhiyuan-amos.github.io/blog/2021-06-27-dependencies-and-application-startup-time/).

##### State Management Containers

Custom State Containers are implemented rather than using the only existing libraries [Blazor-State](https://github.com/TimeWarpEngineering/blazor-state) & [Fluxor](https://github.com/mrpmorris/Fluxor) as:

1. These libraries aren't in active development.
1. [MSDN](https://docs.microsoft.com/en-us/aspnet/core/blazor/state-management?pivots=webassembly&view=aspnetcore-5.0#in-memory-state-container-service-wasm) demonstrates how easy it is to implement custom State Containers.
1. There's no real need to use an existing library yet.

##### Persistence

1. App allows CRUD while offline (only synchronizes when online), so Client-side Persistence is required.
1. Client-side Persistence also resolves potential [privacy concerns](#Privacy) that users might have.
1. IndexedDb is used as it is the only Web-based Database available.

#### Api

##### Architecture

1. The Api project is implemented with references to Vertical Slice Architecture for simplicity, compared to Clean Architecture which has multiple layers and thus there's quite a bit of boilerplate code to implement with unclear benefits.
1. Mediatr (or any form of Command Handler) is not used as the benefits of using it is unclear.

##### Middleware

Azure Functions has [yet to implement](https://github.com/Azure/azure-functions-dotnet-worker/issues/340) short circuiting a HTTP Request in the middleware by modifying the HTTP Response. Therefore, there's some duplicate logic across all the `{Function}.cs` classes.

##### Database

CosmosDB as the application only stores Changes / Events which are unstructured data. Also, it has a free-tier available compared to SQL Databases.

#### Privacy

There's no way to protect the data from myself if it's stored inside the database, as I'm the only developer; there's no one else to audit me. Therefore, data is stored locally, and it is only stored in database temporarily until the other party synchronises the data.

Code is open-sourced so users can verify that no sensitive information is being logged.

## Known Limitations

1. Requires Internet connection.
