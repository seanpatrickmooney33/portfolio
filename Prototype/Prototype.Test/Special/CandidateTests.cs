using System;
using System.Collections.Generic;
using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using Moq;
using SpecialElection.Data.Model;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Prototype.Data;
using SpecialElection.Service;
using SpecialElection.Areas.Special.Pages.CandidateView;
using Prototype.Test.Utility;

namespace Prototype.Test.Special
{
    public class CandidateTests : BaseTest
    {
        public async Task ValidateRace1DefaultCanidates()
        {
            List<Candidate> candidates = await applicationDbService.GetCandidate()
                                                                    .Where(x => x.RaceId.Equals(TestData.Race1Id))
                                                                    .Include(x => x.CandidateResults)
                                                                    .ToListAsync();
            ValidateRace1DefaultCanidates(candidates);
        }

        public void ValidateRace1DefaultCanidates(List<Candidate> candidates)
        {
            Assert.IsTrue(candidates[0].Id.Equals(TestData.Candidate214Id));
            Assert.IsTrue(candidates[0].LastName.Equals(TestData.Candidate214LastName));
            Assert.IsTrue(candidates[0].MiddleName.Equals(TestData.Candidate214MiddleName));
            Assert.IsTrue(candidates[0].FirstName.Equals(TestData.Candidate214FirstName));
            Assert.IsTrue(candidates[0].DisplayName.Equals(TestData.Candidate214DisplayName));
            Assert.IsTrue(candidates[0].DisplayOrder.Equals(TestData.Candidate214Displayorder));
            Assert.IsTrue(candidates[0].Party.Equals(TestData.Candidate214Party));
            Assert.IsTrue(candidates[0].CandidateResults.Count.Equals(1));

            Assert.IsTrue(candidates[1].Id.Equals(TestData.Candidate215Id));
            Assert.IsTrue(candidates[1].LastName.Equals(TestData.Candidate215LastName));
            Assert.IsTrue(candidates[1].MiddleName.Equals(TestData.Candidate215MiddleName));
            Assert.IsTrue(candidates[1].FirstName.Equals(TestData.Candidate215FirstName));
            Assert.IsTrue(candidates[1].DisplayName.Equals(TestData.Candidate215DisplayName));
            Assert.IsTrue(candidates[1].DisplayOrder.Equals(TestData.Candidate215Displayorder));
            Assert.IsTrue(candidates[1].Party.Equals(TestData.Candidate215Party));
            Assert.IsTrue(candidates[1].CandidateResults.Count.Equals(1));

            Assert.IsTrue(candidates[2].Id.Equals(TestData.Candidate216Id));
            Assert.IsTrue(candidates[2].LastName.Equals(TestData.Candidate216LastName));
            Assert.IsTrue(candidates[2].MiddleName.Equals(TestData.Candidate216MiddleName));
            Assert.IsTrue(candidates[2].FirstName.Equals(TestData.Candidate216FirstName));
            Assert.IsTrue(candidates[2].DisplayName.Equals(TestData.Candidate216DisplayName));
            Assert.IsTrue(candidates[2].DisplayOrder.Equals(TestData.Candidate216Displayorder));
            Assert.IsTrue(candidates[2].Party.Equals(TestData.Candidate216Party));
            Assert.IsTrue(candidates[2].CandidateResults.Count.Equals(1));

            Assert.IsTrue(candidates[3].Id.Equals(TestData.Candidate217Id));
            Assert.IsTrue(candidates[3].LastName.Equals(TestData.Candidate217LastName));
            Assert.IsTrue(candidates[3].MiddleName.Equals(TestData.Candidate217MiddleName));
            Assert.IsTrue(candidates[3].FirstName.Equals(TestData.Candidate217FirstName));
            Assert.IsTrue(candidates[3].DisplayName.Equals(TestData.Candidate217DisplayName));
            Assert.IsTrue(candidates[3].DisplayOrder.Equals(TestData.Candidate217Displayorder));
            Assert.IsTrue(candidates[3].Party.Equals(TestData.Candidate217Party));
            Assert.IsTrue(candidates[3].CandidateResults.Count.Equals(1));

            Assert.IsTrue(candidates[4].Id.Equals(TestData.Candidate218Id));
            Assert.IsTrue(candidates[4].LastName.Equals(TestData.Candidate218LastName));
            Assert.IsTrue(candidates[4].MiddleName.Equals(TestData.Candidate218MiddleName));
            Assert.IsTrue(candidates[4].FirstName.Equals(TestData.Candidate218FirstName));
            Assert.IsTrue(candidates[4].DisplayName.Equals(TestData.Candidate218DisplayName));
            Assert.IsTrue(candidates[4].DisplayOrder.Equals(TestData.Candidate218Displayorder));
            Assert.IsTrue(candidates[4].Party.Equals(TestData.Candidate218Party));
            Assert.IsTrue(candidates[4].CandidateResults.Count.Equals(1));
        }

