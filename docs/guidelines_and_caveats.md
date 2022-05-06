- [Guidelines & Caveats](#guidelines--caveats)
  - [State Management](#state-management)
  - [Deserialization](#deserialization)
  - [SQLite](#sqlite)
  - [Miscellaneous](#miscellaneous)

# Guidelines & Caveats

This document provides a list of guidelines & caveats to be aware of when working on this codebase, and several minor design decisions.

## State Management

Within the same page, use `Parameters` or `Cascading Parameters`. Across Pages, use `States` or `Query String Parameters`.

## Deserialization

1. Reading data from EF Core minimally requires properties to have init-only setter & private parameterless constructor or private parameterized constructor with parameter names and types that match those of mapped properties

1. Deserializing through JSInterop minimally requires properties to have init-only setter & public parameterless constructor

1. `List` cannot be deserialized into `IEnumerable`

## SQLite

1. Using integer as PK for entities allow for more efficient database operations, but that is not possible as this app requires [syncing between databases with occasionally connected apps](https://stackoverflow.com/a/404057/8828382).

1. One possible way to design `Done*` is to create an abstract class `IDone` with implementing classes `DoneImage` & `DoneIssueIssue`. These classes are [join entity types](https://docs.microsoft.com/en-us/ef/core/modeling/relationships?tabs=fluent-api%2Cfluent-api-simple-key%2Csimple-key#many-to-many), referencing `Image` and `DoneIssue` correspondingly, and referencing `Done`.

    However, this implementation is slow as it uses multiple joins; retrieving negligible data from the database takes about 1050ms, while the denormalized version only takes 650ms.

1. The denormalized implementation (using JSON) teaches `Image` and `DoneIssue` to implement `IDone`. Apart from having better performance, the code is also simpler to read as there's lesser joins.

1. For performance reasons, `Image` and `Task`, `DoneImage` and `DoneTask` are persisted in a denormalized manner (using JSON) as well. Both `Task` & `DoneTask` overrides `Equals` and `GetHashCode` to determine if the values have been [modified](https://github.com/Zhiyuan-Amos/couple-management/blob/master/Client/Data/AppDbContext.cs#L48-L51).

## Miscellaneous

1. It's unsure why adding custom Json Converters in Program.cs https://github.com/dotnet/runtime/issues/53539#issuecomment-970051936 don't work, so the properties are annotated with the attribute instead.

1. The command `jb-cleanupcode` in `task-runner.json` doesn't look clean because of how `husky` populates the pre-defined variables such as `${staged}`. `${staged}` is a string consisting of file names, delimited by a space. As PowerShell delimits commands by a space as well, `${staged}` has to be surrounded by double-quotes. Pre-defined variables are only correctly interpreted by `husky` when used as a standalone argument, so the double-quotes have to be added as separate arguments directly before and after the `${staged}` argument. As the resultant string is formed by joining each argument with a space in between, quoting `${staged}` results in a leading and trailing space. Therefore, the resultant string has to be trimmed.

1. `Model` classes are usually named without having `Model` as the suffix, unlike other types of classes such as `ViewModel` and `State`. However, the Pages and the Controllers are named without suffixes as well; a Page and its Controller has to have the same names so that the Controller could use the `partial` keyword to indicate that it is a code-behind file of that Page.
