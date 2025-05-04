using Microsoft.AspNetCore.Mvc;
using TaekwondoRanking.Services;
using TaekwondoRanking.ViewModels;

namespace TaekwondoRanking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TournamentApiController : ControllerBase
    {
        private readonly ICompetitionService _competitionService;

        public TournamentApiController(ICompetitionService competitionService)
        {
            _competitionService = competitionService;
        }

        [HttpGet("filter")]
        public IActionResult Filter(string? year, string? category, string? region)
        {
            var results = _competitionService.FilterTournaments(year, category, region)
                .Select(c => new CompetitionViewModel
                {
                    IdCompetition = c.IdCompetition,
                    NameCompetition = c.NameCompetition,
                    Country = c.Country,
                    RangeLabel = c.RangeLabel,
                    FromDate = c.FromDate,
                    TillDate = c.TillDate
                });

            return Ok(results);
        }

        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            var tournament = _competitionService.GetByIdWithDetails(id);

            if (tournament == null)
                return NotFound();

            var subComp1s = tournament.SubCompetition1s.Select(sc1 => new SubCompetition1ViewModel
            {
                AgeClassName = sc1.AgeClassNavigation?.NameAgeClass,
                Categories = sc1.SubCompetition2s.Select(sc2 => new SubCompetition2ViewModel
                {
                    CategoryId = sc2.IdSubCompetition2,
                    CategoryName = sc2.IdCategoryNavigation?.NameCategory
                }).ToList()

            }).ToList();

            var result = new TournamentDetailsViewModel
            {
                SubCompetitions = subComp1s
            };

            return Ok(result);
        }
    }
}
