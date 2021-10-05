# 6. Implement Custom State Containers

Date: 2021-08-23

## Status

Accepted

## Context

State Containers are required.

## Decision

Implement Custom State Containers rather than use existing libraries [Blazor-State](https://github.com/TimeWarpEngineering/blazor-state) & [Fluxor](https://github.com/mrpmorris/Fluxor) as:

1. These libraries aren't in active development.
1. [MSDN](https://docs.microsoft.com/en-us/aspnet/core/blazor/state-management?pivots=webassembly&view=aspnetcore-5.0#in-memory-state-container-service-wasm) demonstrates the ease of implementing custom State Containers.
1. The application is small and there's no real need to use an existing library yet.

## Consequences

1. Decreased initial load time of web application (See [previous ADR](0005-remove_client_side_dependencies.md)).