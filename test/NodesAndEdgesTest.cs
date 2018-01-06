using System.Collections.Generic;
using Xunit;

namespace NavigationApi.Test
{
    public class NodesAndEdgesTest
    {
        [Fact]
        public void Has_a_required_id()
        {
            var a = new Node("a");
            Assert.Equal("a", a.Id);
        }

        [Fact]
        public void Can_have_edges()
        {
            var a = new Node("a");
            var b = new Node("b");
            a.Edges.Add(new Edge(b, 1));
            Assert.Equal(1, a.Edges.Count);
        }

        [Fact]
        public void Edges_have_a_distance()
        {
            var edge = new Edge(new Node("a"), distance: 1);
            Assert.Equal(1, edge.Distance);
        }
    }

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

    public class Edge
    {
        public Edge(Node neighbour, int distance)
        {
            Node = neighbour;
            Distance = distance;
        }

        public Node Node { get; }
        public int Distance { get; }
    }
}