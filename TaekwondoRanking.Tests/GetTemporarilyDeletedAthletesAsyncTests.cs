using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.Services;
using TaekwondoRanking.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic; // Added for List

namespace TaekwondoRanking.Tests
{
    [TestFixture]
    public class GetTemporarilyDeletedAthletesAsyncTests
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

            // Clear static list before each test
            var keys = TemporaryDeletionManager.TemporarilyDeletedAthleteIds.Keys.ToList();
            foreach (var key in keys)
            {
                TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryRemove(key, out _);
            }
        }

        private void SeedDatabase()
        {
            var countryGer = new Country { IdCountry = "GER", NameCountry = "Germany" };
            _dbContext.Countries.Add(countryGer);

            var athlete1 = new Athlete { IdAthlete = "ATH_DEL1", Name = "Deleted User One", Country = "GER", CountryNavigation = countryGer };
            var athlete2 = new Athlete { IdAthlete = "ATH_ACT1", Name = "Active User One", Country = "GER", CountryNavigation = countryGer };
            var athlete3 = new Athlete { IdAthlete = "ATH_DEL2", Name = "Deleted User Two", Country = "GER", CountryNavigation = countryGer };
            _dbContext.Athletes.AddRange(athlete1, athlete2, athlete3);
            _dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public async Task GetTemporarilyDeletedAthletesAsync_NoAthletesMarkedDeleted_ReturnsEmptyList()
        {
            // Arrange (TemporaryDeletionManager is empty after Setup)

            // Act
            var result = await _athleteService.GetTemporarilyDeletedAthletesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result, "Expected an empty list when no athletes are temporarily deleted.");
        }

        [Test]
        public async Task GetTemporarilyDeletedAthletesAsync_OneAthleteMarkedDeleted_ReturnsThatOneAthlete()
        {
            // Arrange
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_DEL1", 0);

            // Act
            var result = await _athleteService.GetTemporarilyDeletedAthletesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count(), "Should return one deleted athlete.");
            Assert.AreEqual("ATH_DEL1", result.First().IdAthlete);
        }


        [Test]
        public async Task GetTemporarilyDeletedAthletesAsync_MultipleAthletesMarkedDeleted_ReturnsAllMarkedAthletes()
        {
            // Arrange
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_DEL1", 0);
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_DEL2", 0);
            // ATH_ACT1 is not added to the deleted list

            // Act
            var result = await _athleteService.GetTemporarilyDeletedAthletesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count(), "Should return two deleted athletes.");
            Assert.IsTrue(result.Any(a => a.IdAthlete == "ATH_DEL1"), "ATH_DEL1 should be in the list of deleted athletes.");
            Assert.IsTrue(result.Any(a => a.IdAthlete == "ATH_DEL2"), "ATH_DEL2 should be in the list of deleted athletes.");
        }

        [Test]
        public async Task GetTemporarilyDeletedAthletesAsync_MarkedAthleteIdNotInDatabase_DoesNotIncludeIt()
        {
            // Arrange
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_DEL1", 0); // Exists in DB
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("NON_EXISTENT", 0); // Does not exist in DB

            // Act
            var result = await _athleteService.GetTemporarilyDeletedAthletesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Count(), "Should only return athletes that exist in the database.");
            Assert.AreEqual("ATH_DEL1", result.First().IdAthlete);
        }

        [Test]
        public async Task GetTemporarilyDeletedAthletesAsync_AllAthletesMarkedDeleted_ReturnsAllAthletesFromSeed()
        {
            // Arrange
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_DEL1", 0);
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_ACT1", 0); //Marking the 'active' one as deleted for this test
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("ATH_DEL2", 0);

            // Act
            var result = await _athleteService.GetTemporarilyDeletedAthletesAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(3, result.Count(), "Should return all three seeded athletes as they are all marked deleted.");
            Assert.IsTrue(result.Any(a => a.IdAthlete == "ATH_DEL1"));
            Assert.IsTrue(result.Any(a => a.IdAthlete == "ATH_ACT1"));
            Assert.IsTrue(result.Any(a => a.IdAthlete == "ATH_DEL2"));
        }
    }
}