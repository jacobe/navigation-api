using System;
using System.Collections.Generic;
using System.Linq;
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
            var mapDocument = new MapDocument
            {
                Id = map.Id,
                Nodes = map.Nodes.Values.Select(ToNodeDocument),
                Edges = map.Nodes.Values.SelectMany(ToEdgeDocuments)
            };

            var collectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, CollectionId);
            await _documentClient.CreateDocumentAsync(collectionUri, mapDocument);
        }

        public async Task<Map> GetById(string id)
        {
            var collectionUri = UriFactory.CreateDocumentCollectionUri(_databaseId, CollectionId);
            var query = _documentClient.CreateDocumentQuery<MapDocument>(collectionUri, 
                new SqlQuerySpec("SELECT * FROM c WHERE c.id=@id")
                {
                    Parameters =
                    {
                        new SqlParameter("@id", id)
                    }
                },
                new FeedOptions
                {
                    PartitionKey = new PartitionKey(id)
                });

            var results = await query.ToAsyncEnumerable().ToList();
            var mapDocument = results.FirstOrDefault();
            
            if (mapDocument == null)
            {
                return null; // map not found
            }

            var map = ToMap(mapDocument);
            return map;
        }

        private static NodeDocument ToNodeDocument(Node node)
        {
            return new NodeDocument { Id = node.Id };
        }

        private static IEnumerable<EdgeDocument> ToEdgeDocuments(Node node)
        {
            return node.Edges.Select(e => new EdgeDocument
            {
                From = node.Id,
                To = e.NodeId,
                Distance = e.Distance
            });
        }
        
        private Map ToMap(MapDocument mapDocument)
        {
            var nodes = mapDocument.Nodes.Select(nd => new Node(nd.Id)).ToArray();
            var map = new Map(mapDocument.Id, nodes);

            foreach (var edgeDocument in mapDocument.Edges)
            {
                map.AddEdge(edgeDocument.From, edgeDocument.To, edgeDocument.Distance);
            }

            return map;
        }
    }
}