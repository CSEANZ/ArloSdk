using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class SyncTests : TestBase
    {
        [TestMethod]
        public async Task TestSync()
        {
            var service = Resolve<ISyncService>();

            var result = await service.SyncAll();

            Assert.IsTrue(result);

            await Task.Delay(10000);
        }
    }
}
