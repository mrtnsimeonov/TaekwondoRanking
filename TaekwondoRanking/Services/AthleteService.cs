using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.ViewModels;
using TaekwondoRanking.Helpers;

public class AthleteService : IAthleteService
{
    private readonly CompetitionDbContext _context;

    public AthleteService(CompetitionDbContext context)
    {
        _context = context;
    }

    public async Task<List<AthletePointsViewModel>> GetAllAthletesAsync()
    {
        if (_context?.Athletes == null)
            return new List<AthletePointsViewModel>();

        var athletes = await _context.Athletes.ToListAsync();

        // You may replace this with your actual logic
        return new List<AthletePointsViewModel>();
    }

    public async Task<Athlete?> GetAthleteByIdAsync(string id)
    {
        return await _context.Athletes.FindAsync(id);
    }

    public async Task<List<AthleteTournamentHistoryViewModel>> GetAthleteHistoryAsync(string athleteId)
    {
        var athlete = await _context.Athletes.FindAsync(athleteId);
        if (athlete == null) return new List<AthleteTournamentHistoryViewModel>();

        return await _context.Results
            .Where(r => r.IdAthlete == athleteId)
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
    }
    public IEnumerable<Athlete> SearchAthletesByName(string name)
    {
        return _context.Athletes
            .Where(a => a.Name.Contains(name))
            .ToList();
    }
    public async Task<IEnumerable<Athlete>> GetTemporarilyDeletedAthletesAsync()
    {
        var deletedAthleteIds = TemporaryDeletionManager.TemporarilyDeletedAthleteIds.Keys;

        if (!deletedAthleteIds.Any())
        {
            return new List<Athlete>(); // Return an empty list if no one is deleted
        }

        return await _context.Athletes
                             .Where(a => deletedAthleteIds.Contains(a.IdAthlete))
                             .ToListAsync();
    }
}