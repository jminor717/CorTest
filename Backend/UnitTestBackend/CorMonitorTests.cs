using Microsoft.VisualStudio.TestTools.UnitTesting;
using MobileBackend.corMonitoring;
using MobileBackend.Models;
using NUnit.Framework.Internal;
using Microsoft.EntityFrameworkCore;

namespace UnitTestBackend {
    [TestClass]
    public class CorMonitorTests {
        [TestMethod]
        public void TestMethod1() {
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void TsesInitililize() {
            var opt = new DbContextOptionsBuilder<MobileBackendContext>().UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MobileBackendTestContext;Trusted_Connection=True;MultipleActiveResultSets=true");
            var options = new DbContextOptions<MobileBackendContext>();
            
            var controller = new CorMonitorService(new MobileBackendContext(options));

           
        }
    }
}
