using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class Humanizetests
    {
        [TestMethod]
        public void TestHumanizeAgo()
        {
            var timeOne = DateTime.Now.AddMinutes(-75);
            var timeTwo = DateTime.Now.AddMinutes(5);

            var ts = timeTwo.Subtract(timeOne);

            var ts2 = timeOne.Subtract(timeTwo);

            var hz1 = ts.Humanize(2);
            var hz2 = ts.Humanize(2);
            var hz3 = ts.Humanize(2);

            var hz11 = ts2.Humanize(2);
            var hz21 = ts2.Humanize(2);
            var hz31 = ts2.Humanize(2);

            Debug.WriteLine($"Hz1: {hz1}");
            Debug.WriteLine($"Hz2: {hz2}");
            Debug.WriteLine($"Hz3: {hz3}");


            Debug.WriteLine($"Hz11: {hz11}");
            Debug.WriteLine($"Hz21: {hz21}");
            Debug.WriteLine($"Hz31: {hz31}");
        }
    }
}
