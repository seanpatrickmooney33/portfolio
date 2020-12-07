using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data;
using SpecialElection.Areas.Special.Pages.ElectionView;
using SpecialElection.Data.Model;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Prototype.Test.Utility;

namespace Prototype.Test.Special
{
    public class ElectionTests : BaseTest
    {
        private async Task ValidateDefaultElection()
        {

            List<Election> elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 1);
            ValidateDefaultElection(elections[0]);
        }
        private void ValidateDefaultElection(Election election)
        {
            Assert.IsTrue(election.Name.Equals(TestData.ElectionName));
            Assert.IsTrue(election.ElectionDate.Equals(TestData.ElectionDate));
            Assert.IsTrue(election.CreatedBy.Equals("Fixme"));
            Assert.IsTrue(election.ModifyBy.Equals("Fixme"));
            Assert.IsTrue(election.IsActive == TestData.ElectionIsActive);
        }


        [Test]
        public async Task CreateElection()
        {
            CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new CreateModel(applicationDbService); });

            List<Election> elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 0);

            DateTime dateTime = DateTime.UtcNow;
            String electionName = "my New Election";
            pageModel.Election = new CreateModel.InputModel
            {
                Name = electionName,
                ElectionDate = dateTime,
                IsActive = true
            };

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Election);

            elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 1);
            Assert.IsTrue(elections[0].Name.Equals(electionName));
            Assert.IsTrue(elections[0].ElectionDate.Equals(dateTime));
            Assert.IsTrue(elections[0].CreatedBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].ModifyBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].IsActive == true);


            DateTime dateTime2 = DateTime.UtcNow;
            String electionName2 = "Next New Election";

            pageModel.Election.Name = electionName2;
            pageModel.Election.ElectionDate = dateTime2;
            pageModel.Election.IsActive = false;

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Election);

            elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 2);
            Assert.IsTrue(elections[0].Name.Equals(electionName));
            Assert.IsTrue(elections[0].ElectionDate.Equals(dateTime));
            Assert.IsTrue(elections[0].CreatedBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].ModifyBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].IsActive == true);

            Assert.IsTrue(elections[1].Name.Equals(electionName2));
            Assert.IsTrue(elections[1].ElectionDate.Equals(dateTime2));
            Assert.IsTrue(elections[1].CreatedBy.Equals("Fixme"));
            Assert.IsTrue(elections[1].ModifyBy.Equals("Fixme"));
            Assert.IsTrue(elections[1].IsActive == false);


            DateTime dateTime3 = DateTime.UtcNow;
            String electionName3 = "Next New Election";

            pageModel.Election.Name = electionName3;
            pageModel.Election.ElectionDate = dateTime3;
            pageModel.Election.IsActive = false;

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Election);

            elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 3);
            Assert.IsTrue(elections[0].Name.Equals(electionName));
            Assert.IsTrue(elections[0].ElectionDate.Equals(dateTime));
            Assert.IsTrue(elections[0].CreatedBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].ModifyBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].IsActive == true);

            Assert.IsTrue(elections[1].Name.Equals(electionName2));
            Assert.IsTrue(elections[1].ElectionDate.Equals(dateTime2));
            Assert.IsTrue(elections[1].CreatedBy.Equals("Fixme"));
            Assert.IsTrue(elections[1].ModifyBy.Equals("Fixme"));
            Assert.IsTrue(elections[1].IsActive == false);

            Assert.IsTrue(elections[2].Name.Equals(electionName3));
            Assert.IsTrue(elections[2].ElectionDate.Equals(dateTime3));
            Assert.IsTrue(elections[2].CreatedBy.Equals("Fixme"));
            Assert.IsTrue(elections[2].ModifyBy.Equals("Fixme"));
            Assert.IsTrue(elections[2].IsActive == false);
        }

        [Test]
        public async Task CreateWithInvalidDataElection()
        {
            CreateModel pageModel = TestUtility.InitPageModel(delegate () { return new CreateModel(applicationDbService); });

            List<Election> elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 0);

            pageModel.Election = new CreateModel.InputModel()
            {
                ElectionDate = DateTime.UtcNow
            };

            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Election);

            elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 0);
        }


        [Test]
        public async Task EditElection()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await ValidateDefaultElection();

            Election e = applicationDbService.GetElection().FirstOrDefault();
            DateTime OrginalCreateDate = e.CreateDate;
            DateTime OrginalModifyDate = e.ModifyDate;

            EditModel pageModel = TestUtility.InitPageModel(delegate () { return new EditModel(applicationDbService); });

            await pageModel.Call<NotFoundResult>(delegate() { return pageModel.OnGetAsync(null); });
            await pageModel.Call<NotFoundResult>(delegate() { return pageModel.OnGetAsync(2); });
            await pageModel.Call<PageResult>(delegate() { return pageModel.OnGetAsync(1); });
            
            await ValidateDefaultElection();
            ValidateDefaultElection(pageModel.Election);

            String updatedEletionName = "changed my name";
            DateTime updateDateTime = DateTime.UtcNow;
            
            pageModel.Election.Name = updatedEletionName;
            pageModel.Election.ElectionDate = updateDateTime;
            pageModel.Election.CreatedBy = "new person";
            pageModel.Election.ModifyBy = "another person";
            pageModel.Election.IsActive = true;

            await pageModel.Call<RedirectToPageResult>(pageModel.OnPostAsync, pageModel.Election);

            List<Election> elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 1);
            Assert.IsTrue(elections[0].Id == 1);
            Assert.IsTrue(elections[0].Name.Equals(updatedEletionName));
            Assert.IsTrue(elections[0].ElectionDate.Equals(updateDateTime));
            Assert.IsTrue(elections[0].CreatedBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].ModifyBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].IsActive == true);

            Assert.IsTrue(elections[0].CreateDate.Equals(OrginalCreateDate));
            Assert.IsTrue(elections[0].ModifyDate > OrginalModifyDate);

        }
        [Test]
        public async Task EditWithInvalidDataElection() 
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await ValidateDefaultElection();

            EditModel pageModel = TestUtility.InitPageModel(delegate () { return new EditModel(applicationDbService); });

            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(null); });
            await pageModel.Call<NotFoundResult>(delegate () { return pageModel.OnGetAsync(2); });
            await pageModel.Call<PageResult>(delegate () { return pageModel.OnGetAsync(1); });

            await ValidateDefaultElection();

            ValidateDefaultElection(pageModel.Election);

            pageModel.Election.Name = null;
            await pageModel.Call<PageResult>(pageModel.OnPostAsync, pageModel.Election);
            await ValidateDefaultElection();
        }

        [Test]
        public async Task ActivateElection()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await ValidateDefaultElection();

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostActivateElectionAsync(1); });

            List<Election> elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 1);
            Assert.IsTrue(elections[0].Id == 1);
            Assert.IsTrue(elections[0].Name.Equals(TestData.ElectionName));
            Assert.IsTrue(elections[0].ElectionDate.Equals(TestData.ElectionDate));
            Assert.IsTrue(elections[0].CreatedBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].ModifyBy.Equals("Fixme"));
            Assert.IsTrue(elections[0].IsActive == !TestData.ElectionIsActive);

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostActivateElectionAsync(1); });
            
            // calling Activate twice should get the election back to its orginal state
            await ValidateDefaultElection();
        }

        [Test]
        public async Task MultipleActiveElections()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await ValidateDefaultElection();

            Assert.IsTrue(applicationDbService.GetElection().Where(x => x.IsActive).Count() == 1);
            await SpecialElectionTestUtility.CreateElectionAction(applicationDbService, "second Election", DateTime.UtcNow, false);
            await SpecialElectionTestUtility.CreateElectionAction(applicationDbService, "Three Election", DateTime.UtcNow, false);
            await SpecialElectionTestUtility.CreateElectionAction(applicationDbService, "Four Election", DateTime.UtcNow, false);

            Assert.IsTrue(applicationDbService.GetElection().Count() == 4);
            Assert.IsTrue(applicationDbService.GetElection().Where(x => x.IsActive).Count() == 1);

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostActivateElectionAsync(1); });

            Assert.IsTrue(applicationDbService.GetElection().Where(x => x.IsActive).Count() == 0);

            await applicationDbService.GetElection().ForEachAsync(async x => {
                await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostActivateElectionAsync(x.Id); });
            });


            Assert.IsTrue(applicationDbService.GetElection().Count() == 4);
            Assert.IsTrue(applicationDbService.GetElection().Where(x => x.IsActive).Count() == 1);

        }

        [Test]
        public async Task ActivateWithInvalidIdElection()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await ValidateDefaultElection();

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostActivateElectionAsync(2); });

            await ValidateDefaultElection();
        }

        [Test]
        public async Task DeleteElection()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await ValidateDefaultElection();

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate() { return pageModel.OnPostDeleteElectionAsync(1); });

            List<Election> elections = await applicationDbService.GetElection().ToListAsync();
            Assert.IsTrue(elections.Count == 0);
        }
        
        [Test]
        public async Task DeleteWithInvalidIdElection()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await ValidateDefaultElection();

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate() { return pageModel.OnPostDeleteElectionAsync(3); });
            await ValidateDefaultElection();
        }

        [Test]
        public async Task DeleteElectionWithFullData()
        {
            await SpecialElectionTestUtility.PopulateWtihCandidateResults(applicationDbService);
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(delegate () { return pageModel.OnPostDeleteElectionAsync(1); });

            Assert.IsTrue(applicationDbService.GetElection().ToList().Count == 0);
            Assert.IsTrue(applicationDbService.GetRaces().ToList().Count == 0);
            Assert.IsTrue(applicationDbService.GetRaceCountyData().ToList().Count == 0);
            Assert.IsTrue(applicationDbService.GetCandidate().ToList().Count == 0);
            Assert.IsTrue(applicationDbService.GetCandidateResults().ToList().Count == 0);

        }

        [Test]
        public async Task GetElections()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            await ValidateDefaultElection();

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);

            Assert.IsTrue(pageModel.Election.Count == 1);
            ValidateDefaultElection(pageModel.Election[0]);
        }
    
    }
}