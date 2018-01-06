using System;
using System.Collections.Generic;
using System.Linq;

namespace NavigationApi.Api.Domain
{
    public class Map
    {
        public Map(string id, params Node[] nodes)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Must not be null or empty", nameof(id));
            if (nodes == null) throw new ArgumentNullException(nameof(nodes));

            Id = id;
            Nodes = nodes.ToDictionary(n => n.Id, n => n);
        }

        public string Id { get; }
        public IDictionary<string, Node> Nodes { get; }

        public void AddEdge(string from, string to, int distance)
        {
            Nodes[from].Edges.Add(new Edge(Nodes[to].Id, distance));
        }

        public void Validate()
        {
            if (Nodes.Count == 0)
            {
                throw new Exception("Must consist of one or more nodes");
            }
        }

        public override string ToString() => Id;
    }
}