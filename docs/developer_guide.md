# Developer Guide

## Setting Up for Local Development

### Initialization

1. Install [Visual Studio](https://visualstudio.microsoft.com/downloads/) and [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0) for development.
1. Install [Azure Functions Core Tools v4.x](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash#v2) to run Azure Functions locally.
1. Install [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=cli%2Cssl-netstd21) to run Azure Cosmos DB locally.
1. Install [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio) to run Azure Blob Storage locally.
    
    As the command to start Azurite is rather lengthy, consider adding an alias on PowerShell. To do so:

   1. Launch PowerShell and run `$profile` to print out the value of the `$profile` variable. 
   1. Create the file if it does not exist. 
   1. Add the following code:
      ```
      Function RunAzurite {azurite --silent --location c:\azurite --debug c:\azurite\debug.log}
      Set-Alias -Name azr -Value RunAzurite
      ```

   Azurite can now be started by running `azr`.
1. Install [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/#overview) to view the contents of Azure Blob Storage on a GUI. Follow this [tutorial](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio#connect-to-azurite-using-http) to connect Azure Storage Explorer to Azurite.
1. Install [CleanupCode](https://www.jetbrains.com/help/rider/CleanupCode.html) to perform code cleanup based on `.editorconfig` by running `dotnet tool install -g JetBrains.ReSharper.GlobalTools`. Compared to [dotnet format](https://github.com/dotnet/format), `CleanupCode` formats additional file types, such as `razor`, `html`, `css` and `js`.
1. Install [npm](https://nodejs.org/en/download/) and run `npm install` to install `husky` and `lint-staged`. These dependencies are required to automatically run `CleanupCode` before each commit on staged code.
1. Install .NET WebAssembly Build Tools by running `dotnet workload install wasm-tools` to allow the Blazor WebAssembly application to use native dependencies such as `SQLite`.
1. Create the relevant database(s) & container(s) using `Azure Cosmos DB Emulator`, and create the relevant Blob Container(s) using `Azure Storage Explorer`.

### Running the Application

1. Run each command in a new terminal instance:
    1. Run Blazor WASM
       ```
       cd Client
       dotnet run
       ```
    1. Run Azure Functions (Api)
       ```
       cd Api
       func start
       ```
    1. Run Azurite
       ```
       azr
       ```
    1. Run Azure Functions (Messaging)
       ```
       cd Messaging
       func start -p 7072
       ```
1. Run `Azure Cosmos DB Emulator`.
1. Access the app at `https://localhost`.

### Guides

1. [Debugging Azure Function Event Grid Triggers Locally](https://harrybellamy.com/posts/debugging-azure-function-event-grid-triggers-locally/)

### Caveats

1. Some of the cloud resources used by this application, such as Azure Event Grid, cannot be run locally / external to Azure.

## Tech Stack

### Frontend

1. Blazor WASM on .NET 6
1. Bootstrap
1. Sqlite

### Backend

1. Azure Functions

### Hosting

1. Azure Static Web App

### Messaging

1. Azure Event Grid

### Persistence

1. Azure Cosmos DB
1. Azure Blob Storage

## Guidelines & Caveats

1. State Management: Within the same page, use `Parameters` or `Cascading Parameters`. Across Pages, use `States` or `Query String Parameters`.
1. Deserialization:
    1. Reading data from EF Core minimally requires properties to have init-only setter & private parameterless constructor
    1. Deserializing through JS interop minimally requires properties to have init-only setter & public parameterless constructor
    1. `List` cannot be deserialized into `IEnumerable`
1. It's unsure why adding custom Json Converters in Program.cs https://github.com/dotnet/runtime/issues/53539#issuecomment-970051936 don't work, so the properties are annotated with the attribute instead.
1. Properties tracked for changes require `set`, while properties to map in the database column require either `set` or `init`.
