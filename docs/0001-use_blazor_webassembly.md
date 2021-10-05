# 1. Use Blazor WebAssembly

Date: 2021-08-23

## Status

Accepted

## Context

Client can be either a Web Application (built using Blazor, React etc) or Mobile Application (built using Flutter etc).

## Decision

Use Blazor WebAssembly because:

1. I am familiar with C# & .NET Core and I want to create this application quickly without spending too much time to pick up another Programming Language and the entire ecosystem that comes along with the other Frameworks.
1. Blazor is relatively new and the performance (especially the initial loading speed for Blazor WebAssembly) isn't as good as existing Frameworks, but it's good enough as I do not plan to release this app in the next 1 to 2 years. Hopefully, the loading speed would have improved by then.
1. Blazor WebAssembly is chosen over Blazor Server as Blazor WebAssembly applications can be hosted on a static file server, which is cheaper than the hosting solutions available for Blazor Server. Also, Blazor WebAssembly applications can be accessed while offline.

## Consequences

1. Slow initial loading speed and relatively larger app download size compared to creating PWA from other Frameworks
1. No push notifications for [iOS](https://stackoverflow.com/a/64576541)
1. Lesser access to device features compared to mobile apps