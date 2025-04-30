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

        //public IActionResult World()
        //{
        //    //var athletesData = _context.Athletes
        //    //    .Join(
        //    //        _context.Results,
        //    //        athlete => athlete.IdAthlete,
        //    //        result => result.IdAthlete,
        //    //        (athlete, result) => new { athlete, result.Points }
        //    //    )
        //    //    .GroupBy(a => new { a.athlete.IdAthlete, a.athlete.Name, a.athlete.Country })
        //    //    .Select(g => new
        //    //    {
        //    //        AthleteName = g.Key.Name,
        //    //        Country = g.Key.Country,
        //    //        TotalPoints = g.Sum(x => x.Points)
        //    //    })
        //    //    .OrderByDescending(a => a.TotalPoints)
        //    //    .ToList();

        //    //return View("WorldAthletes");
        //    var test = await _context.Athletes.ToListAsync();
        //    ViewBag.TestCount = test.Count;

        //}
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



    }
}