# Microfrontend (MFE) Example

This MFE sample shows a simplified Domain Driven Design approach to building applications.

- `App.Domain` - contains all of the Domain objects - Domain Aggregates and Commands, has no refereces to any specific data store
- `App.Infrastructure` - contains Queries that represent projections of Domain objects, implements repositories
- `App.Website` - the primary website acting as a BFF for a React application, using a GraphQL backend

# Running the Application

### ASP.NET Application

In VSCode, run the `website` debug task. Otherwise:

```
cd ./src/App.Website
dotnet run
```

### React Application

```
cd ./src/App.Website/frontend
npm start
```

Browse to `https://localhost:1234`.

# GraphQL

Browse to `/graphql/playground` to try out GraphQL queries.
