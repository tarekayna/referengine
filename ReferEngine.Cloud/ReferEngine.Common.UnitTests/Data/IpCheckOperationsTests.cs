using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReferEngine.Common.Data;

namespace ReferEngine.Common.UnitTests.Data
{
    [TestClass]
    public class IpCheckOperationsTests
    {
        [TestMethod]
        public void TestIpCheck()
        {
            var ipAddressLocation = IpCheckOperations.CheckIpAddress("67.161.120.13");
            Assert.IsNotNull(ipAddressLocation);
            Assert.AreEqual(ipAddressLocation.City, "Seattle");
            Assert.AreEqual(ipAddressLocation.Region, "Washington");
        }
    }
}
