using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TaekwondoRanking.Data;
using TaekwondoRanking.Models;

namespace TaekwondoRanking.Services
{
    public class CompetitionService : ICompetitionService
    {
        private readonly CompetitionDbContext _context;

        public CompetitionService(CompetitionDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Competition> GetAll()
        {
            return _context.Competitions.ToList();
        }

        public Competition GetById(int id)
        {
            return _context.Competitions.FirstOrDefault(c => c.IdCompetition == id);
        }

        public void Add(Competition competition)
        {
            _context.Competitions.Add(competition);
            _context.SaveChanges();
        }

        public void Update(Competition competition)
        {
            _context.Competitions.Update(competition);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var comp = _context.Competitions.FirstOrDefault(c => c.IdCompetition == id);
            if (comp != null)
            {
                _context.Competitions.Remove(comp);
                _context.SaveChanges();
            }
        }


        // ✅ NEW: AJAX filter method
        public IEnumerable<Competition> FilterTournaments(string? year, string? category, string? region)
        {
            var query = _context.Competitions.AsQueryable();

            if (!string.IsNullOrEmpty(year))
                query = query.Where(c => c.FromDate.HasValue && c.FromDate.Value.Year.ToString() == year);

            if (!string.IsNullOrEmpty(category))
                query = query.Where(c => c.RangeLabel != null && c.RangeLabel == category);

            if (!string.IsNullOrEmpty(region))
                query = query.Where(c => c.Country != null && c.Country == region);

            return query.ToList();
        }
        public Competition GetByIdWithDetails(int id)
        {
            return _context.Competitions
                .Include(c => c.SubCompetition1s)
                    .ThenInclude(sc1 => sc1.AgeClassNavigation)
                .Include(c => c.SubCompetition1s)
                    .ThenInclude(sc1 => sc1.SubCompetition2s)
                        .ThenInclude(sc2 => sc2.IdCategoryNavigation)
                .FirstOrDefault(c => c.IdCompetition == id);
        }



    }
}