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
    }

    public class Node
    {
        public Node(string id)
        {
            Id = id;
        }

        public string Id { get; private set; }
    }
}