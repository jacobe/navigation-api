using Newtonsoft.Json;

namespace NavigationApi.Api.Persistance.CosmosDb
{
    public class NodeDocument
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}