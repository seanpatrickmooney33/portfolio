using NUnit.Framework;
using System.Threading.Tasks;
using SpecialElection.Areas.Special.Pages;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SpecialElection.Data.Model;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Prototype.Test.Utility;

namespace Prototype.Test.Special
{
    public class CurrentElectionTests : BaseTest
    {
        [Test]
        public async Task ViewCurrentElectionNoElection()
        {
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService, messageService); });

            await pageModel.Call<RedirectToPageResult>(pageModel.OnGetAsync);
        }

        [Test]
        public async Task ViewCurrentElectionNoActiveElection()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);
            Assert.IsTrue(applicationDbService.GetElection().Count() == 1);

            Election e = await applicationDbService.GetElection().FirstOrDefaultAsync();
            e.IsActive = false;
            await applicationDbService.EditElection(e);
            
            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService, messageService); });

            await pageModel.Call<RedirectToPageResult>(pageModel.OnGetAsync);
        }

        [Test]
        public async Task ViewCurrentElectionWithNoRace()
        {
            await SpecialElectionTestUtility.PopulateWithElectionData(applicationDbService);

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService, messageService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);
        }

        [Test]
        public async Task ViewCurrentElectionWithNoCandidate()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService, messageService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);
        }

        [Test]
        public async Task ViewCurrentElection()
        {
            await SpecialElectionTestUtility.PopulateWtihCandidateResults(applicationDbService);

            IndexModel pageModel = TestUtility.InitPageModel(delegate () { return new IndexModel(applicationDbService, messageService); });

            await pageModel.Call<PageResult>(pageModel.OnGetAsync);
        }

    }
}
