using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NavigationApi.Api.Domain;
using Newtonsoft.Json;

namespace NavigationApi.Api.Controllers
{
    [Route("maps")]
    public class MapsController : Controller
    {
        private readonly IMapRepository _mapsRepository;

        public MapsController(IMapRepository mapsRepository)
        {
            _mapsRepository = mapsRepository;
        }

        [HttpGet("{id}")]
        public IActionResult GetMap(string id)
        {
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMap([FromBody] CreateMapRequestDto createMapRequest)
        {
            if (createMapRequest == null || !ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Map map;
            try
            {
                map = ToMap(createMapRequest);
            }
            catch (Exception e)
            {
                return BadRequest($"Map is invalid. {e.Message}");
            }
            
            await _mapsRepository.Create(map);

            return Created(Url.Action("GetMap", new { id = createMapRequest.Id }), null);
        }

        private static Map ToMap(CreateMapRequestDto createMapRequest)
        {
            var nodes = from n in createMapRequest.Nodes
                        select new Node(n.Key, n.Value.Select(e => new Edge(e.Key, e.Value)).ToArray());
            var map = new Map(createMapRequest.Id, nodes.ToArray());
            map.Validate();
            return map;
        }
    }

    public class CreateMapRequestDto
    {
        [JsonProperty("id", Required = Required.Always)]
        public string Id { get; set; }

        [JsonProperty("nodes", Required = Required.Always)]
        public IDictionary<string, Dictionary<string, int>> Nodes { get; set; }
    }
}
