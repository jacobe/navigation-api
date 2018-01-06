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

        public static IEnumerable<object[]> MapBTestCases()
        {
            Map map = new Map("A",
                new Node("a"),
                new Node("b"),
                new Node("c"),
                new Node("d"));
            
            map.AddEdge("a", "b", 1);
            map.AddEdge("b", "a", 2);
            map.AddEdge("c", "d", 3);
            map.AddEdge("d", "c", 4);

            yield return new object[] { map, "a", "b", "a,b", 1 };
            yield return new object[] { map, "a", "c", null, 0 };
        }

        [Theory]
        [MemberData(nameof(MapATestCases))]
        [MemberData(nameof(MapBTestCases))]
        public void Finds_shortest_paths_in_complex_maps(Map map, string startId, string endId, string expectedPath, int expectedDistance)
        {
            var shortestPath = new ShortestPathAlgorithm();
            Path path = shortestPath.Find(map, startId, endId);

            if (expectedPath == null)
            {
                Assert.Null(path);
            }
            else
            {
                Assert.Equal(expectedPath, string.Join(',', path.NodeIds));
                Assert.Equal(expectedDistance, path.Distance);
            }
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

            // use Dijkstra's algorithm to find the shortest path between two nodes
            Dictionary<string, Vertex> set = nodes.ToDictionary(n => n.Key, n => Vertex.CreateFrom(n.Value));
            set[startId].Distance = 0; // distance from start node to itself is 0

            var queue = new HashSet<string>(nodes.Keys);
            while (queue.Count != 0)
            {
                var current = queue.OrderBy(n => set[n].Distance).First(); // TODO: Can be optimized by using a priority queue
                queue.Remove(current);
                
                if (current == endId) break; // we reached the target, quit here

                // find shortest distances to a neighour node
                foreach (var edge in nodes[current].Edges)
                {
                    var v = set[edge.NodeId];
                    var alt = set[current].Distance + edge.Distance;
                    if (alt < v.Distance)
                    {
                        v.Distance = alt;
                        v.PrevId = current;
                    }
                }
            }

            // Now walk backwards through the vertex set to find the path from start to end
            var path = new List<string>();
            var currentId = endId;
            while (set[currentId].PrevId != null)
            {
                path.Insert(0, currentId);
                currentId = set[currentId].PrevId;
            }

            if (path.Count == 0 && endId != startId)
            {
                return null; // meaning; no path found
            }

            path.Insert(0, startId); // prepend the start node

            return new Path(path, set[endId].Distance);
        }

        class Vertex
        {
            public int Distance;
            public string PrevId;

            public static Vertex CreateFrom(Node n)
            {
                return new Vertex
                {
                    Distance = int.MaxValue,
                    PrevId = null
                };
            }
        }
    }
}