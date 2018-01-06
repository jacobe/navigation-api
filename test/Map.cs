using System.Collections.Generic;
using System.Linq;

namespace NavigationApi.Test
{
    public class Map
    {
        public Map(string id, params Node[] nodes)
        {
            Id = id;
            Nodes = nodes.ToDictionary(n => n.Id, n => n);
        }

        public string Id { get; }
        public IDictionary<string, Node> Nodes { get; }

        public void AddEdge(string from, string to, int distance)
        {
            Nodes[from].Edges.Add(new Edge(Nodes[to], distance));
        }

        public override string ToString()
        {
            return Id;
        }
    }
}