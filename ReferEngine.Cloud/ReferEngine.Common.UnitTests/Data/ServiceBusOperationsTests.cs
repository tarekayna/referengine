using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferEngine.Common.Data;
using ReferEngine.Common.Data.MixPanel;
using ReferEngine.Common.Models;

namespace ReferEngine.Common.UnitTests.Data
{
    [TestClass]
    public class ServiceBusOperationsTests
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            ServiceBusOperations.Initialize();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {

        }

        [TestMethod]
        public void TestBasicConnectivity()
        {
            //PrivateBetaSignup signup = new PrivateBetaSignup("tarek990@gmail.com");
            //ServiceBusOperations.AddToQueue(signup);
        }
    }
}
