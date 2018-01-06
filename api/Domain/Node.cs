using System.Collections.Generic;

namespace NavigationApi.Api.Domain
{
    public class Node
    {
        public Node(string id, params Edge[] edges)
        {
            Id = id;
            Edges = new List<Edge>(edges);
        }

        public string Id { get; }

        public IList<Edge> Edges { get; }
    }
}