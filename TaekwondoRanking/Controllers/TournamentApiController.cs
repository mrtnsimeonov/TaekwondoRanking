using AutoMapper;
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
        private readonly IMapper _mapper;



        public TournamentApiController(ICompetitionService competitionService, IMapper mapper)
        {
            _competitionService = competitionService;
            _mapper = mapper;
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
                })
                .ToList();

            string message = results.Any()
                ? "Tournaments found."
                : "No tournaments match the selected filters.";

            return Ok(new
            {
                Message = message,
                Data = results
            });
        }


        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            var tournament = _competitionService.GetByIdWithDetails(id);

            if (tournament == null)
                return NotFound();

            var subComp1s = _mapper.Map<List<SubCompetition1ViewModel>>(tournament.SubCompetition1s);

            return Ok(new TournamentDetailsViewModel
            {
                SubCompetitions = subComp1s
            });
        }

    }
}