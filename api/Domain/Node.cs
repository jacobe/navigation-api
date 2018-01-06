using System;
using System.Collections.Generic;

namespace NavigationApi.Api.Domain
{
    public class Node
    {
        public Node(string id, params Edge[] edges)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentException("Must not be null or empty", nameof(id));
            if (edges == null) throw new ArgumentNullException(nameof(edges));

            Id = id;
            Edges = new List<Edge>(edges);
        }

        public string Id { get; }

        public IList<Edge> Edges { get; }
    }
}