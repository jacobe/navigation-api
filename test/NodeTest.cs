using System.Collections.Generic;
using Xunit;

namespace NavigationApi.Test
{
    public class NodeTest
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
            a.Edges.Add(new Edge(b));
            Assert.Equal(1, a.Edges.Count);
        }
    }

    public class Node
    {
        public Node(string id)
        {
            Id = id;
        }

        public string Id { get; }

        public IList<Edge> Edges { get; } = new List<Edge>();
    }

    public class Edge
    {
        public Edge(Node neighbour)
        {
            Neighbour = neighbour;
        }

        public Node Neighbour { get; }
    }
}