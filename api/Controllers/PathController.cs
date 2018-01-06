using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NavigationApi.Api.Domain;

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
        public IActionResult GetPath(string id, string start, string end)
        {
            return Ok();
        }
    }
}