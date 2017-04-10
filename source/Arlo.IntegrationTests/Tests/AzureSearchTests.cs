using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using EventBot.SupportLibrary.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SearchIndexer.SupportLibrary;
using XamlingCore.Portable.Data.Glue;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class AzureSearchTests : TestBase
    {
        [TestMethod]
        public async Task TestIndxerUpdate()
        {
            var searchService = Resolve<IArloSdkSearchIndexService>();

            await searchService.InitAsync();
            var result = await searchService.IndexAsync();

            Assert.IsTrue(result);
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        [TestMethod]
        public async Task TestQueryIndex()
        {
            var azureSearch = ContainerHost.Container.Resolve<IAzureSearchHelper>();

            var query = "Jordan Knight";
            var result = await azureSearch.Search(query);

            Assert.IsNotNull(result);
            Debug.WriteLine($"{query}: {result.Count()}");
            Assert.IsTrue(result.Any() && result.Count() < 10);
        }

        [TestMethod]
        public async Task TestResultOrder()
        {
            var azureSearch = ContainerHost.Container.Resolve<IAzureSearchHelper>();

            var query = ".net";
            var result = await azureSearch.Search(query);

            Assert.IsNotNull(result);
            var lastStartDateTime = DateTimeOffset.MinValue;
            foreach (var r in result)
            {
                Debug.WriteLine($"{r.name}: {r.startDateTime}");
                if (r.startDateTime < lastStartDateTime)
                {
                    throw new AssertFailedException("results are not in ascending chronological order");
                }

                lastStartDateTime = r.startDateTime;
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        [TestMethod]
        public async Task TestSpeakerSearch()
        {
            var azureSearch = ContainerHost.Container.Resolve<IAzureSearchHelper>();

            var query = "asdfasdf";
            var result = await azureSearch.SpeakerSearch(query);
            Assert.IsNotNull(result);
            Debug.WriteLine($"{query}: {result.Count()}");
            Assert.IsFalse(result.Any());

            query = "gorden night";
            result = await azureSearch.SpeakerSearch(query);
            Assert.IsNotNull(result);
            Debug.WriteLine($"{query}: {result.Count()}");
            Assert.IsTrue(result.Any() && result.Count() < 10);

            query = "jordan knight";
            result = await azureSearch.SpeakerSearch(query);
            Assert.IsNotNull(result);
            Debug.WriteLine($"{query}: {result.Count()}");
            Assert.IsTrue(result.Any() && result.Count() < 10);

            query = "knight";
            result = await azureSearch.SpeakerSearch(query);
            Assert.IsNotNull(result);
            Debug.WriteLine($"{query}: {result.Count()}");
            Assert.IsTrue(result.Any() && result.Count() < 10);

        }


        [TestMethod]
        public async Task TestSpeakerResultOrder()
        {
            var azureSearch = ContainerHost.Container.Resolve<IAzureSearchHelper>();

            var query = "Jordan Knight";
            var result = await azureSearch.SpeakerSearch(query);

            Assert.IsNotNull(result);
            var lastStartDateTime = DateTimeOffset.MinValue;
            foreach (var r in result)
            {
                Debug.WriteLine($"{r.name}: {r.startDateTime}");
                if (r.startDateTime < lastStartDateTime)
                {
                    throw new AssertFailedException("results are not in ascending chronological order");
                }

                lastStartDateTime = r.startDateTime;
            }
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        [TestMethod]
        public async Task TestTrackSearch()
        {
            var azureSearch = ContainerHost.Container.Resolve<IAzureSearchHelper>();

            var query = "kick111";
            var result1 = await azureSearch.CodeSearch(query);
            Assert.IsNotNull(result1);
            Debug.WriteLine($"{query}: {result1.Count()}");
            Assert.IsTrue(result1.Any());


            query = "kick111b";
            var result2 = await azureSearch.CodeSearch(query);
            Assert.IsNotNull(result2);
            Debug.WriteLine($"{query}: {result2.Count()}");
            Assert.IsTrue(result2.Any());

            Assert.IsTrue(result1.Count() >= result2.Count());
        }
    }
}
