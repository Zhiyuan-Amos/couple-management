# Use Azure AD B2C for Identity Access Management

Date: 2022-05-08

## Status

Accepted

## Context

Auth0 is one of the mainstream IAM solution. B2C is also worth considering as the rest of the services used by this application is also on Azure, and it's nice to have everything on Azure.

One of the key considerations is to keep the user signed in for an extended period of time for ease of usability. Using OAuth's PKCE flow, a new Access Token can be retrieved by using either Refresh Token or Session Cookie.

## Decision

Refresh Token however, has a limited lifetime because they can be compromised through XSS. Therefore, Auth0 persists the Refresh Token in Session Storage while B2C caps the Refresh Token's lifetime at 24hours. Therefore, Refresh Tokens cannot be used to keep a user logged in for a long period of time. 

Session Cookie has a longer lifetime. Auth0 caps the Session lifetime at 3 days (for free plan. Extended to 100 days for enterprise plan) while B2C caps it at 90 days.

Ideally, the user should never have to re-login again upon being authenticated, but having to re-login every 90 days seem to be the best available option for now. Therefore, use B2C.

## Consequences

1. User has to re-login every 90 days.
