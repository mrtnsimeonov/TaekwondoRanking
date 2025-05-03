using TaekwondoRanking.Models;

namespace TaekwondoRanking.Services
{

    public interface ICompetitionService
    {
        Dictionary<int, List<Competition>> GetCompetitionsGroupedByYear();
        (List<Result> Results, string? CategoryName) GetCategoryResults(int subCompetition2Id);
    }


}
