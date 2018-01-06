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

        [Fact]
        public void Finds_shortest_path_between_two_points_with_two_edges()
        {
            var a = new Node("a");
            var b = new Node("b");
            Map map = new Map("t1", a, b);
            map.AddEdge("a", "b", 2);
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

            Dictionary<string, int> dist = nodes.ToDictionary(n => n.Key, n => int.MaxValue);
            dist[startId] = 0; // initialize distance from start node to itself to 0

            Dictionary<string, string> prev = nodes.ToDictionary(n => n.Key, n => (string)null);

            var queue = new HashSet<string>(nodes.Keys);
            while (queue.Count != 0)
            {
                var u = queue.OrderBy(v => dist[v]).First();
                queue.Remove(u);
                
                if (u == endId) break; // we reached the target, quit here

                foreach (var edge in nodes[u].Edges)
                {
                    var v = edge.Node.Id;
                    var alt = dist[u] + edge.Distance;
                    if (alt < dist[v])
                    {
                        dist[v] = alt;
                        prev[v] = u;
                    }
                }
            }

            var path = new List<string>();
            var c = endId;
            while (prev[c] != null)
            {
                path.Insert(0, c);
                c = prev[c];
            }
            path.Insert(0, c);

            if (path.Count == 1) // means; no path found
            {
                return null;
            }

            return new Path(path.Select(id => nodes[id]), dist[endId]);
        }
    }
}