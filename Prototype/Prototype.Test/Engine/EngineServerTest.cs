using EngineServer.Controllers;
using EngineServer.Message;
using EngineServer.Service;
using NUnit.Framework;
using Prototype.Data;
using System.Linq;
using System.Collections.Generic;
using Prototype.Service.InMemoryDatabase;
using Prototype.Data.Models.VoteResults;
using Prototype.Test.Utility;
using SpecialElection.Service;
using System.Net.Http;
using Prototype.Data.Results;
using System;

namespace EngineServer.Test.Engine
{
    public class Tests
    {
        private MessageController MessageController;
        private RaceMessageProcessor RMessageProcessor;
        private VMessageProcessor VMessageProcessor;
        private CMessageProcessor CMessageProcessor;
        //private RedisBuffer RedisBuffer;


        private readonly static HttpClient client = new HttpClient();

        protected IMessageClient messageClient;

        [SetUp]
        public void Setup()
        {

            messageClient = new MockMessageClient();
            RedisBuffer RedisBuffer = new RedisBuffer();

            Driver driver = new Driver(RedisBuffer, new MockRedisService());
            RMessageProcessor = new RaceMessageProcessor(RedisBuffer);
            VMessageProcessor = new VMessageProcessor(RedisBuffer);
            CMessageProcessor = new CMessageProcessor(RedisBuffer);
            MessageController = new MessageController(RMessageProcessor, VMessageProcessor, CMessageProcessor, driver);
        }

        [Test]
        public void UploadLegacyVMessage()
        {

            String result = FileData.GetFileData("V20PG.txt");
            using MultipartFormDataContent content = new MultipartFormDataContent
            {
                { new StringContent(result, System.Text.Encoding.UTF8), "messageFile", "V20PG.txt" },
                { new StringContent("a34fjfweflml3r3qdf43f9v9f434f43", System.Text.Encoding.UTF8), "token" }
            };

            // post to server
            HttpResponseMessage httpResponseMessage = client.PostAsync("http://internal-vote-loadb-1clezmicd7qjo-1996608841.us-gov-west-1.elb.amazonaws.com/upload", content).Result;
            //HttpResponseMessage httpResponseMessage = client.PostAsync("http://172.25.165.213/upload", content).Result;
            
            httpResponseMessage.EnsureSuccessStatusCode();

        }

        #region RMessage
        public static void ValidateRMessage(RaceMessage rMessage)
        {
            //RMessageProcessor.RMessage.DateTime;
            List<RaceRecord> SortedRecords = rMessage.rRecords.OrderBy(x => x.RaceName).ToList();
            Assert.IsTrue(SortedRecords.Count == 2);

            Assert.IsTrue(SortedRecords[0].RaceName.Equals(TestData.Race1Name));
            Assert.IsTrue(SortedRecords[0].ElectionShortName.Equals("SE"));
            Assert.IsTrue(SortedRecords[0].ContestId.RaceType.Equals(TestData.Race1Type));
            Assert.IsTrue(SortedRecords[0].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedRecords[0].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedRecords[0].ContestId.DistrictId.Equals(TestData.Race1District));


            Assert.IsTrue(SortedRecords[1].RaceName.Equals(TestData.Race2Name));
            Assert.IsTrue(SortedRecords[1].ElectionShortName.Equals("SE"));
            Assert.IsTrue(SortedRecords[1].ContestId.RaceType.Equals(TestData.Race2Type));
            Assert.IsTrue(SortedRecords[1].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedRecords[1].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedRecords[1].ContestId.DistrictId.Equals(TestData.Race2District));
        }
        [Test]
        public void UploadValidRMessage()
        {
            MessageController.UploadRMessage(TestData.Rmsg, "R20SE");
            ValidateRMessage(RMessageProcessor.RMessage);
        }
        #endregion RMessage

