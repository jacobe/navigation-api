namespace NavigationApi.Api.Domain
{
    public class Edge
    {
        public Edge(string nodeId, int distance)
        {
            NodeId = nodeId;
            Distance = distance;
        }

        public string NodeId { get; }
        public int Distance { get; }
    }
}