# 2. Persist Data on Client

Date: 2021-08-23

## Status

Accepted

## Context

There's no way to protect the data from myself if it's stored inside the database, as I'm the only developer; there's no one else to audit me.

## Decision

1. Data is stored locally.

1. Data is stored in database temporarily until the other party synchronises the data.

## Consequences

1. Storage space is much more finite.

1. User can read data while offline. 

1. Offline CUD requires additional logic for concurrency conflict resolution.
