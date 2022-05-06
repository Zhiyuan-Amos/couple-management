# 7. Replace IndexedDB with SQLite

Date: 2022-05-05

## Status

Accepted

## Context

IndexedDB is currently used to store data on Client. The Blazor application uses JSInterop (and therefore, JavaScript) is to interact with the database. .NET 6 has a [new capability](https://docs.microsoft.com/en-us/aspnet/core/blazor/webassembly-native-dependencies?view=aspnetcore-6.0#net-webassembly-build-tools) to

> Add native dependencies to a Blazor WebAssembly app by adding NativeFileReference items in the app's project file. When the project is built, each NativeFileReference is passed to Emscripten by the .NET WebAssembly build tools so that they are compiled and linked into the runtime.

SQLite is one of the dependencies that can be referenced by the Blazor application. EF Core is also compatible with SQLite.

## Decision

Comparing the JSInterop & SQLite approaches, the JSInterop approach fares worse because:

1. I am not familiar with JavaScript nor IndexedDB; unsure if the code I'm writing is error-free.
1. Low-level APIs are used to interface with IndexedDB; more code has to be written.
1. Import & Export logic has to be manually written; SQLite provides the Import & Export functionality.
1. Methods that need to perform common logic have to manually call a helper method; EF Core allows overriding of [SaveChangesAsync](https://github.com/Zhiyuan-Amos/couple-management/blob/b752e491d4d852063b4ad2df88ccb9b7423e80cb/Client/Data/AppDbContext.cs#L77) so that methods do not have to manually call the helper method.

Therefore, replace IndexedDB with SQLite.

## Consequences

1. Code is less error-prone and easier to write.
1. Much effort is required to re-write Client-side persistence.

## Affected ADRs

1. [Image Persistence: Optimize Storage Space](0003-image_persistence_optimize_storage_space.md)
1. [Modeling Issues & Tasks on Client-side Database](0004-modeling_issues_and_tasks_database.md)
