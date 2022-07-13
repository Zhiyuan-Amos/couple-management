# Omit Repository Layer

Date: 2022-05-08

## Status

Accepted

## Context

Sometimes, Repository Layer is used in .NET projects. The Repository Layer wraps the ORM (e.g. EF Core), and the Logic Layer uses the Repository Layer to interface with the database rather than using the ORM directly.

## Decision

In the event swapping the ORM or database is required, only the Repository Layer is required. However, this key benefit is unlikely to be useful, because it's unlikely that this project will ever need to swap the ORM or the database. Even if swapping of the database is required, it's relatively easy to do so, having swapped between SQL Server and CosmosDB before; EF Core does a great job in abstracting away the database-specific implementations.

Method names in the Repository Layer are likely to be long and hard to read. For example, if an Entity has multiple Children Entities and filtering is required, a possible method name looks like `GetXWithChildrenYAndZFilterByName`.

If business logic requires several similar permutations such as `GetXWithChildYAndFilterByName`, then it's likely that the Repository Layer class is bloated, making it hard to know from a glance if a method you require already exists in that class.

Since there's no use case for the Repository Layer now, and implementing it is costly, let's not implement it.

## Consequences

1. No overhead during development; use EF Core's `DbContext` to interface with the database as required.
