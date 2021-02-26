# Memories

The main purpose of this application is to allow couples to easily manage their schedules and review shared memories. As such, the application revolves around the calendar, where you can:

1. Upload photos to events and subsequently view them using infinite scrolling.
1. Create to dos and assign them to events. To dos are different from events because they are meant to be things that you want to do, but have yet to schedule it.
1. Create goals and assign them to events, and subsequently view the events that you have completed for each goal.

## Developer Guide

### Set Up

1. Download [Visual Studio 2019](https://visualstudio.microsoft.com/downloads/) for development.
1. Download [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash#v2) to run Azure Functions locally.
1. Download [Azure Cosmos DB Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator?tabs=cli%2Cssl-netstd21) to run Azure Cosmos DB locally.
1. As [Cosmos DB does not support migrations](https://github.com/dotnet/efcore/issues/13200), database & containers cannot be created using command line. There are 2 ways to create them:
    1. Uncomment `[FunctionName("DatabaseInitializerFunction")]` in `DatabaseInitializerFunction` and make a HTTP request to that endpoint (`http://localhost:7071/api/DatabaseInitializer`). Ideally, this code should have been placed at `Startup.cs` so that the database & containers are created when starting Azure Functions, but [that's not possible](https://docs.microsoft.com/en-us/azure/azure-functions/functions-dotnet-dependency-injection#caveats), so the next best trigger is through a HTTP request. Do not commit the uncommented line as that should not be used in production.
    1. Manually create the database based on `local.settings.json > DatabaseName` & containers based on the `DbContexts` in `Api > Data`.

### Running the Application

1. Run Azure Functions by navigating to `Api` folder and run `func start`.
1. Run Blazor WASM application by navigating to `Client` folder and run `dotnet run`.
1. Ensure that `Azure Cosmos DB Emulator` is running.
1. Access the app at `https://localhost`.

### Tech Stack

Application is hosted on Azure Static Web App.

#### Client

1. Blazor WASM on .NET 5
1. Telerik
1. Bootstrap
1. IndexedDb (see [Privacy](#Privacy))

The application has State Management (using `Blazor-State`) to reduce the number of times the Client needs to go to the Database for data.

#### Api

1. Azure Functions on .NET 3.1

#### Authentication & Authorisation

1. Azure Active Directory

#### Database

1. CosmosDB, because it has a free tier available

#### FAQ

Why are we using a Web application rather than an App?

1. A single code base can support both Android & iOS
1. The same code base can be used to support larger screens if required
1. End-to-end solution provided in the form of Azure Static Web App
1. Hosting on Azure Static Web App is free

The main con however, is that there's no native notifications. This can be circumvented by integrating with perhaps a chat app such as Telegram, though it's not optimal.

### Design Decisions

#### Client

##### Telerik Controls

1. Use `Visible` parameter rather than `@if (IsVisible)`.
1. Customise Telerik controls directly, rather than customising the wrapper divs around the Telerik controls because they still do not allow for complete customisation.
1. Customise Telerik controls by modifying the css classes, rather than using `Class` parameter because it still does not allow for complete customisation as it might not target the inner components. Only do so when having [multiple controls of the same type in the same component](https://www.telerik.com/forums/reliability-of-overriding-kendo-css-classes).
1. Calendar control is chosen over Scheduler due to crucial missing elements:
    1. On cell click doesn't go to day view so there's no way to view all the events on that particular day, and there's no onclick event to trigger the loading of a separate ListView.
    1. Header overflows because there's no abbreviation (i.e. Monday -> Mon).

##### General

1. Data binding vs getters: As parent components sometimes require re-render on data changed e.g. to determine form validity, let's standardise with data binding as it automatically re-renders client.
1. State Management: Within the same page, use `Parameters` or `Cascading Parameters`. Across Pages, use `States` or `Query String Parameters`.

#### Privacy

There's no way to protect the data from myself if it's stored inside the database, as I'm the only developer; there's no one else to audit me. Therefore, data is stored locally, and it is only stored in database temporarily until the other party synchronises the data.

## Known Limitations

1. Requires online.
