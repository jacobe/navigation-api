using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using NavigationApi.Api.Domain;

namespace NavigationApi.Test.Domain
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
}