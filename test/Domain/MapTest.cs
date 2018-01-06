using System;
using Xunit;
using NavigationApi.Api.Domain;

namespace NavigationApi.Test.Domain
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
            var m1 = new Map("m1", new Node("a"));
            Assert.Equal(1, m1.Nodes.Count);
        }
    }
}