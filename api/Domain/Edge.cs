namespace NavigationApi.Api.Domain
{
    public class Edge
    {
        public Edge(Node neighbour, int distance)
        {
            Node = neighbour;
            Distance = distance;
        }

        public Node Node { get; }
        public string NodeId => Node.Id;
        public int Distance { get; }
    }
}