using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaekwondoRanking.Helpers;
using TaekwondoRanking.Models;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Controllers
{
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        // ✅ CHANGED: The id parameter is now a string.
        public IActionResult Delete(string id)
        {
            if (id != null)
            {
                // Use TryAdd with the string ID. The '0' is just a placeholder.
                TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd(id, 0);
            }

            TempData["SuccessMessage"] = "Athlete temporarily removed. They will reappear on app restart.";
            return RedirectToAction("World", "Regions");
        }


    }
}