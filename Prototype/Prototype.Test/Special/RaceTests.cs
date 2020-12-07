using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using Moq;
using SpecialElection.Data.Model;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Prototype.Data;
using SpecialElection.Areas.Special.Pages.PrecinctView;
using Prototype.Test.Utility;

namespace Prototype.Test.Special
{
    [TestFixture]
    public class RaceTests : BaseTest
    {
        private async Task ValidateDefaultRaces()
        {
            List<Race> races = await applicationDbService.GetRaces().OrderBy(x => x.Id).ToListAsync();
            Assert.IsTrue(races.Count == 2);
            ValidateRace1(races[0]);
            ValidateRace2(races[1]);
        }
        private void ValidateRace1(Race race)
        {
            Assert.IsTrue(race.Id == 1);
            Assert.IsTrue(race.Name == TestData.Race1Name);
            Assert.IsTrue(race.Type == TestData.Race1Type);
            Assert.IsTrue(race.District == TestData.Race1District);
            Assert.IsTrue(race.Description == TestData.Race1Description);
            Assert.IsTrue(race.Locked == TestData.Race1Locked);
        }
        private void ValidateRace2(Race race)
        {
            Assert.IsTrue(race.Id == 2);
            Assert.IsTrue(race.Name == TestData.Race2Name);
            Assert.IsTrue(race.Type == TestData.Race2Type);
            Assert.IsTrue(race.District == TestData.Race2District);
            Assert.IsTrue(race.Description == TestData.Race2Description);
            Assert.IsTrue(race.Locked == TestData.Race2Locked);
        }
        
        [TestCase("adsfaewggs", RaceTypeEnum.StateAssembly, 22, "jbibigifdjfkdfjdkfjasfa3w4a", false)]
        [TestCase("ggggg", RaceTypeEnum.StateSenate, 13, "dfdddd", false)]
        public async Task CreateRace(String raceName, RaceTypeEnum raceType, int raceDistrict, String raceDescription, Boolean raceLocked)
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await SpecialElectionTestUtility.CreateRaceAction(applicationDbService, new Race() { 
                Name = raceName,
                Type = raceType,
                District = raceDistrict,
                Description = raceDescription,
                Locked = raceLocked,
                ElectionId = 1
            });
        }

