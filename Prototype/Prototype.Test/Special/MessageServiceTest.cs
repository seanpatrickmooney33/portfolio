using System;
using System.Collections.Generic;
using System.Text;

using NUnit.Framework;
using System.Threading.Tasks;
using Prototype.Test.Utility;

namespace Prototype.Test.Special
{
    public class MessageServiceTest : BaseTest
    {
        #region RMSG

        [Test]
        public async Task CreateRMSG()
        {
            await SpecialElectionTestUtility.PopulateWithRaceData(applicationDbService);

            String result = await messageService.GenerateRMSG();

            String[] temp = result.Split("\n");
            List<String> parts = new List<String>();
            for (int j = 1; j < temp.Length - 2; j++)
            {
                parts.Add(temp[j]);
            }

            temp = TestData.Rmsg.Split("\n");
            List<String> cParts = new List<String>();
            for (int j = 1; j < temp.Length - 2; j++)
            {
                cParts.Add(temp[j]);
            }

            Assert.IsTrue(parts.Count.Equals(cParts.Count));

            for (int i = 0; i < parts.Count; i++)
            {
                Assert.IsTrue(parts[i].Equals(cParts[i]), "failed at index [" + i + "]");
            }

        }
        [Test]
        public async Task CreateRMSGWithoutRaces() { }
        [Test]
        public async Task CreateRMSGWithoutElection() { }

        #endregion RMSG

        #region CMSG
        [Test]
        public async Task CreateCMSG()
        {
            await SpecialElectionTestUtility.PopulateWithCandidateData(applicationDbService);
            String result = await messageService.GenerateCMSG();

            String[] temp = result.Split("\n");
            List<String> parts = new List<String>();
            for (int j = 1; j < temp.Length - 2; j++)
            {
                parts.Add(temp[j]);
            }

            temp = TestData.Cmsg.Split("\n");
            List<String> cParts = new List<String>();
            for (int j = 1; j < temp.Length - 2; j++)
            {
                cParts.Add(temp[j]);
            }

            Assert.IsTrue(parts.Count.Equals(cParts.Count));

            for (int i = 0; i < parts.Count; i++)
            {
                Assert.IsTrue(parts[i].Equals(cParts[i]), "failed at index [" + i + "]");
            }
        }
        [Test]
        public async Task CreateCMSGWithoutResults() { }
        [Test]
        public async Task CreateCMSGWithoutElection() { }
        [Test]
        public async Task CreateCMSGWithoutRace() { }
        [Test]
        public async Task CreateCMSGWithoutCandidates() { }

        #endregion CMSG

        #region VMSG
        //[Test]
        //public async Task PostVMSG()
        //{
        //    //await this.messageClient.SendMessage(new SpecialElection.Service.PayLoad()
        //    //{
        //    //    Verb = SpecialElection.Service.APIVerb.upload,
        //    //    FilePrefix = "V",
        //    //    Message = Utilities.Data.Vmsg
        //    //});
        //}

        [Test]
        public async Task CreateVMSG()
        {
            await SpecialElectionTestUtility.PopulateWtihCandidateResults(applicationDbService);

            String result = await messageService.GenerateVMSG();

            String[] temp = result.Split("\n");
            List<String> parts = new List<String>();
            for (int j = 1; j < temp.Length - 2; j++)
            {
                if (j == 6 || j == 5) { continue; }
                parts.Add(temp[j]);
            }

            temp = TestData.Vmsg.Split("\n");
            List<String> vParts = new List<String>();
            for (int j = 1; j < temp.Length - 2; j++)
            {
                if (j == 6 || j == 5) { continue; }
                vParts.Add(temp[j]);
            }

            Assert.IsTrue(parts.Count.Equals(vParts.Count));

            for (int i = 0; i < parts.Count; i++)
            {
                Assert.IsTrue(parts[i].Equals(vParts[i]), "failed at index [" + i + "]");
            }
        }
        [Test]
        public async Task CreateVMSGWithoutElection() { }
        [Test]
        public async Task CreateVMSGWithoutRace() { }
        [Test]
        public async Task CreateVMSGWithoutCandidates() { }
        [Test]
        public async Task CreateVMSGWithoutResults() { }

        #endregion RMSG
    }
}
