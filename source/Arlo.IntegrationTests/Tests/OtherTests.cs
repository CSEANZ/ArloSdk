using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class OtherTests : TestBase
    {

        [TestMethod]
        public void TestTime()
        {
            var serice = Resolve<ISessionService>();
            var time = serice.GetCurrentTime();
            
        }
    }
}
