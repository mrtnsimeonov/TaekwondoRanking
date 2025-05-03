using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Controllers
{
    public class AthletesController : Controller
    {
        private readonly CompetitionDbContext _context;

        public AthletesController(CompetitionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> World()
        {
            try
            {
                if (_context == null)
                {
                    ViewBag.Message = "_context is null";
                    return View(new List<AthletePointsViewModel>());
                }

                if (_context.Athletes == null)
                {
                    ViewBag.Message = "_context.Athletes is null";
                    return View(new List<AthletePointsViewModel>());
                }

                var test = await _context.Athletes.ToListAsync();
                ViewBag.Message = $"Athletes table loaded, count: {test.Count}";

                return View(new List<AthletePointsViewModel>());
            }
            catch (Exception ex)
            {
                ViewBag.Message = "ERROR: " + ex.Message;
                return View(new List<AthletePointsViewModel>());
            }
        }
        public async Task<IActionResult> History(string id)
        {
            var athlete = await _context.Athletes.FindAsync(id);
            if (athlete == null) return NotFound();

            var history = await _context.Results
                .Where(r => r.IdAthlete == id)
                .Select(r => new AthleteTournamentHistoryViewModel
                {
                    CompetitionName = r.IdSubCompetition2Navigation.IdSubCompetition1Navigation.IdCompetitionNavigation.NameCompetition,
                    RangeLabel = r.IdSubCompetition2Navigation.IdSubCompetition1Navigation.IdCompetitionNavigation.RangeLabel,
                    Country = r.IdSubCompetition2Navigation.IdSubCompetition1Navigation.IdCompetitionNavigation.CountryNavigation.NameCountry,
                    AgeClass = r.IdSubCompetition2Navigation.IdCategoryNavigation.AgeClassNavigation.NameAgeClass,
                    Category = r.IdSubCompetition2Navigation.IdCategoryNavigation.NameCategory,
                    Place = r.Place ?? 0,
                    Points = r.Points ?? 0,
                    FromDate = r.IdSubCompetition2Navigation.IdSubCompetition1Navigation.IdCompetitionNavigation.FromDate ?? DateTime.MinValue
                })
                .OrderBy(h => h.FromDate)
                .ToListAsync();

            ViewBag.AthleteName = athlete.Name;
            return View(history);
        }






    }
}