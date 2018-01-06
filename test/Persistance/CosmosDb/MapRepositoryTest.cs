using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using NavigationApi.Api.Domain;
using NavigationApi.Api.Persistance.CosmosDb;
using Xunit;

namespace NavigationApi.Test.Persistance.CosmosDb
{
    // Please note! This test requires manually setting up a database named 'navigation-api-test' and a collection named 'maps'

    public class MapRepositoryTest
    {
        private const string DatabaseId = "navigation-api-test";
        private readonly DocumentClient _documentClient;

        public MapRepositoryTest()
        {
            _documentClient = new DocumentClient(
                new Uri("https://localhost:8081"),
                "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");

            // TODO: Database initialization logic, eg. tear down and re-create database and collections for each test
        }

        [Fact]
        public async Task Can_store_map()
        {
            var repository = new MapRepository(_documentClient, DatabaseId);
            var mapId = "testmap-" + DateTime.Now.ToString("u");
            var map = new Map(mapId,
                new Node("a"),
                new Node("b"));
            map.AddEdge("a", "b", 5);
            await repository.Create(map);
        }
    }
}