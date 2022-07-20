# Api: Migrate from Function App to Container App

Date: 2022-07-10

## Status

Accepted

## Context

Api was hosted on Function App because it's free, even though more work was required compared to having it hosted on App Service (Function App does not have as many features as a standard ASP.NET Server, such as Middleware support, built-in Input Validation etc). 

Container App was recently released, and it provides hosting of ASP.NET Server for free.

## Decision

Re-write Api project as a standard ASP.NET Server because:

1. As mentioned above, lesser code needs to be written.
1. Function App's primary use case is for [event-driven applications](https://docs.microsoft.com/en-us/azure/container-apps/compare-options#azure-functions), which doesn't fit the use case as the backend for Client. 

## Consequences

1. Work required to port the application. It should be smooth, however, as most of the work would involve code deletion and replacing variables.
