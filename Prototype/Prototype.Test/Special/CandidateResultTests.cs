using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpecialElection.Data;
using Moq;
using SpecialElection.Areas.Special.Pages.ResultView;
using SpecialElection.Data.Model;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Prototype.Data;
using Prototype.Test.Utility;

namespace Prototype.Test.Special
{
    public class CandidateResultTests : BaseTest
    {
        public void ValidateCandidateResults(DistrictwideCandidateData c, String DisplayName, PartyTypeEnum party, int votes, string percent)
        {
            Assert.IsTrue(c.DisplayName.Equals(DisplayName), "DisplayName Expected to be ["+ DisplayName+"] but is [" + c.DisplayName + "]");
            Assert.IsTrue(c.Party.Equals(party), "Party Expected to be ["+ party+"] but is [" + c.Party + "]");
            Assert.IsTrue(c.Votes.Equals(votes), "Votes Expected to be ["+ votes+"] but is [" + c.Votes + "]");
            Assert.IsTrue(c.VotePercent.Equals(percent), "VotePercent Expected to be ["+ percent+"] but is [" + c.VotePercent + "]");
        }

        public void ValidateCandidateResults(CandidateResult c, String DisplayName, PartyTypeEnum party, int votes, double percent)
        {
            Assert.IsTrue(c.Candidate.DisplayName.Equals(DisplayName), "DisplayName Expected to be [" + DisplayName + "] but is [" + c.Candidate.DisplayName + "]");
            Assert.IsTrue(c.Candidate.Party.Equals(party), "Party Expected to be [" + party + "] but is [" + c.Candidate.Party + "]");
            Assert.IsTrue(c.Votes.Equals(votes), "Votes Expected to be [" + votes + "] but is [" + c.Votes + "]");
            Assert.IsTrue(c.Percent.Equals(percent), "VotePercent Expected to be [" + percent + "] but is [" + c.Percent + "]");
        }

