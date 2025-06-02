using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Controllers
{
    public class TournamentController : Controller
    {
        private readonly CompetitionDbContext _context;

        public TournamentController(CompetitionDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["ShowUnauthorizedModal"] = true;
                return RedirectToAction("Index", "Home");
            }

            // Normal logic for showing tournament results
            return View();
        }

        public IActionResult CategoryResults(int subCompetition2Id)
        {
            var results = _context.Results
                .Where(r => r.IdSubCompetition2 == subCompetition2Id)
                .Include(r => r.IdAthleteNavigation)
                .OrderBy(r => r.Place)
            .ToList();

            var category = _context.SubCompetition2s
                .Include(sc2 => sc2.IdCategoryNavigation)
                .FirstOrDefault(sc2 => sc2.IdSubCompetition2 == subCompetition2Id)?
                .IdCategoryNavigation?.NameCategory;

            ViewBag.CategoryName = category;
            return View(results);
        }
        
    }
}