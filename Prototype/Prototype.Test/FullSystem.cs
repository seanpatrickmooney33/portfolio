using EngineServer.Controllers;
using EngineServer.Message;
using EngineServer.Service;
using NUnit.Framework;
using Prototype.Data;
using SpecialElection.Data;
using SpecialElection.Service;
using System.Threading.Tasks;
using System;
using Prototype.Lambda.API.Controllers;
using Prototype.Service;
using Prototype.Service.InMemoryDatabase;
using Moq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Prototype.Lambda.API.Service;
using Microsoft.Extensions.Logging;
using Prototype.Web.Areas.Static.Pages.Candidate;
using System.Collections.Generic;
using System.Linq;
using Prototype.Test.Utility;
using Prototype.Service.HttpClient.Mock;
using Prototype.Data.Models.VoteResults;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Prototype.Data.Models.ViewModels;

namespace Prototype.Test
{
    public class Tests
    {
        private MessageController MessageController;
        private Driver driver;


        private CandidateController CandidateController;
        private RaceController RaceController;

        private ApplicationDbService applicationDbService;
        private MessageService messageService;
        private IMessageClient messageClient;

        private ApiService apiService;

        [SetUp]
        public void Setup()
        {
            // set up Special election hanlder
            ApplicationDbContext context = SpecialElectionTestUtility.CreateTestApplcationDbContext();
            applicationDbService = new ApplicationDbService(context);
            messageClient = new MockMessageClient();
            messageService = new MessageService(applicationDbService, messageClient);

            IRedisService redisService = new MockRedisService();

            // Set up engine
            RedisBuffer RedisBuffer = new RedisBuffer();
            driver = new Driver(RedisBuffer, redisService);

            RaceMessageProcessor RMessageProcessor = new RaceMessageProcessor(RedisBuffer);
            VMessageProcessor VMessageProcessor = new VMessageProcessor(RedisBuffer);
            CMessageProcessor CMessageProcessor = new CMessageProcessor(RedisBuffer);
            MessageController = new MessageController(RMessageProcessor, VMessageProcessor, CMessageProcessor, driver);

            // set up API
            CandidateController = new CandidateController(redisService);
            RaceController = new RaceController(redisService);

            //Website
            MockSOSHttpClientFactory factory = new MockSOSHttpClientFactory("Prototype.Lambda.API");

            var logger = Mock.Of<ILogger>();
            apiService = new ApiService(factory, logger);

            factory.Register<ILogger>(logger);
            factory.Register<IRedisService>(redisService);

        }

        [Test]
        public async Task TestCMessage()
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            String result = await messageService.GenerateCMSG();

            MessageController.UploadCMessage(result, "C20SE");
            driver.WriteToRedis();

            CandidateInfo CandidateInfo = CandidateController.Get(TestData.Candidate214Id);

            Assert.IsTrue(CandidateInfo.CandidateId.Equals(TestData.Candidate214Id));
            Assert.IsTrue(CandidateInfo.DisplayName.Equals(TestData.Candidate214DisplayName));
            Assert.IsTrue(CandidateInfo.Party.Equals(TestData.Candidate214Party));
            Assert.IsTrue(CandidateInfo.FirstName.Equals(TestData.Candidate214FirstName));
            Assert.IsTrue(CandidateInfo.MiddleName.Equals(TestData.Candidate214MiddleName));
            Assert.IsTrue(CandidateInfo.LastName.Equals(TestData.Candidate214LastName));

            DetailsModel detailsModel = TestUtility.InitPageModel(() => new DetailsModel(Mock.Of<ILogger<DetailsModel>>(), apiService));
            await detailsModel.Call<PageResult>(delegate () { return detailsModel.OnGetAsync(TestData.Candidate214Id); });

            Assert.IsTrue(detailsModel.Candidate.CandidateId.Equals(TestData.Candidate214Id));
            Assert.IsTrue(detailsModel.Candidate.DisplayName.Equals(TestData.Candidate214DisplayName));
            Assert.IsTrue(detailsModel.Candidate.Party.Equals(TestData.Candidate214Party));
            Assert.IsTrue(detailsModel.Candidate.FirstName.Equals(TestData.Candidate214FirstName));
            Assert.IsTrue(detailsModel.Candidate.MiddleName.Equals(TestData.Candidate214MiddleName));
            Assert.IsTrue(detailsModel.Candidate.LastName.Equals(TestData.Candidate214LastName));


            IndexModel indexModel = TestUtility.InitPageModel(() => new IndexModel(Mock.Of<ILogger<IndexModel>>(), apiService));
            await indexModel.Call<PageResult>(indexModel.OnGetAsync);

            Assert.IsTrue(indexModel.Candidates.Count == 17);
        }

        [Test]
        public async Task TestRmessage()
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            String result = await messageService.GenerateRMSG();

            MessageController.UploadRMessage(result, "R20SE");
            driver.WriteToRedis();

