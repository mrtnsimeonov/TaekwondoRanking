using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using TaekwondoRanking.Models;
using System.Linq;

namespace TaekwondoRanking.Controllers
{
    [Authorize]
    public class TournamentController : Controller
    {
        private readonly CompetitionDbContext _context;

        public TournamentController(CompetitionDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Competitions
                .Include(c => c.SubCompetition1s)
                    .ThenInclude(sc1 => sc1.AgeClassNavigation)
                .Include(c => c.SubCompetition1s)
                    .ThenInclude(sc1 => sc1.SubCompetition2s)
                        .ThenInclude(sc2 => sc2.IdCategoryNavigation)
                .OrderBy(c => c.FromDate)
                .AsEnumerable()
                .GroupBy(c => c.FromDate?.Year ?? 0)
                .OrderByDescending(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => g.ToList()
                );

            return View(data);
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