        [Test]
        public async Task AddResultNoRace()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new CreateModel(applicationDbService); });

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(1); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(2); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(null); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(-1); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(1000); });

            pageModel.inputModel = new CreateModel.InputModel() {
                raceCountyDataId = 1,
                PrecinctsReporting = 100,
                NumberOfPrecinct = 100,
                ResultModelList = new List<CreateModel.ResultModel>() 
                                        { new CreateModel.ResultModel()
                                            {
                                                CandidateId = 231,
                                                Votes = 2510,
                                                Percent = 2.00
                                            }}
            };

            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.inputModel);

            Assert.IsTrue(applicationDbService.GetRaces().Count() == 0);
            Assert.IsTrue(applicationDbService.GetCandidate().Count() == 0);
            Assert.IsTrue(applicationDbService.GetRaceCountyData().Count() == 0);
            Assert.IsTrue(applicationDbService.GetCandidateResults().Count() == 0);
        }

        [Test]
        public async Task AddResultNoCandidate()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);

            Assert.IsTrue(applicationDbService.GetCandidate().Count() == 0);
            Assert.IsTrue(applicationDbService.GetCandidateResults().Count() == 0);

            CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new CreateModel(applicationDbService); });

            Race race1 = await applicationDbService.GetRaces()
                .Where(x => x.Id.Equals(TestData.Race1Id))
                .Include(x => x.RaceCountyDataList).ThenInclude(x => x.CandidateResults)
                .FirstOrDefaultAsync();

            Assert.IsTrue(race1.RaceCountyDataList.Count() == 1);
            RaceCountyData r = race1.RaceCountyDataList.ToList()[0];
            Assert.IsTrue(r.NumberOfPrecinct == 0);
            Assert.IsTrue(r.PrecinctsReporting == 0);
            Assert.IsTrue(r.County == CountyTypeEnum.Riverside);
            Assert.IsTrue(r.CandidateResults.Count == 0);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(1); });

            Assert.IsTrue(pageModel.inputModel.ResultModelList.Count == 0);

            pageModel.inputModel.PrecinctsReporting = 100;
            pageModel.inputModel.NumberOfPrecinct = 1000;

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.inputModel);

            RaceCountyData rcd = await applicationDbService.GetRaceCountyData()
                .Include(x => x.CandidateResults)
                .FirstOrDefaultAsync(x => x.Id.Equals(1));

            Assert.IsTrue(rcd.NumberOfPrecinct == 0);
            Assert.IsTrue(rcd.PrecinctsReporting == 100);
            Assert.IsTrue(rcd.CandidateResults.Count == 0);


            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(1); });
            pageModel.inputModel.ResultModelList.Add(new CreateModel.ResultModel()
            {
                CandidateId = 231,
                Votes = 2510,
                Percent = 2.00
            });
            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.inputModel);

            Assert.IsTrue(applicationDbService.GetCandidate().Count() == 0);
            Assert.IsTrue(applicationDbService.GetCandidateResults().Count() == 0);

            rcd = await applicationDbService.GetRaceCountyData()
                .Include(x => x.CandidateResults)
                .FirstOrDefaultAsync(x => x.Id.Equals(1));

            Assert.IsTrue(rcd.NumberOfPrecinct == 0);
            Assert.IsTrue(rcd.PrecinctsReporting == 100);
            Assert.IsTrue(rcd.CandidateResults.Count == 0);
        }

        [Test]
        public async Task AddResult() 
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            
            CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new CreateModel(applicationDbService); });

            Race race1 = await applicationDbService.GetRaces()
                .Where(x => x.Id.Equals(TestData.Race1Id))
                .Include(x => x.RaceCountyDataList).ThenInclude(x => x.CandidateResults)
                .FirstOrDefaultAsync();

            Assert.IsTrue(race1.RaceCountyDataList.Count() == 1);
            RaceCountyData r = race1.RaceCountyDataList.ToList()[0];

            Assert.IsTrue(r.NumberOfPrecinct == 0);
            Assert.IsTrue(r.PrecinctsReporting == 0);
            Assert.IsTrue(r.County == CountyTypeEnum.Riverside);

            r.CandidateResults.ToList().ForEach(x => {
                Assert.IsTrue(x.Votes == 0);
                Assert.IsTrue(x.Percent == 0);
            });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(r.Id); });

            pageModel.inputModel.PrecinctsReporting = 100;
            pageModel.inputModel.NumberOfPrecinct = 1000;

            CreateModel.ResultModel rm = pageModel.inputModel.ResultModelList.FirstOrDefault(x => x.CandidateId.Equals(TestData.Candidate214Id));
            rm.Percent = 100.0;
            rm.Votes = 1000;

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.inputModel);

            RaceCountyData rcd = await applicationDbService.GetRaceCountyData()
                .Include(x => x.CandidateResults)
                .FirstOrDefaultAsync(x => x.Id.Equals(r.Id));

            Assert.IsTrue(rcd.NumberOfPrecinct == 0);
            Assert.IsTrue(rcd.PrecinctsReporting == 100);

            rcd.CandidateResults.ToList().ForEach(x => {
                if (x.CandidateId.Equals(TestData.Candidate214Id))
                {
                    Assert.IsTrue(x.Votes == 1000);
                    Assert.IsTrue(x.Percent == 100.0);
                } else { 
                    Assert.IsTrue(x.Votes == 0);
                    Assert.IsTrue(x.Percent == 0);
                }
            });

        }


        [Test]
        public async Task PreviewNoElectionResult()
        {
            
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<NotFoundResult>(pageModel.OnGetAsync);

        }

        [Test]
        public async Task PreviewNoRaceResult()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);

            Assert.IsTrue(pageModel.DistrictwideData.Count() == 0);
            
        }

        [Test]
        public async Task PreviewNoCandidateResult()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);

            var temp = pageModel.DistrictwideData[TestData.Race1Id];
            Assert.IsTrue(temp.TotalNumberOfPrecinct.Equals(0));
            Assert.IsTrue(temp.TotalPrecinctsReporting.Equals(0));
            Assert.IsTrue(temp.TotalVotes.Equals(0));
            Assert.IsTrue(temp.Data.Values.Count == 0);

            temp = pageModel.DistrictwideData[TestData.Race2Id];
            Assert.IsTrue(temp.TotalNumberOfPrecinct.Equals(0));
            Assert.IsTrue(temp.TotalPrecinctsReporting.Equals(0));
            Assert.IsTrue(temp.TotalVotes.Equals(0));
            Assert.IsTrue(temp.Data.Values.Count == 0);

            Assert.IsTrue(pageModel.Election.Races.Count == 2);

            Race r = pageModel.Election.Races.FirstOrDefault(r => r.Id.Equals(TestData.Race1Id));
            Assert.IsTrue(r.RaceCountyDataList.Count() == 1);

            RaceCountyData raceCountyData = r.RaceCountyDataList.ToList()[0];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.Riverside));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(0));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(0));
            Assert.IsTrue(raceCountyData.CandidateResults.Count() == 0);

            r = pageModel.Election.Races.FirstOrDefault(r => r.Id.Equals(TestData.Race2Id));
            Assert.IsTrue(r.RaceCountyDataList.Count() == 2);

            raceCountyData = r.RaceCountyDataList.ToList()[0];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.LosAngeles));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(0));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(0));
            Assert.IsTrue(raceCountyData.CandidateResults.Count() == 0);

            raceCountyData = r.RaceCountyDataList.ToList()[1];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.Ventura));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(0));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(0));
            Assert.IsTrue(raceCountyData.CandidateResults.Count() == 0);

        }

        [Test]
        public async Task PreviewNoCandidateDataResult()
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);

            #region validate district wide results
            var temp = pageModel.DistrictwideData[TestData.Race1Id];
            Assert.IsTrue(temp.TotalNumberOfPrecinct.Equals(0));
            Assert.IsTrue(temp.TotalPrecinctsReporting.Equals(0));
            Assert.IsTrue(temp.TotalVotes.Equals(0));

            ValidateCandidateResults(temp.Data[TestData.Candidate214Id], TestData.Candidate214DisplayName, TestData.Candidate214Party, 0, "0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate215Id], TestData.Candidate215DisplayName, TestData.Candidate215Party, 0, "0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate216Id], TestData.Candidate216DisplayName, TestData.Candidate216Party, 0, "0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate217Id], TestData.Candidate217DisplayName, TestData.Candidate217Party, 0, "0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate218Id], TestData.Candidate218DisplayName, TestData.Candidate218Party, 0, "0.0");


            temp = pageModel.DistrictwideData[TestData.Race2Id];
            Assert.IsTrue(temp.TotalNumberOfPrecinct.Equals(0));
            Assert.IsTrue(temp.TotalPrecinctsReporting.Equals(0));
            Assert.IsTrue(temp.TotalVotes.Equals(0));

            ValidateCandidateResults(temp.Data[TestData.Candidate231Id], TestData.Candidate231DisplayName, TestData.Candidate231Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate232Id], TestData.Candidate232DisplayName, TestData.Candidate232Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate233Id], TestData.Candidate233DisplayName, TestData.Candidate233Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate234Id], TestData.Candidate234DisplayName, TestData.Candidate234Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate235Id], TestData.Candidate235DisplayName, TestData.Candidate235Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate236Id], TestData.Candidate236DisplayName, TestData.Candidate236Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate237Id], TestData.Candidate237DisplayName, TestData.Candidate237Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate238Id], TestData.Candidate238DisplayName, TestData.Candidate238Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate239Id], TestData.Candidate239DisplayName, TestData.Candidate239Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate240Id], TestData.Candidate240DisplayName, TestData.Candidate240Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate241Id], TestData.Candidate241DisplayName, TestData.Candidate241Party,0,"0.0");
            ValidateCandidateResults(temp.Data[TestData.Candidate242Id], TestData.Candidate242DisplayName, TestData.Candidate242Party,0,"0.0");

            #endregion validate district wide results

            Assert.IsTrue(pageModel.Election.Races.Count == 2);

            #region Validate Race 1 results
            Race r = pageModel.Election.Races.FirstOrDefault(r => r.Id.Equals(TestData.Race1Id));
            Assert.IsTrue(r.RaceCountyDataList.Count() == 1);

            RaceCountyData raceCountyData = r.RaceCountyDataList.ToList()[0];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.Riverside));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(0));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(0));

            CandidateResult lazy(RaceCountyData r, int id) => r.CandidateResults.FirstOrDefault(x => x.CandidateId.Equals(id));

            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate214Id), TestData.Candidate214DisplayName, TestData.Candidate214Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate215Id), TestData.Candidate215DisplayName, TestData.Candidate215Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate216Id), TestData.Candidate216DisplayName, TestData.Candidate216Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate217Id), TestData.Candidate217DisplayName, TestData.Candidate217Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate218Id), TestData.Candidate218DisplayName, TestData.Candidate218Party, 0, 0);

            #endregion Validate Race 1 results

            #region Validate Race 2 results

            r = pageModel.Election.Races.FirstOrDefault(r => r.Id.Equals(TestData.Race2Id));
            Assert.IsTrue(r.RaceCountyDataList.Count() == 2);

            raceCountyData = r.RaceCountyDataList.ToList()[0];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.LosAngeles));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(0));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(0));

            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate231Id), TestData.Candidate231DisplayName, TestData.Candidate231Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate232Id), TestData.Candidate232DisplayName, TestData.Candidate232Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate233Id), TestData.Candidate233DisplayName, TestData.Candidate233Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate234Id), TestData.Candidate234DisplayName, TestData.Candidate234Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate235Id), TestData.Candidate235DisplayName, TestData.Candidate235Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate236Id), TestData.Candidate236DisplayName, TestData.Candidate236Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate237Id), TestData.Candidate237DisplayName, TestData.Candidate237Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate238Id), TestData.Candidate238DisplayName, TestData.Candidate238Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate239Id), TestData.Candidate239DisplayName, TestData.Candidate239Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate240Id), TestData.Candidate240DisplayName, TestData.Candidate240Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate241Id), TestData.Candidate241DisplayName, TestData.Candidate241Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate242Id), TestData.Candidate242DisplayName, TestData.Candidate242Party, 0, 0);


            raceCountyData = r.RaceCountyDataList.ToList()[1];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.Ventura));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(0));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(0));

            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate231Id), TestData.Candidate231DisplayName, TestData.Candidate231Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate232Id), TestData.Candidate232DisplayName, TestData.Candidate232Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate233Id), TestData.Candidate233DisplayName, TestData.Candidate233Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate234Id), TestData.Candidate234DisplayName, TestData.Candidate234Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate235Id), TestData.Candidate235DisplayName, TestData.Candidate235Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate236Id), TestData.Candidate236DisplayName, TestData.Candidate236Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate237Id), TestData.Candidate237DisplayName, TestData.Candidate237Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate238Id), TestData.Candidate238DisplayName, TestData.Candidate238Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate239Id), TestData.Candidate239DisplayName, TestData.Candidate239Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate240Id), TestData.Candidate240DisplayName, TestData.Candidate240Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate241Id), TestData.Candidate241DisplayName, TestData.Candidate241Party, 0, 0);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate242Id), TestData.Candidate242DisplayName, TestData.Candidate242Party, 0, 0);


            #endregion Validate Race 2 results

        }

        [Test]
        public async Task PreviewCandidateResult()
        {
            await SpecialElectionTestUtility.PopulateWtihCandidateResults(applicationDbService);

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);

            #region validate district wide results
            var temp = pageModel.DistrictwideData[TestData.Race1Id];
            Assert.IsTrue(temp.TotalNumberOfPrecinct.Equals(402));
            Assert.IsTrue(temp.TotalPrecinctsReporting.Equals(402));
            Assert.IsTrue(temp.TotalVotes.Equals(202104));

            ValidateCandidateResults(temp.Data[TestData.Candidate214Id], TestData.Candidate214DisplayName, TestData.Candidate214Party, 5912, "2.9");
            ValidateCandidateResults(temp.Data[TestData.Candidate215Id], TestData.Candidate215DisplayName, TestData.Candidate215Party, 47516, "23.5");
            ValidateCandidateResults(temp.Data[TestData.Candidate216Id], TestData.Candidate216DisplayName, TestData.Candidate216Party, 42222, "20.9");
            ValidateCandidateResults(temp.Data[TestData.Candidate217Id], TestData.Candidate217DisplayName, TestData.Candidate217Party, 81918, "40.5");
            ValidateCandidateResults(temp.Data[TestData.Candidate218Id], TestData.Candidate218DisplayName, TestData.Candidate218Party, 24536, "12.1");


            temp = pageModel.DistrictwideData[TestData.Race2Id];
            Assert.IsTrue(temp.TotalNumberOfPrecinct.Equals(252));
            Assert.IsTrue(temp.TotalPrecinctsReporting.Equals(252));
            Assert.IsTrue(temp.TotalVotes.Equals(158849));

            ValidateCandidateResults(temp.Data[TestData.Candidate231Id], TestData.Candidate231DisplayName, TestData.Candidate231Party, 2908, "1.8");
            ValidateCandidateResults(temp.Data[TestData.Candidate232Id], TestData.Candidate232DisplayName, TestData.Candidate232Party, 1389, "0.9");
            ValidateCandidateResults(temp.Data[TestData.Candidate233Id], TestData.Candidate233DisplayName, TestData.Candidate233Party, 1062, "0.7");
            ValidateCandidateResults(temp.Data[TestData.Candidate234Id], TestData.Candidate234DisplayName, TestData.Candidate234Party, 57423, "36.1");
            ValidateCandidateResults(temp.Data[TestData.Candidate235Id], TestData.Candidate235DisplayName, TestData.Candidate235Party, 10391, "6.5");
            ValidateCandidateResults(temp.Data[TestData.Candidate236Id], TestData.Candidate236DisplayName, TestData.Candidate236Party, 7214, "4.5");
            ValidateCandidateResults(temp.Data[TestData.Candidate237Id], TestData.Candidate237DisplayName, TestData.Candidate237Party, 40311, "25.4");
            ValidateCandidateResults(temp.Data[TestData.Candidate238Id], TestData.Candidate238DisplayName, TestData.Candidate238Party, 2498, "1.6");
            ValidateCandidateResults(temp.Data[TestData.Candidate239Id], TestData.Candidate239DisplayName, TestData.Candidate239Party, 27372, "17.2");
            ValidateCandidateResults(temp.Data[TestData.Candidate240Id], TestData.Candidate240DisplayName, TestData.Candidate240Party, 3030, "1.9");
            ValidateCandidateResults(temp.Data[TestData.Candidate241Id], TestData.Candidate241DisplayName, TestData.Candidate241Party, 2726, "1.7");
            ValidateCandidateResults(temp.Data[TestData.Candidate242Id], TestData.Candidate242DisplayName, TestData.Candidate242Party, 2525, "1.6");

            #endregion validate district wide results

            Assert.IsTrue(pageModel.Election.Races.Count == 2);

            #region Validate Race 1 results
            Race r = pageModel.Election.Races.FirstOrDefault(r => r.Id.Equals(TestData.Race1Id));
            Assert.IsTrue(r.RaceCountyDataList.Count() == 1);
            
            RaceCountyData raceCountyData = r.RaceCountyDataList.ToList()[0];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.Riverside));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(402));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(402));

            CandidateResult lazy(RaceCountyData r, int id) => r.CandidateResults.FirstOrDefault(x => x.CandidateId.Equals(id));

            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate214Id), TestData.Candidate214DisplayName, TestData.Candidate214Party, 5912 , 2.90);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate215Id), TestData.Candidate215DisplayName, TestData.Candidate215Party, 47516, 23.50);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate216Id), TestData.Candidate216DisplayName, TestData.Candidate216Party, 42222, 20.90);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate217Id), TestData.Candidate217DisplayName, TestData.Candidate217Party, 81918, 40.50);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate218Id), TestData.Candidate218DisplayName, TestData.Candidate218Party, 24536, 12.10);

            #endregion Validate Race 1 results

            #region Validate Race 2 results

            r = pageModel.Election.Races.FirstOrDefault(r => r.Id.Equals(TestData.Race2Id));
            Assert.IsTrue(r.RaceCountyDataList.Count() == 2);

            raceCountyData = r.RaceCountyDataList.ToList()[0];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.LosAngeles));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(180));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(180));

            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate231Id), TestData.Candidate231DisplayName, TestData.Candidate231Party,  2510,   2.00); 
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate232Id), TestData.Candidate232DisplayName, TestData.Candidate232Party,  1272,	1.00);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate233Id), TestData.Candidate233DisplayName, TestData.Candidate233Party,  919,	0.70);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate234Id), TestData.Candidate234DisplayName, TestData.Candidate234Party,  45011,	36.00);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate235Id), TestData.Candidate235DisplayName, TestData.Candidate235Party,  8725,	7.00);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate236Id), TestData.Candidate236DisplayName, TestData.Candidate236Party,  6387,	5.10);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate237Id), TestData.Candidate237DisplayName, TestData.Candidate237Party,  29121,	23.30);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate238Id), TestData.Candidate238DisplayName, TestData.Candidate238Party,  2122,	1.70);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate239Id), TestData.Candidate239DisplayName, TestData.Candidate239Party,  21897,	17.50);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate240Id), TestData.Candidate240DisplayName, TestData.Candidate240Party,  2554,	2.00);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate241Id), TestData.Candidate241DisplayName, TestData.Candidate241Party,  2357,   1.90);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate242Id), TestData.Candidate242DisplayName, TestData.Candidate242Party,  2231,   1.80);


            raceCountyData = r.RaceCountyDataList.ToList()[1];
            Assert.IsTrue(raceCountyData.County.Equals(CountyTypeEnum.Ventura));
            Assert.IsTrue(raceCountyData.PrecinctsReporting.Equals(72));
            Assert.IsTrue(raceCountyData.NumberOfPrecinct.Equals(72));

            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate231Id), TestData.Candidate231DisplayName, TestData.Candidate231Party, 398, 1.20);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate232Id), TestData.Candidate232DisplayName, TestData.Candidate232Party, 117, 0.30);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate233Id), TestData.Candidate233DisplayName, TestData.Candidate233Party, 143, 0.40);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate234Id), TestData.Candidate234DisplayName, TestData.Candidate234Party, 12412, 36.80);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate235Id), TestData.Candidate235DisplayName, TestData.Candidate235Party, 1666, 4.90);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate236Id), TestData.Candidate236DisplayName, TestData.Candidate236Party, 827, 2.50);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate237Id), TestData.Candidate237DisplayName, TestData.Candidate237Party, 11190, 33.20);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate238Id), TestData.Candidate238DisplayName, TestData.Candidate238Party, 376, 1.10);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate239Id), TestData.Candidate239DisplayName, TestData.Candidate239Party, 5475, 16.20);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate240Id), TestData.Candidate240DisplayName, TestData.Candidate240Party, 476, 1.40);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate241Id), TestData.Candidate241DisplayName, TestData.Candidate241Party, 369, 1.10);
            ValidateCandidateResults(lazy(raceCountyData, TestData.Candidate242Id), TestData.Candidate242DisplayName, TestData.Candidate242Party, 294, 0.90);


            #endregion Validate Race 2 results

        }

    }
}