            List<RaceInfo> raceInfo = RaceController.Get().OrderBy(x => x.RaceName).Select(x => new RaceInfo(x)).ToList();
            Assert.IsTrue(raceInfo.Count == 2);

            Assert.IsTrue(raceInfo[0].RaceName.Equals(TestData.Race1Name));
            Assert.IsTrue(raceInfo[0].RedisKey.ToKey().Equals(TestData.Race1RedisKey));

            Assert.IsTrue(raceInfo[1].RaceName.Equals(TestData.Race2Name));
            Assert.IsTrue(raceInfo[1].RedisKey.ToKey().Equals(TestData.Race2RedisKey));

            Web.Areas.Static.Pages.Race.IndexModel indexModel = TestUtility.InitPageModel(() => new Web.Areas.Static.Pages.Race.IndexModel(Mock.Of<ILogger<Web.Areas.Static.Pages.Race.IndexModel>>(), apiService));
            await indexModel.Call<PageResult>(indexModel.OnGetAsync);

            Assert.IsTrue(indexModel.Races.Count == 2);
            raceInfo = indexModel.Races.OrderBy(x => x.RaceName).ToList();

            Assert.IsTrue(raceInfo[0].RaceName.Equals(TestData.Race1Name));
            Assert.IsTrue(raceInfo[0].RedisKey.ToKey().Equals(TestData.Race1RedisKey));

            Assert.IsTrue(raceInfo[1].RaceName.Equals(TestData.Race2Name));
            Assert.IsTrue(raceInfo[1].RedisKey.ToKey().Equals(TestData.Race2RedisKey));

            Web.Areas.Static.Pages.Race.DetailsModel detailsModel = TestUtility.InitPageModel(() => new Web.Areas.Static.Pages.Race.DetailsModel(Mock.Of<ILogger<Web.Areas.Static.Pages.Race.DetailsModel>>(), apiService));

            await detailsModel.Call<PageResult>(delegate () { return detailsModel.OnGetAsync(TestData.Race1RedisKey); });
            Assert.IsTrue(detailsModel.RaceInfo.RaceName.Equals(TestData.Race1Name));
            Assert.IsTrue(detailsModel.RaceInfo.RedisKey.ToKey().Equals(TestData.Race1RedisKey));

            await detailsModel.Call<PageResult>(delegate () { return detailsModel.OnGetAsync(TestData.Race2RedisKey); });
            Assert.IsTrue(detailsModel.RaceInfo.RaceName.Equals(TestData.Race2Name));
            Assert.IsTrue(detailsModel.RaceInfo.RedisKey.ToKey().Equals(TestData.Race2RedisKey));
        }


        [Test]
        public async Task TestVmessage()
        {
            await SpecialElectionTestUtility.PopulateWtihCandidateResults(applicationDbService);

            String result = await messageService.GenerateCMSG();
            MessageController.UploadCMessage(result, "C20SE");

            result = await messageService.GenerateRMSG();
            MessageController.UploadRMessage(result, "R20SE");

            result = await messageService.GenerateVMSG();
            MessageController.UploadVMessage(result, "V20SE");

            VoteRecordViewModel dataTest = apiService.GetVoteRecordViewModel("02120059280000");

            int x = 10;
        }


        [Test]
        public async Task SerializationTest()
        {
            RedisKey redisKey = new RedisKey("02125900280000");

            RaceInfoBase raceInfo = new RaceInfoBase() { RedisKey = redisKey.ToKey(), RaceName = "my Race Name" };

            SerializationTestClass serializationTestClass = new SerializationTestClass()
            {
                RedisKey = redisKey,
                RaceInfoBase = raceInfo
            };

            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions
            {
                MaxDepth = 1000,
                IgnoreNullValues = true,
            };

            String redisKeySerialize = JsonSerializer.Serialize<RedisKey>(redisKey, jsonSerializerOptions);
            RedisKey redisKeyDeserialize = JsonSerializer.Deserialize<RedisKey>(redisKeySerialize, jsonSerializerOptions);

            String raceInfoSerialize = JsonSerializer.Serialize<RaceInfoBase>(raceInfo, jsonSerializerOptions);
            RaceInfoBase raceInfoDeserialize = JsonSerializer.Deserialize<RaceInfoBase>(raceInfoSerialize, jsonSerializerOptions);


            String serializationTestClassSerialize = JsonSerializer.Serialize<SerializationTestClass>(serializationTestClass, jsonSerializerOptions);
            SerializationTestClass serializationTestClassDeserialize = JsonSerializer.Deserialize<SerializationTestClass>(serializationTestClassSerialize, jsonSerializerOptions);




            int xxxxx = 10;
        }

    }

    public class SerializationTestClass
    {
        public RedisKey RedisKey { get; set; }
        public RaceInfoBase RaceInfoBase { get; set; }
    }
}