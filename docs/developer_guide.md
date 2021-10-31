# Developer Guide

## Setting Up for Local Development

### Initialization

1. Install [Visual Studio](https://visualstudio.microsoft.com/downloads/) for development.
1. Install [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash#v2) to run Azure Functions locally.
1. Install [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=cli%2Cssl-netstd21) to run Azure Cosmos DB locally.
1. Install [Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio) to run Azure Blob Storage locally.
    
    As the command to start Azurite is [rather lengthy](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio#run-azurite-from-a-command-line), consider adding an alias on PowerShell. To do so:

   1. Launch PowerShell and run `$profile` to print out the value of the `$profile` variable. 
   1. Create the file if it does not exist. 
   1. Add the following code:
      ```
      Function RunAzurite {azurite --silent --location c:\azurite --debug c:\azurite\debug.log
      Set-Alias -Name azr -Value RunAzurite
      ```

   Azurite can now be started by running `azr`.
1. Install [Azure Storage Explorer](https://azure.microsoft.com/en-us/features/storage-explorer/#overview) to view the contents of Azure Blob Storage on a GUI. Follow this [tutorial](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite?tabs=visual-studio#connect-to-azurite-using-http) to connect Azure Storage Explorer to Azurite.
1. Run `git config --local core.hooksPath .hooks` in the root directory to configure Git to run Git hooks in `.hooks`.

### Running the Application

1. Run each command in a new terminal instance:
    1. Run Blazor WASM
       ```
       cd Client
       dotnet run
       ```
    1. Run Azure Functions
       ```
       cd Api
       func start
       ```
    1. Run Azurite
       ```
       azr
       ```
1. Run `Azure Cosmos DB Emulator`.
1. Access the app at `https://localhost`.

## Tech Stack

### Frontend

1. Blazor WASM on .NET 6
1. Bootstrap
1. IndexedDb

### Backend

1. Azure Functions on .NET Core 3.1. Azure Functions does not support newer versions of .NET Core i.e. .NET 5 and .NET 6 Previews; it will only support .NET 6 in the future.

### Hosting

1. Azure Static Web App

### Persistence

1. Azure Cosmos DB
1. Azure Blob Storage

## Guidelines

1. State Management: Within the same page, use `Parameters` or `Cascading Parameters`. Across Pages, use `States` or `Query String Parameters`.
