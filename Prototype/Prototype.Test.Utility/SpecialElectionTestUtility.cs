using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpecialElection.Data;
using SpecialElection.Data.Model;
using System.Linq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Prototype.Test.Utility;
using Prototype.Data;
using SpecialElection.Areas.Special.Pages;
using SpecialElection.Areas.Special.Pages.PrecinctView;
using SpecialElection.Areas.Special.Pages.ResultView;
using SpecialElection.Areas.Special.Pages.ElectionView;
using SpecialElection.Areas.Special.Pages.RaceView;


namespace Prototype.Test.Utility
{
    public static class SpecialElectionTestUtility
    {

        public static ApplicationDbContext CreateTestApplcationDbContext()
        {
            // Create a new service provider to create a new in-memory database.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance using an in-memory database and 
            // IServiceProvider that the context should resolve all of its 
            // services from.
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("myDataBaseName")
                .UseInternalServiceProvider(serviceProvider);

            return new ApplicationDbContext(builder.Options);
        }

        public static async Task CreateElectionAction(ApplicationDbService applicationDbService, String electionName, DateTime electionDate, Boolean isActive)
        {
            SpecialElection.Areas.Special.Pages.ElectionView.CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.ElectionView.CreateModel(applicationDbService); });

            List<Election> elections = await applicationDbService.GetElection().ToListAsync();
            int prevCount = elections.Count;

