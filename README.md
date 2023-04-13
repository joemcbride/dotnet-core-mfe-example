# Microfrontend (MFE) Example

This MFE sample shows a simplified Domain Driven Design approach to building applications.

- `App.Domain` - contains all of the Domain objects - Domain Aggregates and Commands, has no refereces to any specific data store
- `App.Infrastructure` - contains Queries that represent projections of Domain objects, implements repositories
- `App.Website` - the primary website acting as a BFF for a React application, using a GraphQL backend
