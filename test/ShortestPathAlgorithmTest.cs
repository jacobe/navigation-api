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
            Map map = new Map("t1",
                new Node("a"),
                new Node("b"));
            map.AddEdge("a", "b", 1);

            var shortestPath = new ShortestPathAlgorithm();
            Path p = shortestPath.Find(map, "a", "b");

            Assert.Equal(2, p.NodeIds.Length);
            Assert.Equal("a", p.NodeIds.ElementAt(0));
            Assert.Equal("b", p.NodeIds.ElementAt(1));
            Assert.Equal(1, p.Distance);
        }

        [Fact]
        public void Finds_shortest_path_between_two_points_with_two_edges()
        {
            Map map = new Map("t2",
                new Node("a"),
                new Node("b"));
            map.AddEdge("a", "b", 2);
            map.AddEdge("a", "b", 1);

            var shortestPath = new ShortestPathAlgorithm();
            Path p = shortestPath.Find(map, "a", "b");

            Assert.Equal(2, p.NodeIds.Length);
            Assert.Equal("a", p.NodeIds.ElementAt(0));
            Assert.Equal("b", p.NodeIds.ElementAt(1));
            Assert.Equal(1, p.Distance);
        }

        public static IEnumerable<object[]> MapATestCases()
        {
            Map map = new Map("A",
                new Node("a"),
                new Node("b"),
                new Node("c"));

            map.AddEdge("a", "b", 2);
            map.AddEdge("a", "c", 5);
            map.AddEdge("b", "c", 2);
            map.AddEdge("c", "a", 8);

            yield return new object[] { map, "a", "c", "a,b,c", 4 };
            yield return new object[] { map, "b", "a", "b,c,a", 10 };
            yield return new object[] { map, "a", "a", "a", 0 };
        }

        [Theory]
        [MemberData(nameof(MapATestCases))]
        public void Finds_shortest_paths_in_map_A(Map map, string startId, string endId, string expectedPath, int expectedDistance)
        {
            var shortestPath = new ShortestPathAlgorithm();
            Path path = shortestPath.Find(map, startId, endId);

            Assert.Equal(expectedPath, string.Join(',', path.NodeIds));
            Assert.Equal(expectedDistance, path.Distance);
        }
    }

    public class Path
    {
        public Path(IEnumerable<string> nodeIds, int distance)
        {
            NodeIds = nodeIds.ToArray();
            Distance = distance;
        }

        public string[] NodeIds { get; }
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

            return new Path(path, dist[endId]);
        }
    }
}