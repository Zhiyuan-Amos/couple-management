# 3. Image Persistence: Optimize Storage Space

Date: 2021-08-23

## Status

Accepted

## Context

1. Images can be persisted as a byte array or Base64 Encoded String (rendering on UI requires the image to be in this format).

1. Also, images can be persisted in a denormalized manner, which suits IndexedDb as a NoSQL database.

## Decision

1. Images are persisted as a byte array because the Base64 Encoded String is about 33% larger.

1. Images are persisted in a normalized manner rather than duplicating it per View.

## Consequences

1. Smaller storage space required to store images; Users can store more images given a fixed storage space.

1. Some additional processing is required to convert each image from byte array to Base64 Encoded String.

1. Additional logic is required to ensure Referential Integrity and to query images.