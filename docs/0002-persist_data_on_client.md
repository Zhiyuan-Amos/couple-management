# 2. Persist Data on Client

Date: 2021-08-23

## Status

Accepted

## Context

There's no simple & secure way to persist data in the database.

1. If data is stored unencrypted in the database, there's no simple way to protect the data from myself as I'm the only developer; there's no one else to audit me.
1. Public Key Encryption can be used, but it comes with several complexities and limitations:
  1. Complexity: No easy way to perform search.
  1. Complexity: Re-encryption of data when refreshing public & private keys.
  1. Limitation: Requires Internet connection to access all data.

## Decision

1. Data is stored locally.

1. Data is stored in database temporarily until the other party synchronises the data.

## Consequences

1. Storage space is much more finite.

1. User can read & search data while offline. 

1. Offline CUD requires additional logic for concurrency conflict resolution (think git).

1. Additional development effort is required to access data on different devices.
