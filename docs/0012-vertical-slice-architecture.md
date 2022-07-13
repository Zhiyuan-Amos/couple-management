# Architecture

Date: 2022-05-08

## Status

Accepted

## Context

Generally, there are 3 ways to architect the codebase:

1. Horizontal-Slice
2. Clean
3. Vertical-Slice

## Decision

Typically, Horizontal-Slice Architecture is bad. While the main pro is reusability, since Controllers can call any methods in the Logic classes, but it's more likely that the methods in Logic classes are only called once.

Even though I have not worked on Clean Architecture before, based on this [forum](https://www.reddit.com/r/dotnet/comments/lw13r2/choosing_between_using_cleanonion_or_vertical/), it seems that Vertical-Slice Architecture is the better option.

## Consequences

Unsure as I have yet to work on a sufficiently big codebase implementing either Clean Architecture or Vertical-Slice Architecture.
