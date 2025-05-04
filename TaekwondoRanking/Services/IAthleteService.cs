using TaekwondoRanking.Models;
using TaekwondoRanking.ViewModels;

public interface IAthleteService
{
    Task<List<AthletePointsViewModel>> GetAllAthletesAsync();
    Task<Athlete?> GetAthleteByIdAsync(string id);
    Task<List<AthleteTournamentHistoryViewModel>> GetAthleteHistoryAsync(string athleteId);
    IEnumerable<Athlete> SearchAthletesByName(string name);

}