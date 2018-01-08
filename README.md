# navigation-api
Example implementation of a navigation algorithm with an API

## Prerequisites
* .NET Core 2.0 runtime
* A local Azure Cosmos DB Emulator running on port 8081 (https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator)

## Building and running the app
*NOTE:* Before running, make sure the DB emulator is running, and manually create a database named `navigation-api` and a collection named `maps` with `/id` as partition key.

```
cd /api
dotnet run
```

## Running the unit tests
*NOTE:* Before running the tests, make sure the DB emulator is running, and manually create a database named `navigation-api-test` and a collection named `maps` with `/id` as partition key.

```
cd /test
dotnet xunit -verbose
```
