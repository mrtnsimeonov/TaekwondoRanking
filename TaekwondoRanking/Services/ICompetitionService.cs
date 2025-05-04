using System.Collections.Generic;
using TaekwondoRanking.Models;

namespace TaekwondoRanking.Services
{
    public interface ICompetitionService
    {
        IEnumerable<Competition> GetAll();
        Competition GetById(int id);
        void Add(Competition competition);
        void Update(Competition competition);
        void Delete(int id);

        // ✅ NEW: AJAX filtering support
        IEnumerable<Competition> FilterTournaments(string? year, string? category, string? region);
        Competition GetByIdWithDetails(int id);

    }
}