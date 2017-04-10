using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using EventBot.SupportLibrary.Contract;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class BotAuthTests : TestBase
    {
        [TestMethod]
        public async Task TestGetUrl()
        {
            var authService = Resolve<IAuthService>();

            var url = authService.GetCallbackUrl("SomeCookie");

            Assert.IsNotNull(url);
            Assert.IsTrue(url.ToString().Contains("SomeCookie"));

        }
    }
}
