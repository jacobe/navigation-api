using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NavigationApi.Api.Controllers
{
    [Route("maps")]
    public class MapsController : Controller
    {
        [HttpPost]
        public IActionResult CreateMap()
        {
            return Ok();
        }
    }
}
