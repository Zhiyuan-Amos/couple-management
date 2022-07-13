# Programatically format code using dotnet format

Date: 2022-05-08

## Status

Accepted

## Context

There are 2 popular code formatter tools that formats the codebase based on the configurations in `.editorconfig`:

1. [dotnet format](https://github.com/dotnet/format)
2. [CleanupCode](https://www.jetbrains.com/help/rider/CleanupCode.html)

## Decision

While both tools work, `CleanupCode` formats additional file types compared to `dotnet format`, such as `razor`, `html`, `css` and `js`. `CleanupCode` also has additional features such as cleaning up unused imports properly, removing unnecessary code (e.g. `new T() {}` -> `new T {}`) and adhering to line limits. One of the reasons is some of these corrections do not have a corresponding configuration in `.editorconfig`, but `CleanupCode` supplements its capabilities with `Resharper`. There are [additional configurations](https://www.jetbrains.com/help/resharper/EditorConfig_Index.html) available to `CleanupCode` as well.

However, `CleanupCode` tends to be: 
1. Somewhat buggy. When running locally, errors seem to be thrown frequently and the console is filled with red text. Also, it deletes used imports from Function Apps.
2. Documentation is sparse.

Therefore, use `CleanupCode` for `razor`, `html`, `css` & `js`, and `dotnet format` for `cs`, `csproj`, `json` files.

## Consequences

1. More effort required to integrate 2 tools instead of 1.
2. Additional overhead as running each tool against 1 file takes about 7s on my local machine. See [forum](https://rider-support.jetbrains.com/hc/en-us/community/posts/4402880293650-How-to-speed-up-CleanupCode-Command-Line-Tool-) for more details.