        #region CMessage
        public static void ValidateCMessage(CMessage cMessage)
        {

            List<CandidateInfoRecord> SortedInfo = cMessage.candidateInfoRecords.Values.ToList().OrderBy(x => x.CandidateId).ToList();
            Assert.IsTrue(SortedInfo.Count == 17);

            #region CandidateId 214
            Assert.IsTrue(SortedInfo[0].CandidateId.Equals(TestData.Candidate214Id));
            Assert.IsTrue(SortedInfo[0].DisplayName.Equals(TestData.Candidate214DisplayName));
            Assert.IsTrue(SortedInfo[0].Party.Equals(TestData.Candidate214Party));
            Assert.IsTrue(SortedInfo[0].FirstName.Equals(TestData.Candidate214FirstName));
            Assert.IsTrue(SortedInfo[0].MiddleName.Equals(TestData.Candidate214MiddleName));
            Assert.IsTrue(SortedInfo[0].LastName.Equals(TestData.Candidate214LastName));
            Assert.IsTrue(SortedInfo[0].ContestId.RaceType.Equals(RaceTypeEnum.StateSenate));
            Assert.IsTrue(SortedInfo[0].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[0].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[0].ContestId.DistrictId.Equals(28));
            #endregion CandidateId 214

            #region CandidateId 215
            Assert.IsTrue(SortedInfo[1].CandidateId.Equals(TestData.Candidate215Id));
            Assert.IsTrue(SortedInfo[1].DisplayName.Equals(TestData.Candidate215DisplayName));
            Assert.IsTrue(SortedInfo[1].Party.Equals(TestData.Candidate215Party));
            Assert.IsTrue(SortedInfo[1].FirstName.Equals(TestData.Candidate215FirstName));
            Assert.IsTrue(SortedInfo[1].MiddleName.Equals(TestData.Candidate215MiddleName));
            Assert.IsTrue(SortedInfo[1].LastName.Equals(TestData.Candidate215LastName));
            Assert.IsTrue(SortedInfo[1].ContestId.RaceType.Equals(RaceTypeEnum.StateSenate));
            Assert.IsTrue(SortedInfo[1].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[1].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[1].ContestId.DistrictId.Equals(28));
            #endregion CandidateId 215

            #region CandidateId 216
            Assert.IsTrue(SortedInfo[2].CandidateId.Equals(TestData.Candidate216Id));
            Assert.IsTrue(SortedInfo[2].DisplayName.Equals(TestData.Candidate216DisplayName));
            Assert.IsTrue(SortedInfo[2].Party.Equals(TestData.Candidate216Party));
            Assert.IsTrue(SortedInfo[2].FirstName.Equals(TestData.Candidate216FirstName));
            Assert.IsTrue(SortedInfo[2].MiddleName.Equals(TestData.Candidate216MiddleName));
            Assert.IsTrue(SortedInfo[2].LastName.Equals(TestData.Candidate216LastName));
            Assert.IsTrue(SortedInfo[2].ContestId.RaceType.Equals(RaceTypeEnum.StateSenate));
            Assert.IsTrue(SortedInfo[2].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[2].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[2].ContestId.DistrictId.Equals(28));
            #endregion CandidateId 216

            #region CandidateId 217
            Assert.IsTrue(SortedInfo[3].CandidateId.Equals(TestData.Candidate217Id));
            Assert.IsTrue(SortedInfo[3].DisplayName.Equals(TestData.Candidate217DisplayName));
            Assert.IsTrue(SortedInfo[3].Party.Equals(TestData.Candidate217Party));
            Assert.IsTrue(SortedInfo[3].FirstName.Equals(TestData.Candidate217FirstName));
            Assert.IsTrue(SortedInfo[3].MiddleName.Equals(TestData.Candidate217MiddleName));
            Assert.IsTrue(SortedInfo[3].LastName.Equals(TestData.Candidate217LastName));
            Assert.IsTrue(SortedInfo[3].ContestId.RaceType.Equals(RaceTypeEnum.StateSenate));
            Assert.IsTrue(SortedInfo[3].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[3].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[3].ContestId.DistrictId.Equals(28));
            #endregion CandidateId 217

            #region CandidateId 218
            Assert.IsTrue(SortedInfo[4].CandidateId.Equals(TestData.Candidate218Id));
            Assert.IsTrue(SortedInfo[4].DisplayName.Equals(TestData.Candidate218DisplayName));
            Assert.IsTrue(SortedInfo[4].Party.Equals(TestData.Candidate218Party));
            Assert.IsTrue(SortedInfo[4].FirstName.Equals(TestData.Candidate218FirstName));
            Assert.IsTrue(SortedInfo[4].MiddleName.Equals(TestData.Candidate218MiddleName));
            Assert.IsTrue(SortedInfo[4].LastName.Equals(TestData.Candidate218LastName));
            Assert.IsTrue(SortedInfo[4].ContestId.RaceType.Equals(RaceTypeEnum.StateSenate));
            Assert.IsTrue(SortedInfo[4].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[4].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[4].ContestId.DistrictId.Equals(28));
            #endregion CandidateId 218

            #region CandidateId 231
            Assert.IsTrue(SortedInfo[5].CandidateId.Equals(TestData.Candidate231Id));
            Assert.IsTrue(SortedInfo[5].DisplayName.Equals(TestData.Candidate231DisplayName));
            Assert.IsTrue(SortedInfo[5].Party.Equals(TestData.Candidate231Party));
            Assert.IsTrue(SortedInfo[5].FirstName.Equals(TestData.Candidate231FirstName));
            Assert.IsTrue(SortedInfo[5].MiddleName.Equals(TestData.Candidate231MiddleName));
            Assert.IsTrue(SortedInfo[5].LastName.Equals(TestData.Candidate231LastName));
            Assert.IsTrue(SortedInfo[5].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[5].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[5].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[5].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 231

            #region CandidateId 232
            Assert.IsTrue(SortedInfo[6].CandidateId.Equals(TestData.Candidate232Id));
            Assert.IsTrue(SortedInfo[6].DisplayName.Equals(TestData.Candidate232DisplayName));
            Assert.IsTrue(SortedInfo[6].Party.Equals(TestData.Candidate232Party));
            Assert.IsTrue(SortedInfo[6].FirstName.Equals(TestData.Candidate232FirstName));
            Assert.IsTrue(SortedInfo[6].MiddleName.Equals(TestData.Candidate232MiddleName));
            Assert.IsTrue(SortedInfo[6].LastName.Equals(TestData.Candidate232LastName));
            Assert.IsTrue(SortedInfo[6].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[6].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[6].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[6].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 232

            #region CandidateId 233
            Assert.IsTrue(SortedInfo[7].CandidateId.Equals(TestData.Candidate233Id));
            Assert.IsTrue(SortedInfo[7].DisplayName.Equals(TestData.Candidate233DisplayName));
            Assert.IsTrue(SortedInfo[7].Party.Equals(TestData.Candidate233Party));
            Assert.IsTrue(SortedInfo[7].FirstName.Equals(TestData.Candidate233FirstName));
            Assert.IsTrue(SortedInfo[7].MiddleName.Equals(TestData.Candidate233MiddleName));
            Assert.IsTrue(SortedInfo[7].LastName.Equals(TestData.Candidate233LastName));
            Assert.IsTrue(SortedInfo[7].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[7].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[7].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[7].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 233

            #region CandidateId 234
            Assert.IsTrue(SortedInfo[8].CandidateId.Equals(TestData.Candidate234Id));
            Assert.IsTrue(SortedInfo[8].DisplayName.Equals(TestData.Candidate234DisplayName));
            Assert.IsTrue(SortedInfo[8].Party.Equals(TestData.Candidate234Party));
            Assert.IsTrue(SortedInfo[8].FirstName.Equals(TestData.Candidate234FirstName));
            Assert.IsTrue(SortedInfo[8].MiddleName.Equals(TestData.Candidate234MiddleName));
            Assert.IsTrue(SortedInfo[8].LastName.Equals(TestData.Candidate234LastName));
            Assert.IsTrue(SortedInfo[8].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[8].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[8].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[8].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 234

            #region CandidateId 235
            Assert.IsTrue(SortedInfo[9].CandidateId.Equals(TestData.Candidate235Id));
            Assert.IsTrue(SortedInfo[9].DisplayName.Equals(TestData.Candidate235DisplayName));
            Assert.IsTrue(SortedInfo[9].Party.Equals(TestData.Candidate235Party));
            Assert.IsTrue(SortedInfo[9].FirstName.Equals(TestData.Candidate235FirstName));
            Assert.IsTrue(SortedInfo[9].MiddleName.Equals(TestData.Candidate235MiddleName));
            Assert.IsTrue(SortedInfo[9].LastName.Equals(TestData.Candidate235LastName));
            Assert.IsTrue(SortedInfo[9].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[9].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[9].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[9].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 235

            #region CandidateId 236
            Assert.IsTrue(SortedInfo[10].CandidateId.Equals(TestData.Candidate236Id));
            Assert.IsTrue(SortedInfo[10].DisplayName.Equals(TestData.Candidate236DisplayName));
            Assert.IsTrue(SortedInfo[10].Party.Equals(TestData.Candidate236Party));
            Assert.IsTrue(SortedInfo[10].FirstName.Equals(TestData.Candidate236FirstName));
            Assert.IsTrue(SortedInfo[10].MiddleName.Equals(TestData.Candidate236MiddleName));
            Assert.IsTrue(SortedInfo[10].LastName.Equals(TestData.Candidate236LastName));
            Assert.IsTrue(SortedInfo[10].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[10].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[10].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[10].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 236

            #region CandidateId 237
            Assert.IsTrue(SortedInfo[11].CandidateId.Equals(TestData.Candidate237Id));
            Assert.IsTrue(SortedInfo[11].DisplayName.Equals(TestData.Candidate237DisplayName));
            Assert.IsTrue(SortedInfo[11].Party.Equals(TestData.Candidate237Party));
            Assert.IsTrue(SortedInfo[11].FirstName.Equals(TestData.Candidate237FirstName));
            Assert.IsTrue(SortedInfo[11].MiddleName.Equals(TestData.Candidate237MiddleName));
            Assert.IsTrue(SortedInfo[11].LastName.Equals(TestData.Candidate237LastName));
            Assert.IsTrue(SortedInfo[11].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[11].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[11].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[11].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 237

            #region CandidateId 238
            Assert.IsTrue(SortedInfo[12].CandidateId.Equals(TestData.Candidate238Id));
            Assert.IsTrue(SortedInfo[12].DisplayName.Equals(TestData.Candidate238DisplayName));
            Assert.IsTrue(SortedInfo[12].Party.Equals(TestData.Candidate238Party));
            Assert.IsTrue(SortedInfo[12].FirstName.Equals(TestData.Candidate238FirstName));
            Assert.IsTrue(SortedInfo[12].MiddleName.Equals(TestData.Candidate238MiddleName));
            Assert.IsTrue(SortedInfo[12].LastName.Equals(TestData.Candidate238LastName));
            Assert.IsTrue(SortedInfo[12].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[12].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[12].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[12].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 238

            #region CandidateId 239
            Assert.IsTrue(SortedInfo[13].CandidateId.Equals(TestData.Candidate239Id));
            Assert.IsTrue(SortedInfo[13].DisplayName.Equals(TestData.Candidate239DisplayName));
            Assert.IsTrue(SortedInfo[13].Party.Equals(TestData.Candidate239Party));
            Assert.IsTrue(SortedInfo[13].FirstName.Equals(TestData.Candidate239FirstName));
            Assert.IsTrue(SortedInfo[13].MiddleName.Equals(TestData.Candidate239MiddleName));
            Assert.IsTrue(SortedInfo[13].LastName.Equals(TestData.Candidate239LastName));
            Assert.IsTrue(SortedInfo[13].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[13].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[13].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[13].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 239

            #region CandidateId 240
            Assert.IsTrue(SortedInfo[14].CandidateId.Equals(TestData.Candidate240Id));
            Assert.IsTrue(SortedInfo[14].DisplayName.Equals(TestData.Candidate240DisplayName));
            Assert.IsTrue(SortedInfo[14].Party.Equals(TestData.Candidate240Party));
            Assert.IsTrue(SortedInfo[14].FirstName.Equals(TestData.Candidate240FirstName));
            Assert.IsTrue(SortedInfo[14].MiddleName.Equals(TestData.Candidate240MiddleName));
            Assert.IsTrue(SortedInfo[14].LastName.Equals(TestData.Candidate240LastName));
            Assert.IsTrue(SortedInfo[14].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[14].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[14].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[14].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 240

            #region CandidateId 241
            Assert.IsTrue(SortedInfo[15].CandidateId.Equals(TestData.Candidate241Id));
            Assert.IsTrue(SortedInfo[15].DisplayName.Equals(TestData.Candidate241DisplayName));
            Assert.IsTrue(SortedInfo[15].Party.Equals(TestData.Candidate241Party));
            Assert.IsTrue(SortedInfo[15].FirstName.Equals(TestData.Candidate241FirstName));
            Assert.IsTrue(SortedInfo[15].MiddleName.Equals(TestData.Candidate241MiddleName));
            Assert.IsTrue(SortedInfo[15].LastName.Equals(TestData.Candidate241LastName));
            Assert.IsTrue(SortedInfo[15].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[15].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[15].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[15].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 241

            #region CandidateId 242
            Assert.IsTrue(SortedInfo[16].CandidateId.Equals(TestData.Candidate242Id));
            Assert.IsTrue(SortedInfo[16].DisplayName.Equals(TestData.Candidate242DisplayName));
            Assert.IsTrue(SortedInfo[16].Party.Equals(TestData.Candidate242Party));
            Assert.IsTrue(SortedInfo[16].FirstName.Equals(TestData.Candidate242FirstName));
            Assert.IsTrue(SortedInfo[16].MiddleName.Equals(TestData.Candidate242MiddleName));
            Assert.IsTrue(SortedInfo[16].LastName.Equals(TestData.Candidate242LastName));
            Assert.IsTrue(SortedInfo[16].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(SortedInfo[16].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(SortedInfo[16].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(SortedInfo[16].ContestId.DistrictId.Equals(25));
            #endregion CandidateId 242

        }

        [Test]
        public void UploadValidCMessage()
        {
            MessageController.UploadCMessage(TestData.Cmsg, "C20SE");
            ValidateCMessage(CMessageProcessor.CMessage);
        }
        #endregion CMessage

        #region VMessage
        public static void ValidateVMessage(VMessage vMessage) {

            Assert.IsTrue(vMessage.JudgeRecords.Count.Equals(0));
            Assert.IsTrue(vMessage.PropRecords.Count.Equals(0));
            Assert.IsTrue(vMessage.RecallRecords.Count.Equals(0));


            Assert.IsTrue(vMessage.aRecords.Count.Equals(4));
            Assert.IsTrue(vMessage.CandidateResultRecords.Count.Equals(5));

            #region Validate A Records
            List<ARecord> SortedARecords = vMessage.aRecords.OrderBy(x => x.County).ToList();

            Assert.IsTrue(SortedARecords[0].County.Equals(CountyTypeEnum.LosAngeles));
            Assert.IsTrue(SortedARecords[0].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(SortedARecords[0].ReportNumber.Equals(0));
            Assert.IsTrue(SortedARecords[0].CorrectionReportNumber.Equals(0));
            Assert.IsTrue(SortedARecords[0].PrecinctReporting.Equals(180));
            Assert.IsTrue(SortedARecords[0].TotalNumberOfPrecinct.Equals(180));
            Assert.IsTrue(SortedARecords[0].PrecinctReportingPercentage.Equals(0.0));
            Assert.IsTrue(SortedARecords[0].VoterTurnout.Equals(0));
            Assert.IsTrue(SortedARecords[0].TotalVoters.Equals(0));
            Assert.IsTrue(SortedARecords[0].VoterTurnoutPercentage.Equals(0.0));

            Assert.IsTrue(SortedARecords[1].County.Equals(CountyTypeEnum.Riverside));
            Assert.IsTrue(SortedARecords[1].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(SortedARecords[1].ReportNumber.Equals(0));
            Assert.IsTrue(SortedARecords[1].CorrectionReportNumber.Equals(0));
            Assert.IsTrue(SortedARecords[1].PrecinctReporting.Equals(402));
            Assert.IsTrue(SortedARecords[1].TotalNumberOfPrecinct.Equals(402));
            Assert.IsTrue(SortedARecords[1].PrecinctReportingPercentage.Equals(0.0));
            Assert.IsTrue(SortedARecords[1].VoterTurnout.Equals(0));
            Assert.IsTrue(SortedARecords[1].TotalVoters.Equals(0));
            Assert.IsTrue(SortedARecords[1].VoterTurnoutPercentage.Equals(0.0));

            Assert.IsTrue(SortedARecords[2].County.Equals(CountyTypeEnum.Ventura));
            Assert.IsTrue(SortedARecords[2].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(SortedARecords[2].ReportNumber.Equals(0));
            Assert.IsTrue(SortedARecords[2].CorrectionReportNumber.Equals(0));
            Assert.IsTrue(SortedARecords[2].PrecinctReporting.Equals(72));
            Assert.IsTrue(SortedARecords[2].TotalNumberOfPrecinct.Equals(72));
            Assert.IsTrue(SortedARecords[2].PrecinctReportingPercentage.Equals(0.0));
            Assert.IsTrue(SortedARecords[2].VoterTurnout.Equals(0));
            Assert.IsTrue(SortedARecords[2].TotalVoters.Equals(0));
            Assert.IsTrue(SortedARecords[2].VoterTurnoutPercentage.Equals(0.0));

            Assert.IsTrue(SortedARecords[3].County.Equals(CountyTypeEnum.Statewide));
            Assert.IsTrue(SortedARecords[3].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(SortedARecords[3].ReportNumber.Equals(0));
            Assert.IsTrue(SortedARecords[3].CorrectionReportNumber.Equals(0));
            Assert.IsTrue(SortedARecords[3].PrecinctReporting.Equals(654));
            Assert.IsTrue(SortedARecords[3].TotalNumberOfPrecinct.Equals(654));
            Assert.IsTrue(SortedARecords[3].PrecinctReportingPercentage.Equals(0.0));
            Assert.IsTrue(SortedARecords[3].VoterTurnout.Equals(0));
            Assert.IsTrue(SortedARecords[3].TotalVoters.Equals(0));
            Assert.IsTrue(SortedARecords[3].VoterTurnoutPercentage.Equals(0.0));
            #endregion Validate A Records

            #region Validate V Records

            #region StateSentate
            List<InputCandidateRecord> CandidateRecordSet = vMessage.CandidateResultRecords
                                                                                .Where(x => x.ContestId.RaceType.Equals(RaceTypeEnum.StateSenate))
                                                                                .OrderBy(x => x.County).ToList();
            Assert.IsTrue(CandidateRecordSet.Count.Equals(2));

            #region Riverside
            Assert.IsTrue(CandidateRecordSet[0].County.Equals(CountyTypeEnum.Riverside));
            Assert.IsTrue(CandidateRecordSet[0].PrecinctReporting.Equals(402));
            Assert.IsTrue(CandidateRecordSet[0].TotalNumberOfPrecinct.Equals(402));
            Assert.IsTrue(CandidateRecordSet[0].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(CandidateRecordSet[0].ContestId.RaceType.Equals(RaceTypeEnum.StateSenate));
            Assert.IsTrue(CandidateRecordSet[0].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(CandidateRecordSet[0].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(CandidateRecordSet[0].ContestId.DistrictId.Equals(28));

            #region results
            List<CandidateRecord> results = CandidateRecordSet[0].ResultRecords.OrderBy(x => x.CandidateId).ToList();

            Assert.IsTrue(results.Count.Equals(5));

            Assert.IsTrue(results[0].CandidateId.Equals(TestData.Candidate214Id));
            Assert.IsTrue(results[0].Votes.Equals(5912));
            Assert.IsTrue(results[0].PartyPercent.Equals(2.90));
            Assert.IsTrue(results[0].RacePercent.Equals(2.90));

            Assert.IsTrue(results[1].CandidateId.Equals(TestData.Candidate215Id));
            Assert.IsTrue(results[1].Votes.Equals(47516));
            Assert.IsTrue(results[1].PartyPercent.Equals(23.50));
            Assert.IsTrue(results[1].RacePercent.Equals(23.50));

            Assert.IsTrue(results[2].CandidateId.Equals(TestData.Candidate216Id));
            Assert.IsTrue(results[2].Votes.Equals(42222));
            Assert.IsTrue(results[2].PartyPercent.Equals(20.90));
            Assert.IsTrue(results[2].RacePercent.Equals(20.90));

            Assert.IsTrue(results[3].CandidateId.Equals(TestData.Candidate217Id));
            Assert.IsTrue(results[3].Votes.Equals(81918));
            Assert.IsTrue(results[3].PartyPercent.Equals(40.50));
            Assert.IsTrue(results[3].RacePercent.Equals(40.50));

            Assert.IsTrue(results[4].CandidateId.Equals(TestData.Candidate218Id));
            Assert.IsTrue(results[4].Votes.Equals(24536));
            Assert.IsTrue(results[4].PartyPercent.Equals(12.10));
            Assert.IsTrue(results[4].RacePercent.Equals(12.10));

            #endregion results

            #endregion Riverside

            #region Statewide

            Assert.IsTrue(CandidateRecordSet[1].County.Equals(CountyTypeEnum.Statewide));
            Assert.IsTrue(CandidateRecordSet[1].PrecinctReporting.Equals(402));
            Assert.IsTrue(CandidateRecordSet[1].TotalNumberOfPrecinct.Equals(402));
            Assert.IsTrue(CandidateRecordSet[1].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(CandidateRecordSet[1].ContestId.RaceType.Equals(RaceTypeEnum.StateSenate));
            Assert.IsTrue(CandidateRecordSet[1].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(CandidateRecordSet[1].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(CandidateRecordSet[1].ContestId.DistrictId.Equals(28));

            #region results
            results = CandidateRecordSet[1].ResultRecords.OrderBy(x => x.CandidateId).ToList();

            Assert.IsTrue(results.Count.Equals(5));

            Assert.IsTrue(results[0].CandidateId.Equals(TestData.Candidate214Id));
            Assert.IsTrue(results[0].Votes.Equals(5912));
            Assert.IsTrue(results[0].PartyPercent.Equals(2.9));
            Assert.IsTrue(results[0].RacePercent.Equals(2.9));

            Assert.IsTrue(results[1].CandidateId.Equals(TestData.Candidate215Id));
            Assert.IsTrue(results[1].Votes.Equals(47516));
            Assert.IsTrue(results[1].PartyPercent.Equals(23.5));
            Assert.IsTrue(results[1].RacePercent.Equals(23.5));

            Assert.IsTrue(results[2].CandidateId.Equals(TestData.Candidate216Id));
            Assert.IsTrue(results[2].Votes.Equals(42222));
            Assert.IsTrue(results[2].PartyPercent.Equals(20.9));
            Assert.IsTrue(results[2].RacePercent.Equals(20.9));

            Assert.IsTrue(results[3].CandidateId.Equals(TestData.Candidate217Id));
            Assert.IsTrue(results[3].Votes.Equals(81918));
            Assert.IsTrue(results[3].PartyPercent.Equals(40.5));
            Assert.IsTrue(results[3].RacePercent.Equals(40.5));

            Assert.IsTrue(results[4].CandidateId.Equals(TestData.Candidate218Id));
            Assert.IsTrue(results[4].Votes.Equals(24536));
            Assert.IsTrue(results[4].PartyPercent.Equals(12.1));
            Assert.IsTrue(results[4].RacePercent.Equals(12.1));
            #endregion results

            #endregion Statewide

            #endregion StateSenate

            #region USRep

            CandidateRecordSet = vMessage.CandidateResultRecords
                                                            .Where(x => x.ContestId.RaceType.Equals(RaceTypeEnum.Usrep))
                                                            .OrderBy(x => x.County).ToList();
            Assert.IsTrue(CandidateRecordSet.Count.Equals(3));

            #region LA
            Assert.IsTrue(CandidateRecordSet[0].County.Equals(CountyTypeEnum.LosAngeles));
            Assert.IsTrue(CandidateRecordSet[0].PrecinctReporting.Equals(180));
            Assert.IsTrue(CandidateRecordSet[0].TotalNumberOfPrecinct.Equals(180));
            Assert.IsTrue(CandidateRecordSet[0].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(CandidateRecordSet[0].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(CandidateRecordSet[0].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(CandidateRecordSet[0].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(CandidateRecordSet[0].ContestId.DistrictId.Equals(25));

            #region Results
            results = CandidateRecordSet[0].ResultRecords.OrderBy(x => x.CandidateId).ToList();

            Assert.IsTrue(results.Count.Equals(12));

            Assert.IsTrue(results[0].CandidateId.Equals(TestData.Candidate231Id));
            Assert.IsTrue(results[0].Votes.Equals(2510));
            Assert.IsTrue(results[0].PartyPercent.Equals(2.00));
            Assert.IsTrue(results[0].RacePercent.Equals(2.00));

            Assert.IsTrue(results[1].CandidateId.Equals(TestData.Candidate232Id));
            Assert.IsTrue(results[1].Votes.Equals(1272));
            Assert.IsTrue(results[1].PartyPercent.Equals(1.00));
            Assert.IsTrue(results[1].RacePercent.Equals(1.00));

            Assert.IsTrue(results[2].CandidateId.Equals(TestData.Candidate233Id));
            Assert.IsTrue(results[2].Votes.Equals(919));
            Assert.IsTrue(results[2].PartyPercent.Equals(0.7));
            Assert.IsTrue(results[2].RacePercent.Equals(0.7));

            Assert.IsTrue(results[3].CandidateId.Equals(TestData.Candidate234Id));
            Assert.IsTrue(results[3].Votes.Equals(45011));
            Assert.IsTrue(results[3].PartyPercent.Equals(36.00));
            Assert.IsTrue(results[3].RacePercent.Equals(36.00));

            Assert.IsTrue(results[4].CandidateId.Equals(TestData.Candidate235Id));
            Assert.IsTrue(results[4].Votes.Equals(8725));
            Assert.IsTrue(results[4].PartyPercent.Equals(7.0));
            Assert.IsTrue(results[4].RacePercent.Equals(7.0));

            Assert.IsTrue(results[5].CandidateId.Equals(TestData.Candidate236Id));
            Assert.IsTrue(results[5].Votes.Equals(6387));
            Assert.IsTrue(results[5].PartyPercent.Equals(5.10));
            Assert.IsTrue(results[5].RacePercent.Equals(5.10));

            Assert.IsTrue(results[6].CandidateId.Equals(TestData.Candidate237Id));
            Assert.IsTrue(results[6].Votes.Equals(29121));
            Assert.IsTrue(results[6].PartyPercent.Equals(23.30));
            Assert.IsTrue(results[6].RacePercent.Equals(23.30));

            Assert.IsTrue(results[7].CandidateId.Equals(TestData.Candidate238Id));
            Assert.IsTrue(results[7].Votes.Equals(2122));
            Assert.IsTrue(results[7].PartyPercent.Equals(1.70));
            Assert.IsTrue(results[7].RacePercent.Equals(1.70));

            Assert.IsTrue(results[8].CandidateId.Equals(TestData.Candidate239Id));
            Assert.IsTrue(results[8].Votes.Equals(21897));
            Assert.IsTrue(results[8].PartyPercent.Equals(17.50));
            Assert.IsTrue(results[8].RacePercent.Equals(17.50));

            Assert.IsTrue(results[9].CandidateId.Equals(TestData.Candidate240Id));
            Assert.IsTrue(results[9].Votes.Equals(2554));
            Assert.IsTrue(results[9].PartyPercent.Equals(2.00));
            Assert.IsTrue(results[9].RacePercent.Equals(2.00));

            Assert.IsTrue(results[10].CandidateId.Equals(TestData.Candidate241Id));
            Assert.IsTrue(results[10].Votes.Equals(2357));
            Assert.IsTrue(results[10].PartyPercent.Equals(1.90));
            Assert.IsTrue(results[10].RacePercent.Equals(1.90));

            Assert.IsTrue(results[11].CandidateId.Equals(TestData.Candidate242Id));
            Assert.IsTrue(results[11].Votes.Equals(2231));
            Assert.IsTrue(results[11].PartyPercent.Equals(1.80));
            Assert.IsTrue(results[11].RacePercent.Equals(1.80));
            #endregion Results

            #endregion LA

            #region ventura

            Assert.IsTrue(CandidateRecordSet[1].County.Equals(CountyTypeEnum.Ventura));
            Assert.IsTrue(CandidateRecordSet[1].PrecinctReporting.Equals(72));
            Assert.IsTrue(CandidateRecordSet[1].TotalNumberOfPrecinct.Equals(72));
            Assert.IsTrue(CandidateRecordSet[1].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(CandidateRecordSet[1].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(CandidateRecordSet[1].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(CandidateRecordSet[1].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(CandidateRecordSet[1].ContestId.DistrictId.Equals(25));

            #region Results
            results = CandidateRecordSet[1].ResultRecords.OrderBy(x => x.CandidateId).ToList();

            Assert.IsTrue(results.Count.Equals(12));

            Assert.IsTrue(results[0].CandidateId.Equals(TestData.Candidate231Id));
            Assert.IsTrue(results[0].Votes.Equals(398));
            Assert.IsTrue(results[0].PartyPercent.Equals(1.20));
            Assert.IsTrue(results[0].RacePercent.Equals(1.20));

            Assert.IsTrue(results[1].CandidateId.Equals(TestData.Candidate232Id));
            Assert.IsTrue(results[1].Votes.Equals(117));
            Assert.IsTrue(results[1].PartyPercent.Equals(0.30));
            Assert.IsTrue(results[1].RacePercent.Equals(0.30));

            Assert.IsTrue(results[2].CandidateId.Equals(TestData.Candidate233Id));
            Assert.IsTrue(results[2].Votes.Equals(143));
            Assert.IsTrue(results[2].PartyPercent.Equals(0.40));
            Assert.IsTrue(results[2].RacePercent.Equals(0.40));

            Assert.IsTrue(results[3].CandidateId.Equals(TestData.Candidate234Id));
            Assert.IsTrue(results[3].Votes.Equals(12412));
            Assert.IsTrue(results[3].PartyPercent.Equals(36.80));
            Assert.IsTrue(results[3].RacePercent.Equals(36.80));

            Assert.IsTrue(results[4].CandidateId.Equals(TestData.Candidate235Id));
            Assert.IsTrue(results[4].Votes.Equals(1666));
            Assert.IsTrue(results[4].PartyPercent.Equals(4.9));
            Assert.IsTrue(results[4].RacePercent.Equals(4.9));

            Assert.IsTrue(results[5].CandidateId.Equals(TestData.Candidate236Id));
            Assert.IsTrue(results[5].Votes.Equals(827));
            Assert.IsTrue(results[5].PartyPercent.Equals(2.50));
            Assert.IsTrue(results[5].RacePercent.Equals(2.50));

            Assert.IsTrue(results[6].CandidateId.Equals(TestData.Candidate237Id));
            Assert.IsTrue(results[6].Votes.Equals(11190));
            Assert.IsTrue(results[6].PartyPercent.Equals(33.20));
            Assert.IsTrue(results[6].RacePercent.Equals(33.20));

            Assert.IsTrue(results[7].CandidateId.Equals(TestData.Candidate238Id));
            Assert.IsTrue(results[7].Votes.Equals(376));
            Assert.IsTrue(results[7].PartyPercent.Equals(1.10));
            Assert.IsTrue(results[7].RacePercent.Equals(1.10));

            Assert.IsTrue(results[8].CandidateId.Equals(TestData.Candidate239Id));
            Assert.IsTrue(results[8].Votes.Equals(5475));
            Assert.IsTrue(results[8].PartyPercent.Equals(16.20));
            Assert.IsTrue(results[8].RacePercent.Equals(16.20));

            Assert.IsTrue(results[9].CandidateId.Equals(TestData.Candidate240Id));
            Assert.IsTrue(results[9].Votes.Equals(476));
            Assert.IsTrue(results[9].PartyPercent.Equals(1.40));
            Assert.IsTrue(results[9].RacePercent.Equals(1.40));

            Assert.IsTrue(results[10].CandidateId.Equals(TestData.Candidate241Id));
            Assert.IsTrue(results[10].Votes.Equals(369));
            Assert.IsTrue(results[10].PartyPercent.Equals(1.10));
            Assert.IsTrue(results[10].RacePercent.Equals(1.10));

            Assert.IsTrue(results[11].CandidateId.Equals(TestData.Candidate242Id));
            Assert.IsTrue(results[11].Votes.Equals(294));
            Assert.IsTrue(results[11].PartyPercent.Equals(0.90));
            Assert.IsTrue(results[11].RacePercent.Equals(0.90));
            #endregion Results

            #endregion ventura

            #region Statewide

            Assert.IsTrue(CandidateRecordSet[2].County.Equals(CountyTypeEnum.Statewide));
            Assert.IsTrue(CandidateRecordSet[2].PrecinctReporting.Equals(252));
            Assert.IsTrue(CandidateRecordSet[2].TotalNumberOfPrecinct.Equals(252));
            Assert.IsTrue(CandidateRecordSet[2].ReportType.Equals(ReportingTypeEnum.Unknown));
            Assert.IsTrue(CandidateRecordSet[2].ContestId.RaceType.Equals(RaceTypeEnum.Usrep));
            Assert.IsTrue(CandidateRecordSet[2].ContestId.ElectionType.Equals(ElectionTypeEnum.Special));
            Assert.IsTrue(CandidateRecordSet[2].ContestId.PartyType.Equals(PartyTypeEnum.NotAPartyRace));
            Assert.IsTrue(CandidateRecordSet[2].ContestId.DistrictId.Equals(25));

            #region Results
            results = CandidateRecordSet[2].ResultRecords.OrderBy(x => x.CandidateId).ToList();

            Assert.IsTrue(results.Count.Equals(12));

            Assert.IsTrue(results[0].CandidateId.Equals(TestData.Candidate231Id));
            Assert.IsTrue(results[0].Votes.Equals(2908));
            Assert.IsTrue(results[0].PartyPercent.Equals(1.80));
            Assert.IsTrue(results[0].RacePercent.Equals(1.80));

            Assert.IsTrue(results[1].CandidateId.Equals(TestData.Candidate232Id));
            Assert.IsTrue(results[1].Votes.Equals(1389));
            Assert.IsTrue(results[1].PartyPercent.Equals(0.90));
            Assert.IsTrue(results[1].RacePercent.Equals(0.90));

            Assert.IsTrue(results[2].CandidateId.Equals(TestData.Candidate233Id));
            Assert.IsTrue(results[2].Votes.Equals(1062));
            Assert.IsTrue(results[2].PartyPercent.Equals(0.70));
            Assert.IsTrue(results[2].RacePercent.Equals(0.70));

            Assert.IsTrue(results[3].CandidateId.Equals(TestData.Candidate234Id));
            Assert.IsTrue(results[3].Votes.Equals(57423));
            Assert.IsTrue(results[3].PartyPercent.Equals(36.10));
            Assert.IsTrue(results[3].RacePercent.Equals(36.10));

            Assert.IsTrue(results[4].CandidateId.Equals(TestData.Candidate235Id));
            Assert.IsTrue(results[4].Votes.Equals(10391));
            Assert.IsTrue(results[4].PartyPercent.Equals(6.5));
            Assert.IsTrue(results[4].RacePercent.Equals(6.5));

            Assert.IsTrue(results[5].CandidateId.Equals(TestData.Candidate236Id));
            Assert.IsTrue(results[5].Votes.Equals(7214));
            Assert.IsTrue(results[5].PartyPercent.Equals(4.50));
            Assert.IsTrue(results[5].RacePercent.Equals(4.50));

            Assert.IsTrue(results[6].CandidateId.Equals(TestData.Candidate237Id));
            Assert.IsTrue(results[6].Votes.Equals(40311));
            Assert.IsTrue(results[6].PartyPercent.Equals(25.40));
            Assert.IsTrue(results[6].RacePercent.Equals(25.40));

            Assert.IsTrue(results[7].CandidateId.Equals(TestData.Candidate238Id));
            Assert.IsTrue(results[7].Votes.Equals(2498));
            Assert.IsTrue(results[7].PartyPercent.Equals(1.60));
            Assert.IsTrue(results[7].RacePercent.Equals(1.60));

            Assert.IsTrue(results[8].CandidateId.Equals(TestData.Candidate239Id));
            Assert.IsTrue(results[8].Votes.Equals(27372));
            Assert.IsTrue(results[8].PartyPercent.Equals(17.20));
            Assert.IsTrue(results[8].RacePercent.Equals(17.20));

            Assert.IsTrue(results[9].CandidateId.Equals(TestData.Candidate240Id));
            Assert.IsTrue(results[9].Votes.Equals(3030));
            Assert.IsTrue(results[9].PartyPercent.Equals(1.90));
            Assert.IsTrue(results[9].RacePercent.Equals(1.90));

            Assert.IsTrue(results[10].CandidateId.Equals(TestData.Candidate241Id));
            Assert.IsTrue(results[10].Votes.Equals(2726));
            Assert.IsTrue(results[10].PartyPercent.Equals(1.70));
            Assert.IsTrue(results[10].RacePercent.Equals(1.70));

            Assert.IsTrue(results[11].CandidateId.Equals(TestData.Candidate242Id));
            Assert.IsTrue(results[11].Votes.Equals(2525));
            Assert.IsTrue(results[11].PartyPercent.Equals(1.60));
            Assert.IsTrue(results[11].RacePercent.Equals(1.60));
            #endregion Results

            #endregion Statewide

            #endregion USRep

            #endregion Validate V Records
        }
        [Test]
        public void UploadValidVMessage()
        {
            MessageController.UploadRMessage(TestData.Rmsg, "R20SE");
            MessageController.UploadCMessage(TestData.Cmsg, "C20SE");
            MessageController.UploadVMessage(TestData.Vmsg, "V20SE");
            ValidateVMessage(VMessageProcessor.VMessage);
        }
        #endregion VMessage
    }
}