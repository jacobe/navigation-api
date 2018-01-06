using System.Collections.Generic;
using Newtonsoft.Json;

namespace NavigationApi.Api.Persistance.CosmosDb
{
    public class MapDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nodes")]
        public IEnumerable<NodeDocument> Nodes { get; set; }

        [JsonProperty("edges")]
        public IEnumerable<EdgeDocument> Edges { get; set; }
    }
}