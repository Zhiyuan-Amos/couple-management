# 5. Remove Client-side Dependencies

Date: 2021-08-23

## Status

Accepted

## Context

Blazor Wasm applications have a long initial loading time which worsens when dependencies are added into the project.

## Decision

Remove the use of Telerik & AutoMapper. See this post for the difference in [initial load time](https://zhiyuan-amos.github.io/blog/2021-06-27-dependencies-and-application-startup-time/).

## Consequences

1. Additional code has to be written to replace these dependencies.

1. Decreased initial load time of web application.