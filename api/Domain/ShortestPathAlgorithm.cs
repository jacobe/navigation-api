using System.Collections.Generic;
using System.Linq;

namespace NavigationApi.Api.Domain
{
    public class ShortestPathAlgorithm : IPathAlgorithm
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

            // Now walk backwards through the vertex set to find the individual nodes on the path
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