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
    public class WorldRankingFilterTests
    {
        private CompetitionDbContext _dbContext;
        private RegionService _regionService;

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
            var countryGer = new Country { IdCountry = "GER", NameCountry = "Germany" };
            var countryKor = new Country { IdCountry = "KOR", NameCountry = "Korea" };
            _dbContext.Countries.AddRange(countryGer, countryKor);

            var athlete1 = new Athlete { IdAthlete = "WRLD001", Name = "Walter White", Country = "GER", CountryNavigation = countryGer }; // 30 pts
            var athlete2 = new Athlete { IdAthlete = "WRLD002", Name = "Jesse Pinkman", Country = "KOR", CountryNavigation = countryKor }; // 25 pts
            var athlete3 = new Athlete { IdAthlete = "WRLD003", Name = "Saul Goodman", Country = "GER", CountryNavigation = countryGer }; // 15 pts
            var athlete4 = new Athlete { IdAthlete = "WRLD004", Name = "Gus Fring", Country = "KOR", CountryNavigation = countryKor };    // 5 pts (to be deleted)
            _dbContext.Athletes.AddRange(athlete1, athlete2, athlete3, athlete4);

            var ageCadet = new AgeClass { IdAgeClass = "CAD", NameAgeClass = "Cadet" };
            _dbContext.AgeClasses.Add(ageCadet);

            var catCadMaleU55 = new Category { IdCategory = "CADMU55", AgeClass = "CAD", Mf = "Male", NameCategory = "-55kg", AgeClassNavigation = ageCadet };
            var catCadFemaleU50 = new Category { IdCategory = "CADFU50", AgeClass = "CAD", Mf = "Female", NameCategory = "-50kg", AgeClassNavigation = ageCadet };
            _dbContext.Categories.AddRange(catCadMaleU55, catCadFemaleU50);

            var competition1 = new Competition { IdCompetition = 10, NameCompetition = "World Open", Country = "GER", CountryNavigation = countryGer };
            _dbContext.Competitions.Add(competition1);

            var subComp1Cad = new SubCompetition1 { IdSubCompetition1 = 20, IdCompetition = 10, AgeClass = "CAD", AgeClassNavigation = ageCadet, IdCompetitionNavigation = competition1 };
            _dbContext.SubCompetition1s.Add(subComp1Cad);

            var subComp2Male = new SubCompetition2 { IdSubCompetition2 = 201, IdSubCompetition1 = 20, IdCategory = "CADMU55", IdCategoryNavigation = catCadMaleU55, IdSubCompetition1Navigation = subComp1Cad };
            var subComp2Female = new SubCompetition2 { IdSubCompetition2 = 202, IdSubCompetition1 = 20, IdCategory = "CADFU50", IdCategoryNavigation = catCadFemaleU50, IdSubCompetition1Navigation = subComp1Cad };
            _dbContext.SubCompetition2s.AddRange(subComp2Male, subComp2Female);

            _dbContext.Results.AddRange(
                new Result { IdAthlete = "WRLD001", IdSubCompetition2 = 201, Points = 20, IdAthleteNavigation = athlete1, IdSubCompetition2Navigation = subComp2Male },
                new Result { IdAthlete = "WRLD001", IdSubCompetition2 = 201, Points = 10, IdAthleteNavigation = athlete1, IdSubCompetition2Navigation = subComp2Male },
                new Result { IdAthlete = "WRLD002", IdSubCompetition2 = 201, Points = 25, IdAthleteNavigation = athlete2, IdSubCompetition2Navigation = subComp2Male },
                new Result { IdAthlete = "WRLD003", IdSubCompetition2 = 202, Points = 15, IdAthleteNavigation = athlete3, IdSubCompetition2Navigation = subComp2Female },
                new Result { IdAthlete = "WRLD004", IdSubCompetition2 = 202, Points = 5, IdAthleteNavigation = athlete4, IdSubCompetition2Navigation = subComp2Female }
            );
            _dbContext.SaveChanges();
        }

        [TearDown]
        public void TearDown() { _dbContext.Database.EnsureDeleted(); _dbContext.Dispose(); }

        [Test]
        public async Task ApplyWorldRankingFilters_NoFilters_ReturnsAllActiveAthletesSorted()
        {
            var model = await _regionService.BuildInitialWorldRankingModelAsync();
            var resultViewModel = await _regionService.ApplyWorldRankingFiltersAsync(model, null, null);

            Assert.IsNotNull(resultViewModel.Results);
            Assert.AreEqual(4, resultViewModel.Results.Count);
            Assert.AreEqual("WRLD001", resultViewModel.Results.ElementAt(0).IdAthlete); // Walter 30
            Assert.AreEqual("WRLD002", resultViewModel.Results.ElementAt(1).IdAthlete); // Jesse 25
        }

        [Test]
        public async Task ApplyWorldRankingFilters_WithValidSearchQuery_ReturnsMatchingAthletes()
        {
            var model = await _regionService.BuildInitialWorldRankingModelAsync();
            model.SearchQuery = "Goodman"; // Saul
            var resultViewModel = await _regionService.ApplyWorldRankingFiltersAsync(model, null, model.SearchQuery);

            Assert.IsNotNull(resultViewModel.Results);
            Assert.AreEqual(1, resultViewModel.Results.Count);
            Assert.AreEqual("WRLD003", resultViewModel.Results.First().IdAthlete);
        }

        [Test]
        public async Task ApplyWorldRankingFilters_WithSpecificCategory_ReturnsCorrectSubset()
        {
            var model = await _regionService.BuildInitialWorldRankingModelAsync();
            model.SelectedAgeClass = "CAD";
            model.SelectedGender = "Male";
            model.SelectedCategory = "-55kg";
            model.Categories = new List<string> { "-55kg" }; // Simulate UI populating this

            var resultViewModel = await _regionService.ApplyWorldRankingFiltersAsync(model, null, null);

            Assert.IsNotNull(resultViewModel.Results);
            Assert.AreEqual(2, resultViewModel.Results.Count, "Walter and Jesse are CAD Male -55kg");
            Assert.IsTrue(resultViewModel.Results.Any(r => r.IdAthlete == "WRLD001"));
            Assert.IsTrue(resultViewModel.Results.Any(r => r.IdAthlete == "WRLD002"));
        }

        [Test]
        public async Task ApplyWorldRankingFilters_TemporarilyDeletedAthlete_IsExcluded()
        {
            var model = await _regionService.BuildInitialWorldRankingModelAsync();
            TemporaryDeletionManager.TemporarilyDeletedAthleteIds.TryAdd("WRLD004", 0); // Gus is deleted

            var resultViewModel = await _regionService.ApplyWorldRankingFiltersAsync(model, null, null);

            Assert.IsNotNull(resultViewModel.Results);
            Assert.AreEqual(3, resultViewModel.Results.Count, "Gus should be excluded.");
            Assert.IsFalse(resultViewModel.Results.Any(r => r.IdAthlete == "WRLD004"));
        }

        [Test]
        public async Task ApplyWorldRankingFilters_ResetButton_ClearsFiltersAndResults()
        {
            var model = await _regionService.BuildInitialWorldRankingModelAsync();
            model.SearchQuery = "Walter";
            model = await _regionService.ApplyWorldRankingFiltersAsync(model, null, model.SearchQuery);
            Assert.IsNotEmpty(model.Results, "Results should not be empty before reset.");

            var resetViewModel = await _regionService.ApplyWorldRankingFiltersAsync(model, "true", null);

            Assert.IsEmpty(resetViewModel.SearchQuery);
            Assert.IsEmpty(resetViewModel.Results);
        }
    }
}
