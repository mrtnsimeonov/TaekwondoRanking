using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using TaekwondoRanking.Models;
using TaekwondoRanking.Services;
using TaekwondoRanking.ViewModels;
using TaekwondoRanking.Helpers;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace TaekwondoRanking.Tests
{
    [TestFixture]
    public class CountryRankingFilterTests
    {
        private CompetitionDbContext _dbContext;
        private RegionService _regionService; // Still using RegionService for these methods

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<CompetitionDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new CompetitionDbContext(options);
            SeedDatabase();
            _regionService = new RegionService(_dbContext);
            var keys = TemporaryDeletionManager.TemporarilyDeletedAthleteIds.Keys.ToList();
            foreach (var key in keys) { TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryRemove(key, out _); }
        }

        private void SeedDatabase()
        {
            // Same seed as WorldRanking for consistency, can be trimmed if specific data needed
            var countryGer = new Country { IdCountry = "GER", NameCountry = "Germany" };
            var countryKor = new Country { IdCountry = "KOR", NameCountry = "Korea" };
            var countryUsa = new Country { IdCountry = "USA", NameCountry = "USA" };
            _dbContext.Countries.AddRange(countryGer, countryKor, countryUsa);

            var athleteGer1 = new Athlete { IdAthlete = "CTY_GER1", Name = "Hans Zimmer", Country = "GER", CountryNavigation = countryGer }; // 30
            var athleteKor1 = new Athlete { IdAthlete = "CTY_KOR1", Name = "Park Chan-wook", Country = "KOR", CountryNavigation = countryKor }; // 25
            var athleteGer2 = new Athlete { IdAthlete = "CTY_GER2", Name = "Diane Kruger", Country = "GER", CountryNavigation = countryGer }; // 15
            var athleteUsa1 = new Athlete { IdAthlete = "CTY_USA1", Name = "Tom Hanks", Country = "USA", CountryNavigation = countryUsa }; // 10 (to be deleted)
            _dbContext.Athletes.AddRange(athleteGer1, athleteKor1, athleteGer2, athleteUsa1);

            var ageSenior = new AgeClass { IdAgeClass = "SEN", NameAgeClass = "Senior" };
            _dbContext.AgeClasses.Add(ageSenior);
            var catSeniorMaleO80 = new Category { IdCategory = "SENMO80", AgeClass = "SEN", Mf = "Male", NameCategory = "+80kg", AgeClassNavigation = ageSenior };
            _dbContext.Categories.Add(catSeniorMaleO80);

            var competitionGlobal = new Competition { IdCompetition = 20, NameCompetition = "Global Championship", Country = "USA", CountryNavigation = countryUsa };
            _dbContext.Competitions.Add(competitionGlobal);
            var subComp1Senior = new SubCompetition1 { IdSubCompetition1 = 30, IdCompetition = 20, AgeClass = "SEN", AgeClassNavigation = ageSenior, IdCompetitionNavigation = competitionGlobal };
            _dbContext.SubCompetition1s.Add(subComp1Senior);
            var subComp2SeniorMaleO80 = new SubCompetition2 { IdSubCompetition2 = 301, IdSubCompetition1 = 30, IdCategory = "SENMO80", IdCategoryNavigation = catSeniorMaleO80, IdSubCompetition1Navigation = subComp1Senior };
            _dbContext.SubCompetition2s.Add(subComp2SeniorMaleO80);

            _dbContext.Results.AddRange(
                new Result { IdAthlete = "CTY_GER1", IdSubCompetition2 = 301, Points = 30, IdAthleteNavigation = athleteGer1, IdSubCompetition2Navigation = subComp2SeniorMaleO80 },
                new Result { IdAthlete = "CTY_KOR1", IdSubCompetition2 = 301, Points = 25, IdAthleteNavigation = athleteKor1, IdSubCompetition2Navigation = subComp2SeniorMaleO80 },
                new Result { IdAthlete = "CTY_GER2", IdSubCompetition2 = 301, Points = 15, IdAthleteNavigation = athleteGer2, IdSubCompetition2Navigation = subComp2SeniorMaleO80 },
                new Result { IdAthlete = "CTY_USA1", IdSubCompetition2 = 301, Points = 10, IdAthleteNavigation = athleteUsa1, IdSubCompetition2Navigation = subComp2SeniorMaleO80 }
            );
            _dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown() { _dbContext.Database.EnsureDeleted(); _dbContext.Dispose(); }

        [Test]
        public async Task ApplyCountryRankingFilters_NoFilters_ReturnsAllActiveAthletes()
        {
            var model = await _regionService.BuildInitialCountryRankingModelAsync(); // Gets dropdowns
            var resultViewModel = await _regionService.ApplyCountryRankingFiltersAsync(model, null, null);

            Assert.IsNotNull(resultViewModel.Results);
            Assert.AreEqual(4, resultViewModel.Results.Count);
            Assert.AreEqual("CTY_GER1", resultViewModel.Results.ElementAt(0).IdAthlete); // Hans 30
        }

        [Test]
        public async Task ApplyCountryRankingFilters_FilterBySpecificCountry_ReturnsOnlyAthletesFromThatCountry()
        {
            var model = await _regionService.BuildInitialCountryRankingModelAsync();
            model.SelectedCountry = "Germany";
            var resultViewModel = await _regionService.ApplyCountryRankingFiltersAsync(model, null, null);

            Assert.IsNotNull(resultViewModel.Results);
            Assert.AreEqual(2, resultViewModel.Results.Count, "Should only be athletes from Germany.");
            Assert.IsTrue(resultViewModel.Results.All(r => r.Country == "Germany"));
            Assert.IsTrue(resultViewModel.Results.Any(r => r.IdAthlete == "CTY_GER1"));
            Assert.IsTrue(resultViewModel.Results.Any(r => r.IdAthlete == "CTY_GER2"));
        }

        [Test]
        public async Task ApplyCountryRankingFilters_FilterByCountryWithNoAthletes_ReturnsEmptyResults()
        {
            var model = await _regionService.BuildInitialCountryRankingModelAsync();
            // Add a country with no athletes for this test if not already seeded
            var countryFra = new Country { IdCountry = "FRA", NameCountry = "France" };
            if (!_dbContext.Countries.Any(c => c.IdCountry == "FRA"))
            {
                _dbContext.Countries.Add(countryFra);
                _dbContext.SaveChanges();
            }
            model.Countries = await _dbContext.Countries.Select(c => c.NameCountry).ToListAsync(); // Update model

            model.SelectedCountry = "France";
            var resultViewModel = await _regionService.ApplyCountryRankingFiltersAsync(model, null, null);

            Assert.IsNotNull(resultViewModel.Results);
            Assert.IsEmpty(resultViewModel.Results, "Should be no athletes for France.");
        }

        [Test]
        public async Task ApplyCountryRankingFilters_TemporarilyDeletedAthleteInSelectedCountry_IsExcluded()
        {
            var model = await _regionService.BuildInitialCountryRankingModelAsync();
            model.SelectedCountry = "USA";
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("CTY_USA1", 0); // Tom Hanks is deleted

            var resultViewModel = await _regionService.ApplyCountryRankingFiltersAsync(model, null, null);

            Assert.IsNotNull(resultViewModel.Results);
            Assert.IsEmpty(resultViewModel.Results, "Tom Hanks was the only USA athlete and is deleted.");
        }

        [Test]
        public async Task ApplyCountryRankingFilters_CombineCountryAndCategoryFilters_ReturnsCorrectIntersection()
        {
            var model = await _regionService.BuildInitialCountryRankingModelAsync();
            model.SelectedCountry = "Germany";
            model.SelectedAgeClass = "SEN"; // All seeded are Senior for this test
            model.SelectedGender = "Male";
            model.SelectedCategory = "+80kg";
            // Simulate UI populating dropdowns
            model.AgeClasses = new List<string> { "SEN" };
            model.Genders = new List<string> { "Male" };
            model.Categories = new List<string> { "+80kg" };


            var resultViewModel = await _regionService.ApplyCountryRankingFiltersAsync(model, null, null);

            Assert.IsNotNull(resultViewModel.Results);
            // Hans (GER1) and Diane (GER2) are German, SEN, Male, +80kg
            Assert.AreEqual(2, resultViewModel.Results.Count);
            Assert.IsTrue(resultViewModel.Results.Any(r => r.IdAthlete == "CTY_GER1"));
            Assert.IsTrue(resultViewModel.Results.Any(r => r.IdAthlete == "CTY_GER2"));
        }
    }
}