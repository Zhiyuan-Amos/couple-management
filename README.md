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

#### Client

1. Blazor WASM on .NET 6
1. Bootstrap
1. IndexedDb

#### Api

1. Azure Functions on .NET Core 3.1. Azure Functions does not support newer versions of .NET Core i.e. .NET 5 and .NET 6 Previews; it will only support .NET 6 in the future.

#### Hosting

1. Azure Static Web App

#### Database

1. Azure Cosmos DB
