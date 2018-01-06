using Newtonsoft.Json;

namespace NavigationApi.Api.Persistance.CosmosDb
{
    public class EdgeDocument
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("distance")]
        public int Distance { get; set; }
    }
}