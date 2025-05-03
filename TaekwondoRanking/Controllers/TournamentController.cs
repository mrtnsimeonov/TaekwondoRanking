using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaekwondoRanking.Models;
using TaekwondoRanking.Services;

namespace TaekwondoRanking.Controllers
{
    [Authorize]
    public class TournamentController : Controller
    {
        private readonly ICompetitionService _competitionService;

        public TournamentController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        public IActionResult Index()
        {
            var data = _competitionService.GetCompetitionsGroupedByYear();
            return View(data);
        }

        public IActionResult CategoryResults(int subCompetition2Id)
        {
            var (results, categoryName) = _competitionService.GetCategoryResults(subCompetition2Id);
            ViewBag.CategoryName = categoryName;
            return View(results);
        }
    }
}