        [Test]
        public async Task CreateRaceWithoutElection() 
        {
            SpecialElection.Areas.Special.Pages.RaceView.CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.RaceView.CreateModel(applicationDbService); });

            List<Race> races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 0);

            await pageModel.Call<NotFoundResult>(delegate () { 
                return pageModel.OnGetAsync(1); 
            });
            races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 0);


            await pageModel.Call<NotFoundResult>(delegate () {
                return pageModel.OnGetAsync(null);
            });
            races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 0);

            String raceName = "adsfaewggs";
            RaceTypeEnum raceType = RaceTypeEnum.StateAssembly;
            int raceDistrict = 22;
            String raceDescription = "jbibigifdjfkdfjdkfjasfa3w4a";
            Boolean raceLocked = false;

            pageModel.Race = new Race()
            {
                Name = raceName,
                Type = raceType,
                District = raceDistrict,
                Description = raceDescription,
                Locked = raceLocked
            };

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Race);

            races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 0);
        }

        [Test]
        public async Task CreateRaceWithInvalidData()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            SpecialElection.Areas.Special.Pages.RaceView.CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.RaceView.CreateModel(applicationDbService); });

            List<Race> races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 0);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(1); });

            String raceName = "adsfaewggs";
            RaceTypeEnum raceType = RaceTypeEnum.StateAssembly;
            int raceDistrict = 22;
            String raceDescription = "jbibigifdjfkdfjdkfjasfa3w4a";
            Boolean raceLocked = false;

            pageModel.Race = new Race()
            {
                Name = raceName,
                Type = raceType,
                District = -10,
                Description = raceDescription,
                Locked = raceLocked
            };

            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Race);

            races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 0);


            pageModel.Race = new Race()
            {
                Name = null,
                Type = raceType,
                District = raceDistrict,
                Description = raceDescription,
                Locked = raceLocked
            };

            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Race);

            races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 0);
        }

        [Test]
        public async Task EditRace()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);
            await ValidateDefaultRaces();

            SpecialElection.Areas.Special.Pages.RaceView.EditModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.RaceView.EditModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate() { return pageModel.OnGetAsync(1); });

            ValidateRace1(pageModel.Race);

            String newName = "djbuf83in56naoisdvuppaisdufoiajdirnpaw";
            RaceTypeEnum newType = RaceTypeEnum.StateAssembly;
            int newDistrict = 55;
            String newDescription = "hgiaiididididididienka;sdnfklanvisaphgoiahtewihwiaojp";
            Boolean newLock = true;

            pageModel.Race.Name = newName;
            pageModel.Race.Type = newType;
            pageModel.Race.District = newDistrict;
            pageModel.Race.Description = newDescription;
            pageModel.Race.Locked = newLock;

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Race);

            List<Race> races = await applicationDbService.GetRaces().OrderBy(x => x.Id).ToListAsync();
            Assert.IsTrue(races.Count == 2);
            ValidateRace2(races[1]);

            Race race = races[0];
            Assert.IsTrue(race.Id == 1);
            Assert.IsTrue(race.Name == newName);
            Assert.IsTrue(race.Type == newType);
            Assert.IsTrue(race.District == newDistrict);
            Assert.IsTrue(race.Description == newDescription);
            Assert.IsTrue(race.Locked == newLock);

            List<CountyTypeEnum> counties = District.DistrictInfo[race.Type][race.District].OrderBy(x => (int)x).ToList();
            List<RaceCountyData> raceCountyDataList = race.RaceCountyDataList.OrderBy(x => (int)x.County).ToList();

            Assert.IsTrue(counties.Count.Equals(raceCountyDataList.Count));

            for (int i = 0; i < counties.Count; i++)
            {
                Assert.IsTrue(counties[i].Equals(raceCountyDataList[i].County));
            }

        }
        [Test]
        public async Task EditInvalidDataRace()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);
            await ValidateDefaultRaces();

            SpecialElection.Areas.Special.Pages.RaceView.EditModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.RaceView.EditModel(applicationDbService); });

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(666999); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(null); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(444); });

            await ValidateDefaultRaces();

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(2); });
            ValidateRace2(pageModel.Race);
            pageModel.Race.Name = null;
            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Race);
            await ValidateDefaultRaces();

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(2); });
            ValidateRace2(pageModel.Race);
            pageModel.Race.Name = "awertjbsneawertjbsnedgetyawertjawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetybsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgety";
            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Race);
            await ValidateDefaultRaces();

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(2); });
            ValidateRace2(pageModel.Race);
            pageModel.Race.District = 99999;
            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Race);
            await ValidateDefaultRaces();

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(2); });
            ValidateRace2(pageModel.Race);
            pageModel.Race.Description = "awertjbsneawertjbsnedgetyawertjawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetybsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgetyawertjbsneawertjbsnedgety";
            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Race);
            await ValidateDefaultRaces();

        }

        [Test]
        public async Task DeleteRace()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);
            await ValidateDefaultRaces();

            SpecialElection.Areas.Special.Pages.ElectionView.IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.ElectionView.IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteRaceAsync(1); });

            List<Race> races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 1);
            ValidateRace2(races[0]);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteRaceAsync(2); });

            races = await applicationDbService.GetRaces().ToListAsync();
            Assert.IsTrue(races.Count == 0);
        }
        [Test]
        public async Task DeleteInvalidIdRace()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);

            await ValidateDefaultRaces();

            SpecialElection.Areas.Special.Pages.ElectionView.IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.ElectionView.IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteRaceAsync(585858); });

            await ValidateDefaultRaces();
        }

        [Test]
        public async Task ViewRace()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);
            await ValidateDefaultRaces();

            SpecialElection.Areas.Special.Pages.ElectionView.IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.ElectionView.IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);
            Assert.IsTrue(pageModel.Election.Count == 1);
            Assert.IsTrue(pageModel.Election[0].Races.Count == 2);
            ValidateRace1(pageModel.Election[0].Races.ToList().FirstOrDefault(x => x.Id.Equals(TestData.Race1Id)));
            ValidateRace2(pageModel.Election[0].Races.ToList().FirstOrDefault(x => x.Id.Equals(TestData.Race2Id)));
        }

        [Test]
        public async Task EditPrecinct()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);
            await ValidateDefaultRaces();


            EditRacePrecinctsModel pageModel = TestUtility.InitPageModel(delegate () { return new EditRacePrecinctsModel(applicationDbService); });

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(null); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(99999); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(454545); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race2Id); });

            pageModel.RaceCountyData.ForEach(x => {
                Assert.IsTrue(x.NumberOfPrecinct == 0); 
                Assert.IsTrue(x.PrecinctsReporting == 0);

                x.NumberOfPrecinct = -1;
            });

            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.RaceCountyData.ToArray());
            await applicationDbService.GetRaceCountyData().Where(x => x.RaceId.Equals(TestData.Race2Id)).ForEachAsync(x => {
                Assert.IsTrue(x.NumberOfPrecinct == 0);
                Assert.IsTrue(x.PrecinctsReporting == 0);
                Assert.IsTrue(x.CandidateResults.Count == 0);
            });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race2Id); });
            pageModel.RaceCountyData.ForEach(x => {
                Assert.IsTrue(x.NumberOfPrecinct == 0);
                Assert.IsTrue(x.PrecinctsReporting == 0);
                Assert.IsTrue(x.CandidateResults.Count == 0);

                x.NumberOfPrecinct = 10;
                x.PrecinctsReporting = 888888;
            });

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.RaceCountyData.ToArray());
            await applicationDbService.GetRaceCountyData().Where(x => x.RaceId.Equals(TestData.Race2Id)).ForEachAsync(x => {
                Assert.IsTrue(x.NumberOfPrecinct == 10);
                Assert.IsTrue(x.PrecinctsReporting == 0);
                Assert.IsTrue(x.CandidateResults.Count == 0);
            });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race2Id); });
            pageModel.RaceCountyData.ForEach(x => {
                Assert.IsTrue(x.NumberOfPrecinct == 10);
                Assert.IsTrue(x.PrecinctsReporting == 0);
                Assert.IsTrue(x.CandidateResults.Count == 0);

                x.CandidateResults.Add( new CandidateResult() {
                    RaceCountyDataId = x.Id,
                    CandidateId = 231,
                    Votes = 2510,
                    Percent = 2.00
                });
            });

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.RaceCountyData.ToArray());
            await applicationDbService.GetRaceCountyData().Where(x => x.RaceId.Equals(TestData.Race2Id)).ForEachAsync(x => {
                Assert.IsTrue(x.NumberOfPrecinct == 10);
                Assert.IsTrue(x.PrecinctsReporting == 0);
                Assert.IsTrue(x.CandidateResults.Count == 0);
            });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race2Id); });
            pageModel.RaceCountyData.ForEach(x => {
                Assert.IsTrue(x.NumberOfPrecinct == 10);
                Assert.IsTrue(x.PrecinctsReporting == 0);
                Assert.IsTrue(x.CandidateResults.Count == 0);
            });
        }
    }
}
