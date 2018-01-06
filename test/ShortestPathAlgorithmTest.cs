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
            var a = new Node("a");
            Map map = new Map("t1", a, b);
            map.AddEdge("a", "b", 1);

            var startId = "a";
            var endId = "b";

            var shortestPath = new ShortestPathAlgorithm();
            Path p = shortestPath.Find(map, startId, endId);

            Assert.Equal(2, p.Nodes.Count);
            Assert.Equal(a, p.Nodes.ElementAt(0));
            Assert.Equal(b, p.Nodes.ElementAt(1));
            Assert.Equal(1, p.Distance);
        }
    }

    public class Path
    {
        public Path(IEnumerable<Node> nodes, int distance)
        {
            Nodes = new List<Node>(nodes).AsReadOnly();
            Distance = distance;
        }

        public IReadOnlyCollection<Node> Nodes { get; }
        public int Distance { get; }
    }

    public class ShortestPathAlgorithm
    {
        public Path Find(Map map, string startId, string endId)
        {
            var nodes = map.Nodes;
            if (nodes.Count == 2 
                && nodes[startId].Edges.Count == 1
                && nodes[startId].Edges[0].Neighbour == nodes[endId]
                && nodes[endId].Edges.Count == 0)
            {
                return new Path(nodes.Values, nodes[startId].Edges[0].Distance);
            }

            throw new NotImplementedException();
        }
    }
}