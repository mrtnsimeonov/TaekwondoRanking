using Microsoft.AspNetCore.Authorization; // ✅ ADDED for [Authorize]
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TaekwondoRanking.Models;
// using TaekwondoRanking.Views; // This using might not be necessary if FilterViewModel is not used here
using TaekwondoRanking.Services; // ✅ ADDED for IAthleteService

namespace TaekwondoRanking.Controllers
{
    public class HomeController : Controller
    {
        // ✅ ADDED: Private field to hold the athlete service
        private readonly IAthleteService _athleteService;
        private readonly ILogger<HomeController> _logger; // Optional: If you want to add logging later

        // ✅ ADDED: Constructor to inject the IAthleteService (and ILogger if you uncomment)
        public HomeController(IAthleteService athleteService, ILogger<HomeController> logger)
        {
            _athleteService = athleteService;
            _logger = logger; // Optional
        }

        public IActionResult Index() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // This action seems to point to a view in a different folder. Ensure this is intended.
        // If "World.cshtml" is in "Views/Home/", then just "return View();" is fine.
        public IActionResult World() => View("~/Views/Regions/World.cshtml");

        [ResponseCache(Duration = 3600, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        public IActionResult TournamentResults()
        {
            return View();
        }

        // ✅ ADDED: New action for the Deleted Athletes page
        [Authorize(Roles = "Admin")] // Ensures only admins can access this page
        public async Task<IActionResult> DeletedAthletes()
        {
            var deletedAthletes = await _athleteService.GetTemporarilyDeletedAthletesAsync();
            return View(deletedAthletes); // This will look for a view at Views/Home/DeletedAthletes.cshtml
        }
    }
}