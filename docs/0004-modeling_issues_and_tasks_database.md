# 4. Modeling Issues & Tasks on Client-side Database

Date: 2021-08-23

## Status

Accepted

## Context

1. An Issue has 1 or more Tasks.
1. Based on existing functionalities (Read Issues page, Update & Delete Issues page, Read Completed Issues page), whenever an Issue is retrieved, all Tasks belonging to that Issue are retrieved as well.
1. Tasks can be embedded or reference the Issue they belong to.

According to [Data modeling in Azure Cosmos DB](https://docs.microsoft.com/en-us/azure/cosmos-db/modeling-data), reference data when:

1. Related data changes frequently.
1. Referenced data could be unbounded.

This is to avoid the negative performance impact of "transmit\[ting\] the data over the wire as well as reading and updating the item, at scale."

Referencing data however, comes with additional work:

1. Additional code has to be written to perform CRUD operations. In the case of Updating an Issue, either Set Difference or tracking Tasks that are Created, Updated and Deleted is required. This is contrasted with the one-liner of updating the old Issue object with the new Issue object.
1. "There is currently no concept of a constraint, foreign-key or otherwise, any inter-document relationships that you have in documents are effectively "weak links" and will not be verified by the database itself. If you want to ensure that the data a document is referring to actually exists, then you need to do this in your application". This is a minor issue as the only additional validation required is that the Issue referenced by the Task actually exists.

The above document recommends embedding data when:

1. There is embedded data that is queried frequently together.

However, embedding data comes with the con of sending additional (unnecessary) data over the network when Updating Tasks, as the entire Issue is sent rather than only sending the Changes.

## Decision

Embed Tasks in Issue, for the following reasons:

1. Tasks are always queried together with the corresponding Issue.
1. We do not expect couples to Create, Complete or Update Tasks frequently, so realistically:
  a. Data changes infrequently
  b. Tasks are "bounded"
1. Due to the above point, size of data transfer is not expected to be an issue.

## Consequences

1. Simplicity in development of Issues domain.
1. Larger than required data transfer.

## Updates

Superseded by [replacing IndexedDB with SQLite](0007-replace_indexeddb_with_sqlite.md).
