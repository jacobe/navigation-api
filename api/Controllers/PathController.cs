using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NavigationApi.Api.Domain;
using Newtonsoft.Json;

namespace NavigationApi.Api.Controllers
{
    [Route("maps/{id}/path")]
    public class PathController : Controller
    {
        private readonly IMapRepository _mapRepository;
        private readonly IPathAlgorithm _pathAlgorithm;

        public PathController(IMapRepository mapRepository, IPathAlgorithm pathAlgorithm)
        {
            _mapRepository = mapRepository;
            _pathAlgorithm = pathAlgorithm;
        }

        [HttpGet]
        public async Task<IActionResult> GetPath(string id, string start, string end)
        {
            if (string.IsNullOrEmpty(start) || string.IsNullOrEmpty(end))
            {
                return BadRequest("'start' and/or 'end' parameters are required but missing");
            }

            var map = await _mapRepository.GetById(id);
            if (map == null)
            {
                return NotFound($"Map with id '{id}' was not found");
            }

            var path = _pathAlgorithm.Find(map, start, end);
            if (path == null)
            {
                return Ok(null);
            }

            var result = ToResultDto(path);
            return Ok(result);
        }

        private PathResultDto ToResultDto(Path path)
        {
            return new PathResultDto
            {
                TotalDistance = path.Distance,
                Path = path.NodeIds
            };
        }
    }

    public class PathResultDto
    {
        [JsonProperty("totalDistance")]
        public int TotalDistance { get; set; }

        [JsonProperty("path")]
        public string[] Path { get; set; }
    }
}