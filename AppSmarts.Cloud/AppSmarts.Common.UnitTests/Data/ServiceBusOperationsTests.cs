using AppSmarts.Common.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AppSmarts.Common.UnitTests.Data
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
