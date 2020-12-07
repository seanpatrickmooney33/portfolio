using EngineServer.Controllers;
using EngineServer.Message;
using EngineServer.Service;
using NUnit.Framework;
using SpecialElection.Data;
using SpecialElection.Service;
using System.Threading.Tasks;
using System;
using EngineServer.Test;
using Prototype.Service.InMemoryDatabase;
using Prototype.Test.Utility;
using Prototype.Test.Special;

namespace Prototype.Test
{
    public class SpecialApplicationIntegrationTest
    {
        private MessageController MessageController;
        private RaceMessageProcessor RMessageProcessor;
        private VMessageProcessor VMessageProcessor;
        private CMessageProcessor CMessageProcessor;


        private ApplicationDbService applicationDbService;
        private MessageService messageService;
        private IMessageClient messageClient;

        [SetUp]
        public void Setup()
        {

            // set up Special election hanlder
            ApplicationDbContext context = SpecialElectionTestUtility.CreateTestApplcationDbContext();
            applicationDbService = new ApplicationDbService(context);
            messageClient = new MockMessageClient();
            messageService = new MessageService(applicationDbService, messageClient);


            // Set up engine
            RedisBuffer RedisBuffer = new RedisBuffer();
            Driver driver = new Driver(RedisBuffer, new MockRedisService());

            RMessageProcessor = new RaceMessageProcessor(RedisBuffer);
            VMessageProcessor = new VMessageProcessor(RedisBuffer);
            CMessageProcessor = new CMessageProcessor(RedisBuffer);
            MessageController = new MessageController(RMessageProcessor, VMessageProcessor, CMessageProcessor, driver);
        }

        [Test]
        public async Task CreateVMSG()
        {
            await SpecialElectionTestUtility.PopulateWtihCandidateResults(applicationDbService);

            String result = await messageService.GenerateCMSG();
            MessageController.UploadCMessage(result, "C20SE");

            result = await messageService.GenerateRMSG();
            MessageController.UploadRMessage(result, "R20SE");

            result = await messageService.GenerateVMSG();
            MessageController.UploadVMessage(result, "V20SE");

            EngineServer.Test.Engine.Tests.ValidateVMessage(VMessageProcessor.VMessage);
        }

        [Test]
        public async Task CreateCMSG()
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            String result = await messageService.GenerateCMSG();

            MessageController.UploadCMessage(result, "C20SE");

            EngineServer.Test.Engine.Tests.ValidateCMessage(CMessageProcessor.CMessage);
        }


        [Test]
        public async Task CreateRMSG()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);

            String result = await messageService.GenerateRMSG();

            MessageController.UploadRMessage(result, "R20SE");

            EngineServer.Test.Engine.Tests.ValidateRMessage(RMessageProcessor.RMessage);
        }
    }
}
