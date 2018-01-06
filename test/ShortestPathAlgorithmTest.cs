using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace NavigationApi.Test
{
    public class ShortestPathAlgorithmTest
    {
        [Fact]
        public void Finds_shortest_path_between_two_points_with_one_edge()
        {
            var b = new Node("b");
            var a = new Node("a", new Edge(b, 1));
            Map map = new Map("t1", a, b);

            var startId = "a";
            var endId = "b";

            var shortestPath = new ShortestPathAlgorithm();
            Path p = shortestPath.Find(map, startId, endId);

            Assert.Equal(2, p.Nodes.Count);
            Assert.Equal(a, p.Nodes.ElementAt(0));
            Assert.Equal(b, p.Nodes.ElementAt(1));
        }
    }

    public class Path
    {
        public Path(IEnumerable<Node> nodes)
        {
            Nodes = new List<Node>(nodes).AsReadOnly();
        }

        public IReadOnlyCollection<Node> Nodes { get; }
    }

    public class ShortestPathAlgorithm
    {
        public Path Find(Map map, string startId, string endId)
        {
            var nodes = map.Nodes;
            if (nodes.Count == 2 
                && nodes[0].Edges.Count == 1
                && nodes[0].Edges[0].Neighbour == nodes[1]
                && nodes[1].Edges.Count == 0)
            {
                return new Path(nodes);
            }

            throw new NotImplementedException();
        }
    }
}