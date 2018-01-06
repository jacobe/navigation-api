using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using NavigationApi.Api.Domain;

namespace NavigationApi.Api.Persistance.CosmosDb
{
    public class MapRepository : IMapRepository
    {
        const string CollectionId = "maps";

        private readonly IDocumentClient _documentClient;
        private readonly string _databaseId;

        public MapRepository(IDocumentClient documentClient, string databaseId)
        {
            _documentClient = documentClient;
            _databaseId = databaseId;
        }

        public async Task Create(Map map)
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, CollectionId);
            await _documentClient.CreateDocumentAsync(collectionUri, map);
        }

        public Task<Map> GetById(string id)
        {
            throw new System.NotImplementedException();
        }
    }
}