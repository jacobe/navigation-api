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
    // in the Azure Cosmos DB Storage Emulator (see https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator)

    public class MapRepositoryTest
    {
        private const string DatabaseId = "navigation-api-test";
        private readonly IDocumentClient _documentClient;

        public MapRepositoryTest()
        {
            _documentClient = LocalEmulatorHelper.CreateDocumentClient();

            // TODO: Database initialization logic, eg. tear down and re-create database and collections for each test
        }

        [Fact]
        public async Task Can_store_and_retrieve_map()
        {
            var repository = new MapRepository(_documentClient, DatabaseId);
            var mapId = "testmap-" + DateTime.Now.ToString("u");
            var map = new Map(mapId,
                new Node("a"),
                new Node("b"));
            map.AddEdge("a", "b", 5);
            await repository.Create(map);

            var result = await repository.GetById(mapId);
            Assert.NotNull(result);
            Assert.Equal(mapId, result.Id);
            Assert.Contains("a", result.Nodes.Keys);
            Assert.Contains("b", result.Nodes.Keys);
            Assert.Equal(5, result.Nodes["a"].Edges[0].Distance);
        }
    }
}