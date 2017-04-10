using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.IntegrationTests.Glue;
using Autofac;

namespace Arlo.IntegrationTests
{
    public class TestBase
    {
        protected IContainer Container;

        public TestBase()
        {
            var g = new ProjectGlue();
            Container = g.Init();
        }

        public T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
