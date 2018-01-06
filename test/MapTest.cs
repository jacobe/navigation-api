using System.Collections.Generic;
using Xunit;

namespace NavigationApi.Test
{
    public class MapTest
    {
        [Fact]
        public void Has_a_required_id()
        {
            var m1 = new Map("m1");
            Assert.Equal("m1", m1.Id);
        }

        [Fact]
        public void Has_nodes()
        {
            var m1 = new Map("m1");
            m1.Nodes.Add(new Node("a"));
            Assert.Equal(1, m1.Nodes.Count);
        }
    }

    public class Map
    {
        public Map(string id, params Node[] nodes)
        {
            Id = id;
            Nodes = new List<Node>(nodes);
        }

        public string Id { get; }
        public IList<Node> Nodes { get; }
    }
}