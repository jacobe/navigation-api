using System;

namespace NavigationApi.Api.Domain
{
    public class Edge
    {
        public Edge(string nodeId, int distance)
        {
            if (string.IsNullOrEmpty(nodeId)) throw new ArgumentException("message", nameof(nodeId));
            if (distance < 0) throw new ArgumentException("Must not be negative", nameof(distance));

            NodeId = nodeId;
            Distance = distance;
        }

        public string NodeId { get; }
        public int Distance { get; }
    }
}