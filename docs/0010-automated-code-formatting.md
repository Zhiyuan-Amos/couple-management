# Automated Code Formatting

Date: 2022-05-08

## Status

Accepted

## Context

There are several ways to format code automatically:

1. [husky](https://alirezanet.github.io/Husky.Net/) to format code on commit.
2. `husky` to format code on push.
3. [slash-command-dispatch](https://github.com/peter-evans/slash-command-dispatch).
4. [lint-action](https://github.com/wearerequired/lint-action).
5. `GitHub Actions` to format code on demand.

## Decision

.NET code formatters are not performant; formatting 1 file takes about 7s on my local machine. Therefore, formatting code per commit results in quite some overhead.

While formatting code on push incurs lesser overhead, there is still overhead.

As I am the only developer of this project, I have the tendency to push to `master` rather than creating a PR, so the `slash-command-dispatch` to trigger code cleanup would be bypassed.

`lint-action` does not support `CleanupCode` which is used to format some files. Also, same as above, I have the tendency to push to `master`, so this action would be bypassed.

`GitHub Actions` incurs zero overhead. As I am the only developer of this project and I write code in a consistent manner, the code largely does not require additional formatting.

Based on this project's unique requirements, formatting code on demand using GitHub Actions seems to be the best option.

## Consequences

1. No overhead during development.
2. Codebase will never be in a "pristine" state except at the point in time when `GitHub Actions` completes a run and the changes merged into `master`.
