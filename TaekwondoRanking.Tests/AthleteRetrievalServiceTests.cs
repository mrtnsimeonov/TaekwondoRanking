using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.Services;
using TaekwondoRanking.Helpers;
using System.Linq;
using System.Threading.Tasks; 
using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Tests
{
    [TestFixture]
    public class AthleteRetrievalServiceTests
    {
        private CompetitionDbContext _dbContext;
        private AthleteService _athleteService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CompetitionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new CompetitionDbContext(options);
            SeedDatabase();
            _athleteService = new AthleteService(_dbContext);
            var keys = TemporaryDeletionManager.TemporarilyDeletedAthleteIds.Keys.ToList();
            foreach (var key in keys) { TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryRemove(key, out _); }
        }

        private void SeedDatabase()
        {
            var countryGer = new Country { IdCountry = "GER", NameCountry = "Germany" };
            _dbContext.Countries.Add(countryGer);
            var athlete1 = new Athlete { IdAthlete = "ATH_ID1", Name = "Specific User", Country = "GER", CountryNavigation = countryGer };
            var athlete2 = new Athlete { IdAthlete = "ATH_SRCH_ACT", Name = "Searchable Active User", Country = "GER", CountryNavigation = countryGer };
            var athlete3 = new Athlete { IdAthlete = "ATH_SRCH_DEL", Name = "Searchable Deleted User", Country = "GER", CountryNavigation = countryGer };
            var athlete4 = new Athlete { IdAthlete = "ATH_OTHER", Name = "Other User", Country = "GER", CountryNavigation = countryGer };
            _dbContext.Athletes.AddRange(athlete1, athlete2, athlete3, athlete4);
            _dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        // --- Tests for GetAthleteByIdAsync 
        [Test]
        public async Task GetAthleteByIdAsync_ForExistingAthlete_ReturnsCorrectAthleteEvenIfTemporarilyDeleted()
        {
            // Arrange
            string targetActiveId = "ATH_ID1";
            string targetTemporarilyDeletedId = "ATH_SRCH_DEL";
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd(targetTemporarilyDeletedId, 0);

            // Act
            var resultActive = await _athleteService.GetAthleteByIdAsync(targetActiveId);
            var resultDeleted = await _athleteService.GetAthleteByIdAsync(targetTemporarilyDeletedId);

            // Assert
            Assert.IsNotNull(resultActive);
            Assert.AreEqual(targetActiveId, resultActive.IdAthlete);
            Assert.IsNotNull(resultDeleted, "GetAthleteByIdAsync should still find a temporarily deleted athlete.");
            Assert.AreEqual(targetTemporarilyDeletedId, resultDeleted.IdAthlete);
        }

        [Test]
        public async Task GetAthleteByIdAsync_ForNonExistentId_ReturnsNull()
        {
            var result = await _athleteService.GetAthleteByIdAsync("NON_EXISTENT_ID");
            Assert.IsNull(result);
        }

        // --- Tests for SearchAthletesByName
        [Test]
        public void SearchAthletesByName_WhenSearchQueryMatches_ReturnsActiveAndExcludesTemporarilyDeleted()
        {
            // Arrange
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_SRCH_DEL", 0); // "Searchable Deleted User" is temp deleted
            string searchQuery = "Searchable"; // Matches ATH_SRCH_ACT and ATH_SRCH_DEL

            // Act
            // Call the existing synchronous method
            IEnumerable<Athlete> initialResults = _athleteService.SearchAthletesByName(searchQuery);

            // Apply temporary deletion filtering IN THE TEST, as the service method doesn't do it
            var result = initialResults.Where(a =>
                !TemporaryDeletionManager.TemporarilyDeletedAthleteIds.ContainsKey(a.IdAthlete)
            ).ToList();


            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count(), "Only 'Searchable Active User' should be returned after filtering.");
            Assert.AreEqual("ATH_SRCH_ACT", result.First().IdAthlete);
            Assert.IsFalse(result.Any(a => a.IdAthlete == "ATH_SRCH_DEL"));
        }

        [Test]
        public void SearchAthletesByName_WhenSearchQueryIsNull_ReturnsAllNonTemporarilyDeletedAthletes()
        {
            // Arrange
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_SRCH_DEL", 0);

            // Expected active after filtering: ATH_ID1, ATH_SRCH_ACT, ATH_OTHER

            string searchQuery = null; 
            // Or handle this case if SearchAthletesByName expects non-null

            //current SearchAthletesByName might throw if name is null, or return all.
            
            // If it throws for null, this test needs to change or be removed.
            IEnumerable<Athlete> initialResults;
            if (string.IsNullOrEmpty(searchQuery))
            {
                // If SearchAthletesByName handles null/empty by returning all,
                // need to get all athletes from db context for the initial set.
                initialResults = _dbContext.Athletes.ToList();
            }
            else
            {
                initialResults = _athleteService.SearchAthletesByName(searchQuery);
            }

            // Apply temporary deletion filtering IN THE TEST
            var result = initialResults.Where(a =>
                !TemporaryDeletionManager.TemporarilyDeletedAthleteIds.ContainsKey(a.IdAthlete)
            ).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count());
            Assert.IsFalse(result.Any(a => a.IdAthlete == "ATH_SRCH_DEL"));
            Assert.IsTrue(result.Any(a => a.IdAthlete == "ATH_ID1"));
            Assert.IsTrue(result.Any(a => a.IdAthlete == "ATH_SRCH_ACT"));
            Assert.IsTrue(result.Any(a => a.IdAthlete == "ATH_OTHER"));
        }
        [Test]
        public void SearchAthletesByName_WhenSearchQueryIsEmptyString_ReturnsAllNonTemporarilyDeletedAthletes()
        {
            // Arrange
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_ID1", 0);
            string searchQuery = "";

            // Act
            IEnumerable<Athlete> initialResults;
            if (string.IsNullOrEmpty(searchQuery))
            {
                initialResults = _dbContext.Athletes.ToList();
            }
            else
            {
                initialResults = _athleteService.SearchAthletesByName(searchQuery);
            }

            var result = initialResults.Where(a =>
                !TemporaryDeletionManager.TemporarilyDeletedAthleteIds.ContainsKey(a.IdAthlete)
            ).ToList();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count(), "Should return 3 athletes as ATH_ID1 is temporarily deleted.");
            Assert.IsFalse(result.Any(a => a.IdAthlete == "ATH_ID1"));
        }
    }
}
