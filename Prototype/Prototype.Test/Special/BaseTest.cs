using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using SpecialElection.Data;
using SpecialElection.Service;
using Prototype.Test.Utility;

namespace Prototype.Test.Special
{
    [TestFixture]
    public abstract class BaseTest
    {
        protected ApplicationDbService applicationDbService; 
        protected MessageService messageService;
        protected IMessageClient messageClient;

        [SetUp]
        public virtual void Setup()
        {
            ApplicationDbContext context = SpecialElectionTestUtility.CreateTestApplcationDbContext();
            applicationDbService = new ApplicationDbService(context);

            messageClient = new MockMessageClient();
            messageService = new MessageService(applicationDbService, messageClient);
        }
    }
}