        [TestCase(TestData.Candidate231DisplayName, TestData.Candidate231FirstName, TestData.Candidate231LastName, TestData.Candidate231MiddleName, TestData.Candidate231Party, TestData.Candidate231Displayorder, TestData.Race2Id)]
        public async Task CreateCandidate(String DisplayName, String FirstName, String LastName, String MiddleName, PartyTypeEnum Party, int DisplayOrder, int RaceId) {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);

            await SpecialElectionTestUtility.CreateCandidateAction(applicationDbService,new Candidate() { 
                DisplayName =  DisplayName,
                FirstName = FirstName,
                LastName =  LastName,
                MiddleName = MiddleName,
                Party = Party,
                DisplayOrder = DisplayOrder,
                RaceId = RaceId
            });
        }

        [Test]
        public async Task CreateWithoutElection()
        {
            CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new CreateModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(99); });
            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);
        }

        [Test]
        public async Task CreateWithoutRace()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new CreateModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(99); });
            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);
        }

        [Test]
        public async Task CreateWithInvalidDataCandidate()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);
            CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new CreateModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);


            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(99); });

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);



            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(1); });

            pageModel.Candidate = new Candidate()
            {
                DisplayName = null,
                FirstName = "6hsfnfs",
                LastName = "awesnsjk;vna",
                MiddleName = "safsdfasfsdf",
                DisplayOrder = 1,
                RaceId = 1
            };

            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Candidate);

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(1); });

            pageModel.Candidate = new Candidate()
            {
                DisplayName = "asdfasdf",
                FirstName = null,
                LastName = "awesnsjk;vna",
                MiddleName = "safsdfasfsdf",
                DisplayOrder = 1,
                RaceId = 1
            };

            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Candidate);

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(1); });

            pageModel.Candidate = new Candidate()
            {
                DisplayName = "asdfasdf",
                FirstName = "6hsfnfs",
                LastName = null,
                MiddleName = "safsdfasfsdf",
                DisplayOrder = 1,
                RaceId = 1
            };

            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Candidate);

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 0);
        }


        [Test]
        public async Task EditCandidate()
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 17);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race1Id); });

            Assert.IsTrue(pageModel.Candidate.Count == 5);

            pageModel.Candidate.ForEach(x => x.Party = PartyTypeEnum.NaturalLaw);
            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostSaveCandidateAsync);
            pageModel.Candidate.ForEach(x => Assert.IsTrue(x.Party.Equals(PartyTypeEnum.NaturalLaw)));

            String inputName = "gjrotjrnaklsdnl";
            pageModel.Candidate.ForEach(x => x.MiddleName = inputName);
            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostSaveCandidateAsync);
            pageModel.Candidate.ForEach(x => Assert.IsTrue(x.Party.Equals(PartyTypeEnum.NaturalLaw)));
            pageModel.Candidate.ForEach(x => Assert.IsTrue(x.MiddleName.Equals(inputName)));
        }
        
        [Test]
        public async Task EditWithInvalidDataCandidate() {

            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 17);
            await ValidateRace1DefaultCanidates();

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(99999); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race1Id); });
            await ValidateRace1DefaultCanidates();

            pageModel.Candidate.ForEach(x => x.DisplayName = null);
            await pageModel.Call<PageResult>(pageModel.OnPostSaveCandidateAsync, pageModel.Candidate.ToArray());
            await ValidateRace1DefaultCanidates();

            pageModel.Candidate.ForEach(x => x.FirstName = null);
            await pageModel.Call<PageResult>(pageModel.OnPostSaveCandidateAsync, pageModel.Candidate.ToArray());
            await ValidateRace1DefaultCanidates();

            pageModel.Candidate.ForEach(x => x.LastName = null);
            await pageModel.Call<PageResult>(pageModel.OnPostSaveCandidateAsync, pageModel.Candidate.ToArray());
            await ValidateRace1DefaultCanidates();


            pageModel.Candidate.ForEach(x => x.CandidateResults.Add( new CandidateResult()
                                                                    {
                                                                        RaceCountyDataId = 1,
                                                                        CandidateId = x.Id,
                                                                        Votes = 2510,
                                                                        Percent = 2.00
                                                                    })
            );
            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostSaveCandidateAsync, pageModel.Candidate.ToArray());
            await ValidateRace1DefaultCanidates();
        }


        [Test]
        public async Task ViewCandidate() {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 17);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race1Id); });

            candidates = await applicationDbService.GetCandidate().Where(x => x.RaceId.Equals(TestData.Race1Id)).ToListAsync();
            Assert.IsTrue(pageModel.Candidate.Count == candidates.Count);

            List<IndexModel.InputModel> testCandidates = pageModel.Candidate.ToList();

            for(int i = 0; i < candidates.Count; i++)
            {
                Assert.IsTrue(testCandidates[i].Compare(candidates[i]));
            }
        }
        [Test]
        public async Task ViewWithInvalidRaceIdCandidate()
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 17);

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(999999); });
        }

        [Test]
        public async Task DeleteCandidate() {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 17);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race1Id); });

            Assert.IsTrue(pageModel.Candidate.Count == 5);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteCandidateAsync(TestData.Candidate214Id); });
            
            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 16);

            Assert.IsTrue(candidates.FirstOrDefault(x => x.Id.Equals(TestData.Candidate214Id)) == null);
            Assert.IsTrue(pageModel.Candidate.Count == 4);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteCandidateAsync(TestData.Candidate215Id); });

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 15);

            Assert.IsTrue(candidates.FirstOrDefault(x => x.Id.Equals(TestData.Candidate215Id)) == null);
            Assert.IsTrue(pageModel.Candidate.Count == 3);



            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteCandidateAsync(TestData.Candidate216Id); });

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 14);

            Assert.IsTrue(candidates.FirstOrDefault(x => x.Id.Equals(TestData.Candidate216Id)) == null);
            Assert.IsTrue(pageModel.Candidate.Count == 2);



            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteCandidateAsync(TestData.Candidate217Id); });

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 13);

            Assert.IsTrue(candidates.FirstOrDefault(x => x.Id.Equals(TestData.Candidate217Id)) == null);
            Assert.IsTrue(pageModel.Candidate.Count == 1);

        }

        [Test]
        public async Task DeleteWithInvalidIdCandidate()
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            List<Candidate> candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 17);

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(99); }); 
            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 17);


            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(TestData.Race1Id); });

            Assert.IsTrue(pageModel.Candidate.Count == 5);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteCandidateAsync(999999); });

            candidates = await applicationDbService.GetCandidate().ToListAsync();
            Assert.IsTrue(candidates.Count == 17);
            Assert.IsTrue(pageModel.Candidate.Count == 5);
        }

    }
}