            pageModel.Election = new SpecialElection.Areas.Special.Pages.ElectionView.CreateModel.InputModel
            {
                Name = electionName,
                ElectionDate = electionDate,
                IsActive = isActive
            };

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Election);

            elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == (prevCount + 1));

            Election election = elections[elections.Count - 1];
            Assert.IsTrue(election.Name.Equals(electionName));
            Assert.IsTrue(election.ElectionDate.Equals(electionDate));
            Assert.IsTrue(election.CreatedBy.Equals("Fixme"));
            Assert.IsTrue(election.ModifyBy.Equals("Fixme"));
            Assert.IsTrue(election.IsActive == isActive);

        }
        public static async Task PopulateWithElectionData(ApplicationDbService ApplicationDbService)
        {
            await CreateElectionAction(ApplicationDbService, TestData.ElectionName, TestData.ElectionDate, TestData.ElectionIsActive);
        }


        public static async Task CreateRaceAction(ApplicationDbService applicationDbService, Race inputRace)
        {
            SpecialElection.Areas.Special.Pages.RaceView.CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.RaceView.CreateModel(applicationDbService); });

            List<Race> races = await applicationDbService.GetRaces().ToListAsync();
            int prevCount = races.Count;

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(inputRace.ElectionId); });

            pageModel.Race = inputRace;

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Race);

            races = await applicationDbService.GetRaces().OrderBy(x => x.Id).Include(x => x.RaceCountyDataList).ToListAsync();
            Assert.IsTrue(races.Count == (prevCount + 1));

            Race race = races.FirstOrDefault(r => r.Id.Equals(inputRace.Id));
            Assert.IsTrue(race != null);

            Assert.IsTrue(race.Name == inputRace.Name);
            Assert.IsTrue(race.Type == inputRace.Type);
            Assert.IsTrue(race.District == inputRace.District);
            Assert.IsTrue(race.Description == inputRace.Description);
            Assert.IsTrue(race.Locked == inputRace.Locked);


            List<CountyTypeEnum> counties = District.DistrictInfo[race.Type][race.District].OrderBy(x => (int)x).ToList();
            List<RaceCountyData> raceCountyDataList = race.RaceCountyDataList.OrderBy(x => (int)x.County).ToList();

            Assert.IsTrue(counties.Count.Equals(raceCountyDataList.Count));

            for (int i = 0; i < counties.Count; i++)
            {
                Assert.IsTrue(counties[i].Equals(raceCountyDataList[i].County));
            }
        }
        public static async Task PopulateWithRaceData(ApplicationDbService applicationDbService)
        {
            await PopulateWithElectionData(applicationDbService);

            Election election = applicationDbService.GetElection().FirstOrDefault();

            await CreateRaceAction(applicationDbService, new Race()
            {
                Id = TestData.Race2Id,
                Name = TestData.Race2Name,
                Type = TestData.Race2Type,
                District = TestData.Race2District,
                Description = TestData.Race2Description,
                Locked = TestData.Race2Locked,
                ElectionId = election.Id
            });

            await CreateRaceAction(applicationDbService, new Race()
            {
                Id = TestData.Race1Id,
                Name = TestData.Race1Name,
                Type = TestData.Race1Type,
                District = TestData.Race1District,
                Description = TestData.Race1Description,
                Locked = TestData.Race1Locked,
                ElectionId = election.Id
            });
        }


        public static async Task CreateCandidateAction(ApplicationDbService applicationDbService, Candidate inputCandidate)
        {
            SpecialElection.Areas.Special.Pages.CandidateView.CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.CandidateView.CreateModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            int prevCount = candidates.Count;

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(inputCandidate.RaceId); });

            pageModel.Candidate = inputCandidate;

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Candidate);
        }
        public static async Task PopulateWithCandidateData(ApplicationDbService applicationDbService)
        {
            await PopulateWithRaceData(applicationDbService);

            List<Candidate> candidateSeeds = new List<Candidate>() {
                
                #region Senate Race Candidates    
                new Candidate(){
                    RaceId = TestData.Race1Id,
                    Id = TestData.Candidate218Id,
                    LastName = TestData.Candidate218LastName,
                    FirstName = TestData.Candidate218FirstName,
                    MiddleName = TestData.Candidate218MiddleName,
                    DisplayName = TestData.Candidate218DisplayName,
                    DisplayOrder = TestData.Candidate218Displayorder,
                    Party = TestData.Candidate218Party
                },
                new Candidate(){
                    RaceId = TestData.Race1Id,
                    Id = TestData.Candidate217Id,
                    LastName = TestData.Candidate217LastName,
                    FirstName = TestData.Candidate217FirstName,
                    MiddleName = TestData.Candidate217MiddleName,
                    DisplayName = TestData.Candidate217DisplayName,
                    DisplayOrder = TestData.Candidate217Displayorder,
                    Party = TestData.Candidate217Party
                },
                new Candidate(){
                    RaceId = TestData.Race1Id,
                    Id = TestData.Candidate216Id,
                    LastName = TestData.Candidate216LastName,
                    FirstName = TestData.Candidate216FirstName,
                    MiddleName = TestData.Candidate216MiddleName,
                    DisplayName = TestData.Candidate216DisplayName,
                    DisplayOrder = TestData.Candidate216Displayorder,
                    Party = TestData.Candidate216Party
                },
                new Candidate(){
                    RaceId = TestData.Race1Id,
                    Id = TestData.Candidate215Id,
                    LastName = TestData.Candidate215LastName,
                    FirstName = TestData.Candidate215FirstName,
                    MiddleName = TestData.Candidate215MiddleName,
                    DisplayName = TestData.Candidate215DisplayName,
                    DisplayOrder = TestData.Candidate215Displayorder,
                    Party = TestData.Candidate215Party
                },

                new Candidate(){
                    RaceId = TestData.Race1Id,
                    Id = TestData.Candidate214Id,
                    LastName = TestData.Candidate214LastName,
                    FirstName = TestData.Candidate214FirstName,
                    MiddleName = TestData.Candidate214MiddleName,
                    DisplayName = TestData.Candidate214DisplayName,
                    DisplayOrder = TestData.Candidate214Displayorder,
                    Party = TestData.Candidate214Party
                },
                
                #endregion Senate Race Candidates
            
                #region House of Representatives Candidates
                
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate236Id,
                    LastName = TestData.Candidate236LastName,
                    FirstName = TestData.Candidate236FirstName,
                    MiddleName = TestData.Candidate236MiddleName,
                    DisplayName = TestData.Candidate236DisplayName,
                    DisplayOrder = TestData.Candidate236Displayorder,
                    Party = TestData.Candidate236Party
                },
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate235Id,
                    LastName = TestData.Candidate235LastName,
                    FirstName = TestData.Candidate235FirstName,
                    MiddleName = TestData.Candidate235MiddleName,
                    DisplayName = TestData.Candidate235DisplayName,
                    DisplayOrder = TestData.Candidate235Displayorder,
                    Party = TestData.Candidate235Party
                },
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate234Id,
                    LastName = TestData.Candidate234LastName,
                    FirstName = TestData.Candidate234FirstName,
                    MiddleName = TestData.Candidate234MiddleName,
                    DisplayName = TestData.Candidate234DisplayName,
                    DisplayOrder = TestData.Candidate234Displayorder,
                    Party = TestData.Candidate234Party
                },

                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate233Id,
                    LastName = TestData.Candidate233LastName,
                    FirstName = TestData.Candidate233FirstName,
                    MiddleName = TestData.Candidate233MiddleName,
                    DisplayName = TestData.Candidate233DisplayName,
                    DisplayOrder = TestData.Candidate233Displayorder,
                    Party = TestData.Candidate233Party
                },
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate232Id,
                    LastName = TestData.Candidate232LastName,
                    FirstName = TestData.Candidate232FirstName,
                    MiddleName = TestData.Candidate232MiddleName,
                    DisplayName = TestData.Candidate232DisplayName,
                    DisplayOrder = TestData.Candidate232Displayorder,
                    Party = TestData.Candidate232Party
                },
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate231Id,
                    LastName = TestData.Candidate231LastName,
                    FirstName = TestData.Candidate231FirstName,
                    MiddleName = TestData.Candidate231MiddleName,
                    DisplayName = TestData.Candidate231DisplayName,
                    DisplayOrder = TestData.Candidate231Displayorder,
                    Party = TestData.Candidate231Party
                },

                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate242Id,
                    LastName = TestData.Candidate242LastName,
                    FirstName = TestData.Candidate242FirstName,
                    MiddleName = TestData.Candidate242MiddleName,
                    DisplayName = TestData.Candidate242DisplayName,
                    DisplayOrder = TestData.Candidate242Displayorder,
                    Party = TestData.Candidate242Party
                },
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate241Id,
                    LastName = TestData.Candidate241LastName,
                    FirstName = TestData.Candidate241FirstName,
                    MiddleName = TestData.Candidate241MiddleName,
                    DisplayName = TestData.Candidate241DisplayName,
                    DisplayOrder = TestData.Candidate241Displayorder,
                    Party = TestData.Candidate241Party
                },
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate240Id,
                    LastName = TestData.Candidate240LastName,
                    FirstName = TestData.Candidate240FirstName,
                    MiddleName = TestData.Candidate240MiddleName,
                    DisplayName = TestData.Candidate240DisplayName,
                    DisplayOrder = TestData.Candidate240Displayorder,
                    Party = TestData.Candidate240Party
                },

                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate239Id,
                    LastName = TestData.Candidate239LastName,
                    FirstName = TestData.Candidate239FirstName,
                    MiddleName = TestData.Candidate239MiddleName,
                    DisplayName = TestData.Candidate239DisplayName,
                    DisplayOrder = TestData.Candidate239Displayorder,
                    Party = TestData.Candidate239Party
                },
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate238Id,
                    LastName = TestData.Candidate238LastName,
                    FirstName = TestData.Candidate238FirstName,
                    MiddleName = TestData.Candidate238MiddleName,
                    DisplayName = TestData.Candidate238DisplayName,
                    DisplayOrder = TestData.Candidate238Displayorder,
                    Party = TestData.Candidate238Party
                },
                new Candidate(){
                    RaceId = TestData.Race2Id,
                    Id = TestData.Candidate237Id,
                    LastName = TestData.Candidate237LastName,
                    FirstName = TestData.Candidate237FirstName,
                    MiddleName = TestData.Candidate237MiddleName,
                    DisplayName = TestData.Candidate237DisplayName,
                    DisplayOrder = TestData.Candidate237Displayorder,
                    Party = TestData.Candidate237Party
                },
                
                #endregion House of Representatives Candidates

            };

            foreach(Candidate candidate in candidateSeeds)
            {
                await CreateCandidateAction(applicationDbService, candidate);
            }
        }


        public static async Task AddResultAction(ApplicationDbService applicationDbService, int RaceCountyDataId, int PrecinctsReporting, List<CandidateResult> candidateResults)
        {
            SpecialElection.Areas.Special.Pages.ResultView.CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new SpecialElection.Areas.Special.Pages.ResultView.CreateModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(RaceCountyDataId); });

            pageModel.inputModel.PrecinctsReporting = PrecinctsReporting;
            pageModel.inputModel.ResultModelList = candidateResults.Select(x => new SpecialElection.Areas.Special.Pages.ResultView.CreateModel.ResultModel()
            {
                CandidateId = x.CandidateId,
                Votes = x.Votes,
                Percent = x.Percent
            }).ToList();

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.inputModel);
        }
        public static async Task EditPrecinctAction(ApplicationDbService applicationDbService, int RaceId, List<RaceCountyData> RaceCountyData)
        {
            EditRacePrecinctsModel pageModel = TestUtility.InitPageModel(delegate () { return new EditRacePrecinctsModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(RaceId); });

            EditRacePrecinctsModel.InputModel[] result = new EditRacePrecinctsModel.InputModel[pageModel.RaceCountyData.Count];
            pageModel.RaceCountyData.CopyTo(result);
            List<EditRacePrecinctsModel.InputModel> r = result.OrderBy(x => x.Id).ToList();

            r.ForEach(x => x.NumberOfPrecinct = RaceCountyData.FirstOrDefault(c => c.Id.Equals(x.Id)).NumberOfPrecinct);

            pageModel.RaceCountyData = RaceCountyData.Select(x => new EditRacePrecinctsModel.InputModel(x)).ToList();

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.RaceCountyData.ToArray());

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(RaceId); });

            List<EditRacePrecinctsModel.InputModel> e = pageModel.RaceCountyData.OrderBy(x => x.Id).ToList();

            Assert.IsTrue(e.Count.Equals(r.Count));
            for (int i = 0; i < r.Count; i++)
            {
                Assert.IsTrue(e[i].Compare(r[i]));
            }
        }

        public static async Task PopulateWtihCandidateResults(ApplicationDbService applicationDbService)
        {
            await PopulateWithCandidateData(applicationDbService);

            List<Race> races = await applicationDbService.GetRaces().Include(x => x.RaceCountyDataList).ThenInclude(x => x.CandidateResults).ToListAsync();

            foreach(Race race in races)
            {
                foreach (RaceCountyData raceCountyData in race.RaceCountyDataList)
                {
                    switch ((int)raceCountyData.County)
                    {
                        case 19:
                            await AddResultAction(applicationDbService, raceCountyData.Id, 180, new List<CandidateResult>() {
                             new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 233,
                                Votes = 919,
                                Percent = 0.70
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 234,
                                Votes = 45011,
                                Percent = 36.00
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 235,
                                Votes = 8725,
                                Percent = 7.00
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 236,
                                Votes = 6387,
                                Percent = 5.10
                            },

                                new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 231,
                                Votes = 2510,
                                Percent = 2.00
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 232,
                                Votes = 1272,
                                Percent = 1.00
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 239,
                                Votes = 21897,
                                Percent = 17.50
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 240,
                                Votes = 2554,
                                Percent = 2.00
                            },

                                new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 237,
                                Votes = 29121,
                                Percent = 23.30
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 238,
                                Votes = 2122,
                                Percent = 1.70
                            },
                           new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 241,
                                Votes = 2357,
                                Percent = 1.90
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 242,
                                Votes = 2231,
                                Percent = 1.80
                            },
                        });

                            raceCountyData.NumberOfPrecinct = 180;
                            raceCountyData.PrecinctsReporting = 180;
                            raceCountyData.UpdatedDateTime = new DateTime(2020, 3, 17, 16, 41, 0, DateTimeKind.Utc);

                            break;
                        case 33:
                            raceCountyData.NumberOfPrecinct = 402;
                            raceCountyData.PrecinctsReporting = 402;
                            raceCountyData.UpdatedDateTime = new DateTime(2020, 3, 17, 16, 41, 0, DateTimeKind.Utc);

                            await AddResultAction(applicationDbService, raceCountyData.Id, 402, new List<CandidateResult>() {
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 214,
                                Votes = 5912,
                                Percent = 2.90
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 217,
                                Votes = 81918,
                                Percent = 40.50
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 218,
                                Votes = 24536,
                                Percent = 12.10
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 215,
                                Votes = 47516,
                                Percent = 23.50
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 216,
                                Votes = 42222,
                                Percent = 20.90
                            },
                        });

                            break;
                        case 56:
                            raceCountyData.NumberOfPrecinct = 72;
                            raceCountyData.PrecinctsReporting = 72;
                            raceCountyData.UpdatedDateTime = new DateTime(2020, 3, 17, 16, 41, 0, DateTimeKind.Utc);

                            await AddResultAction(applicationDbService, raceCountyData.Id, 72, new List<CandidateResult>() {
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 231,
                                Votes = 398,
                                Percent = 1.20
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 232,
                                Votes = 117,
                                Percent = 0.30
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 233,
                                Votes = 143,
                                Percent = 0.40
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 234,
                                Votes = 12412,
                                Percent = 36.80
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 235,
                                Votes = 1666,
                                Percent = 4.90
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 236,
                                Votes = 827,
                                Percent = 2.50
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 237,
                                Votes = 11190,
                                Percent = 33.20
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 238,
                                Votes = 376,
                                Percent = 1.10
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 239,
                                Votes = 5475,
                                Percent = 16.20
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 240,
                                Votes = 476,
                                Percent = 1.40
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 241,
                                Votes = 369,
                                Percent = 1.10
                            },
                            new CandidateResult() {
                                RaceCountyDataId = raceCountyData.Id,
                                CandidateId = 242,
                                Votes = 294,
                                Percent = 0.90
                            },
                        });
                            break;
                        default:
                            break;
                    }
                }

                await EditPrecinctAction(applicationDbService, race.Id, race.RaceCountyDataList.ToList());

                await applicationDbService.EditRaceCountyData(race.RaceCountyDataList.ToList());
            }

        }

    }
}
