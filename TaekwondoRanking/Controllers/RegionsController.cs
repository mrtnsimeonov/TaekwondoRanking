using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models; // <- for CompetitionDbContext
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Controllers
{
    public class RegionsController : Controller
    {
        private readonly CompetitionDbContext _context;

        public RegionsController(CompetitionDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> World()
        {
            var athletes = await _context.Athletes
                .Include(a => a.Results)
                .Select(a => new AthletePointsViewModel
                {
                    IdAthlete = a.IdAthlete,
                    Name = a.Name,
                    Country = a.Country,
                    TotalPoints = a.Results.Sum(r => (int?)r.Points) ?? 0

                })
                .OrderByDescending(a => a.TotalPoints)
                .ToListAsync();

            ViewBag.Message = $"Loaded {athletes.Count} athletes with points.";
            return View(athletes);
        }


        public IActionResult Bulgaria()
        {
            return View();
        }

        public IActionResult Europe()
        {
            return View();
        }
    }
}
