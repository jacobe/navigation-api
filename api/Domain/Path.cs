using System.Collections.Generic;
using System.Linq;

namespace NavigationApi.Api.Domain
{
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
}