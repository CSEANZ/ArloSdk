using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities.Product;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Xamling.Azure.Portable.Contract.Cache;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class CacheTests : TestBase
    {
        [TestMethod]
        public async Task SimpleCacheTest()
        {
            var cache = Resolve<IRedisEntityCache>();

            await cache.Delete<ArloEvent>("test");

            var d = await cache.GetEntity<ArloEvent>("test");

            Assert.IsNull(d);

            var arloItem = new ArloEvent
            {
                Description = "test"
            };

            var write = await cache.SetEntity("test", arloItem);

            d = await cache.GetEntity<ArloEvent>("test");

            Assert.IsNotNull(d);

            await cache.Delete<ArloEvent>("test");

            d = await cache.GetEntity<ArloEvent>("test");

            Assert.IsNull(d);
        }
    }
}
