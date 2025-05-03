using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.Services;

public class CompetitionService : ICompetitionService
{
    private readonly CompetitionDbContext _context;

    public CompetitionService(CompetitionDbContext context)
    {
        _context = context;
    }

    public Dictionary<int, List<Competition>> GetCompetitionsGroupedByYear()
    {
        return _context.Competitions
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
    }

    public (List<Result> Results, string? CategoryName) GetCategoryResults(int subCompetition2Id)
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

        return (results, category);
    }
}
