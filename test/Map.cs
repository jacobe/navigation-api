using System.Collections.Generic;
using System.Linq;

namespace NavigationApi.Test
{
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