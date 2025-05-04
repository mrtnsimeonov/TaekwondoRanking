using Microsoft.AspNetCore.Mvc;
using TaekwondoRanking.Models;

namespace TaekwondoRanking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AthletesApiController : ControllerBase
    {
        private readonly IAthleteService _athleteService;

        public AthletesApiController(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        [HttpGet("search")]
        public IActionResult Search(string name)
        {
            var results = _athleteService.SearchAthletesByName(name);
            return Ok(results);
        }
    }
}
