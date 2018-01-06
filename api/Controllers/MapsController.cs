using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace NavigationApi.Api.Controllers
{
    [Route("maps")]
    public class MapsController : Controller
    {
        [HttpGet("{id}")]
        public IActionResult GetMap(string id)
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult CreateMap([FromBody] CreateMapRequestDto createMapRequest)
        {
            return Created(Url.Action("GetMap", new { id = createMapRequest.Id }), null);
        }
    }

    public class CreateMapRequestDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nodes")]
        public IDictionary<string, Dictionary<string, int>> Nodes { get; set; }
    }
}
