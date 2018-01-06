using Xunit;
using NavigationApi.Api.Domain;

namespace NavigationApi.Test.Domain
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
}