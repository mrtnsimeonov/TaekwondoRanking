using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaekwondoRanking.Models;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Controllers
{
    [Authorize]
    public class AthletesController : Controller
    {
        private readonly IAthleteService _athleteService;

        public AthletesController(IAthleteService athleteService)
        {
            _athleteService = athleteService;
        }

        public async Task<IActionResult> World()
        {
            var athletes = await _athleteService.GetAllAthletesAsync();
            return View(athletes);
        }

        public async Task<IActionResult> History(string id)
        {
            var athlete = await _athleteService.GetAthleteByIdAsync(id);
            if (athlete == null) return NotFound();

            var history = await _athleteService.GetAthleteHistoryAsync(id);
            ViewBag.AthleteName = athlete.Name;
            return View(history);
        }
    }
}
