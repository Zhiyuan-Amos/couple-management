- [Guidelines & Caveats](#guidelines--caveats)
  - [Authentication & Authorization](#authentication--authorization)
  - [State Management](#state-management)
  - [Deserialization](#deserialization)
  - [SQLite](#sqlite)
  - [Miscellaneous](#miscellaneous)

# Guidelines & Caveats

This document provides a list of guidelines & caveats to be aware of when working on this codebase, and several minor design decisions.

## Authentication & Authorization

1. Unlike a .NET Core Web API application, endpoints in a Function App cannot be protected by different scope requirements.

1. Login is disabled for Development environment, as it is mainly not required in the development of the application because there's no expected changes to AuthN or AuthZ in the near future. If testing logging in is subsequently required, explore creating a new tenant.

## State Management

Within the same page, use `Parameters` or `Cascading Parameters`. Across Pages, use `States` or `Query String Parameters`.

## Deserialization

1. Reading data from EF Core minimally requires properties to have init-only setter & private parameterless constructor or private parameterized constructor with parameter names and types that match those of mapped properties

1. Writing data to EF Core minimally requires properties to have a public getter

1. Deserializing JSON minimally requires properties to have init-only setter & public parameterless constructor. If any of the property's accessors (`get` or `set`) are non-public, the property has to be annotated with the `[JsonInclude]` attribute

1. `List` cannot be deserialized into `IEnumerable`

## SQLite

1. Using integer as PK for entities allow for more efficient database operations, but that is not possible as this app requires [syncing between databases with occasionally connected apps](https://stackoverflow.com/a/404057/8828382).

1. One possible way to design `Done*` is: 

    ```mermaid
    classDiagram
    IDone <|-- DoneImage
    IDone <|-- DoneIssueIssue
    <<interface>> IDone
    DoneImage --> Image
    DoneIssueIssue --> DoneIssue
    DoneImage --> Done
    DoneIssueIssue --> Done

    IDone : Guid DoneId
    DoneImage : Guid ImageId
    DoneIssueIssue : Guid DoneIssueId
    Done : Guid Id
    Done : DateOnly DoneDate
    Done : int Order
    ```

    Join entities (`DoneImage`, `DoneIssueIssue`) are used to decouple the database entities (`Image`, `DoneIssue`) from the relationship with `Done`. However, this implementation is slow as it uses multiple joins; retrieving even a small amount of data from the database takes about 1050ms, while the denormalized version only takes 650ms.

1. The denormalized implementation (using JSON) teaches `Image` and `DoneIssue` to implement `IDone`. Apart from having better performance, the code is also simpler to read as there's lesser joins.

1. For performance reasons (joins are expensive computationally), `Image` and `Task`, `DoneImage` and `DoneTask` are persisted in a denormalized manner (using JSON) as well. Both `Task` & `DoneTask` overrides `Equals` and `GetHashCode` to determine if the values have been [modified](https://github.com/Zhiyuan-Amos/couple-management/blob/master/Client/Data/AppDbContext.cs#L48-L51).

1. Blazor Wasm doesn't seem to support migrations yet, so manual migration is performed by loading the new database, attaching the existing database, and executing SQL code like:

    ```sqlite
    INSERT INTO Done(Date, LargestOrder, Count) SELECT Date, LargestOrder, Count FROM app.Done;
    INSERT INTO Images(Id, TakenOnDate, "Order", TakenOn, Data, IsFavourite) SELECT Id, TakenOnDate, "Order", TakenOn, Data, IsFavourite FROM app.Images;
    INSERT INTO Issues(Id, Tasks, Title, "For", CreatedOn) SELECT Id, Tasks, Title, "For", CreatedOn FROM app.Issues;
    INSERT INTO DoneIssues(Id, Tasks, "Order", DoneDate, Title, "For") SELECT Id, Tasks, "Order", DoneDate, Title, "For" FROM app.DoneIssues;
    ```

## Miscellaneous

1. It's unsure why adding custom Json Converters in Program.cs https://github.com/dotnet/runtime/issues/53539#issuecomment-970051936 don't work, so the properties are annotated with the attribute instead.

1. `Model` classes are usually named without having `Model` as the suffix, unlike other types of classes such as `ViewModel` and `State`. However, the Pages and the Controllers are named without suffixes as well; a Page and its Controller has to have the same names so that the Controller could use the `partial` keyword to indicate that it is a code-behind file of that Page.

1. `Messaging` project runs `in-process` rather than `out-of-process`. This is because `Messaging` uses non-HTTP Triggers which are not well-documented for running `out-of-process`.

1. Associated constant values of enum members should start from 1 instead of 0 if the enum does not have the concept of a default value. As C# sets the enum's value to 0 by default (i.e. value is not supplied), it's intuitive to not have an enum member that maps to 0, unless that enum member is really, a default value. 
