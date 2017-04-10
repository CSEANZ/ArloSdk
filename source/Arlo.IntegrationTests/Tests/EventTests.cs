using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class EventTests : TestBase
    {
        //[TestMethod]
        //public async Task TestTest()
        //{
        //    var dt = DateTime.Parse("2017-01-24TAF");
        //    Assert.IsTrue(dt != null);
        //}

        [TestMethod]
        public async Task GetEvents()
        {
            var eventService = Resolve<IEventService>();

            var events = await eventService.GetAllEvents();

            Assert.IsNotNull(events);

            Assert.IsTrue(events.Link?.Count > 0);

            foreach (var ev in events.Link)
            {
                Debug.WriteLine(ev.Href);
            }
        }

        [TestMethod]
        public async Task GetById()
        {
            var eventService = Resolve<IEventService>();

            var ev = await eventService.GetEventById("1881", false);
           
            Assert.IsNotNull(ev);
            Assert.IsTrue(ev.EventID == "1881");
            Assert.IsTrue(ev.Link?.Count > 0);
            Debug.WriteLine(ev.Description);
        }

        [TestMethod]
        public async Task GetEventSessions()
        {
            var eventService = Resolve<IEventService>();
            var sessionsService = Resolve<ISessionService>();

            var ev = await eventService.GetEventById("1881", false);

            Assert.IsNotNull(ev);
            Assert.IsTrue(ev.EventID == "1881");
            Assert.IsTrue(ev.Link?.Count > 0);
            var sessions = await sessionsService.GetAllEventSessionLinks(ev, true, false);
            Assert.IsNotNull(sessions);
            Assert.IsTrue(sessions.Link?.Count > 110);
        }


        [TestMethod]
        public async Task GetEventSessionDetail()
        {
            var eventService = Resolve<IEventService>();
            var sessionsService = Resolve<ISessionService>();

            var ev = await eventService.GetEventById("1881", false);

            Assert.IsNotNull(ev);
            Assert.IsTrue(ev.EventID == "1881");
            Assert.IsTrue(ev.Link?.Count > 0);
            var sessions = await sessionsService.GetAllEventSessionLinks(ev, false, false);
            Assert.IsNotNull(sessions);
            Assert.IsTrue(sessions.Link?.Count > 0);

            var eventSession = await sessionsService.GetEventSession(sessions.Link[60].Href, false);

            Assert.IsNotNull(eventSession);
        }


        [TestMethod]
        public async Task GetAllEventSessionDetail()
        {
            var sessionsService = Resolve<ISessionService>();

            var eventSession = await sessionsService.GetEventSessions(true);

            Assert.IsNotNull(eventSession);

            var filtered = await sessionsService.GetEventSessionsFilterField(Constants.Fields.Track, "net");

            Assert.IsNotNull(filtered);

            Assert.IsTrue(filtered.Count > 0);

            var l = new List<string>();

            foreach (var f in filtered)
            {
                foreach (var fieldValu in f.CustomFields.Field)
                {
                    if (!l.Contains(fieldValu.Name))
                    {
                        l.Add(fieldValu.Name);
                    }
                }

                var firstOrDefault = f.CustomFields.Field.FirstOrDefault(_ => _.Name == Constants.Fields.Track);
                if (firstOrDefault != null)
                {
                    Debug.WriteLine($"{f.Name} ({firstOrDefault.Value.String})");
                }
                else
                {
                    Debug.WriteLine("No match");
                }
                    
            }

            foreach (var listItem in l)
            {
                Debug.WriteLine(listItem);
            }
        }


        [TestMethod]
        public async Task GetAllTopics()
        {
            var eventService = Resolve<IEventService>();
            var sessionsService = Resolve<ISessionService>();

            var ev = await eventService.GetEventById("1881", false);

            Assert.IsNotNull(ev);
            Assert.IsTrue(ev.EventID == "1881");
            Assert.IsTrue(ev.Link?.Count > 0);

            var result = await sessionsService.GetAllTopics(ev);

            Assert.IsNotNull(result);

            Assert.IsFalse(result.Count == 0);


            foreach (var i in result)
            {
                Debug.WriteLine(i);
            }
        }


        [TestMethod]
        public async Task GetTopicsByDateTime()
        {
            var eventService = Resolve<IEventService>();
            var sessionsService = Resolve<ISessionService>();

            var ev = await eventService.GetEventById("1881", false);

            Assert.IsNotNull(ev);
            Assert.IsTrue(ev.EventID == "1881");
            Assert.IsTrue(ev.Link?.Count > 0);

            // There is only one topic at this time, "Keynote"
            var result = await sessionsService.GetTopicsByDateTime(ev, new DateTime(2017,2,14), "morning");

            Assert.IsNotNull(result);

            Assert.IsTrue(result.Count == 1);


            foreach (var i in result)
            {
                Debug.WriteLine(i);
            }
        }

        [TestMethod]
        public async Task GetAllPresenters()
        {
            var eventService = Resolve<IEventService>();
            var sessionsService = Resolve<ISessionService>();

            var ev = await eventService.GetEventById("1881", false);

            Assert.IsNotNull(ev);
            Assert.IsTrue(ev.EventID == "1881");
            Assert.IsTrue(ev.Link?.Count > 0);

            var presenters = await sessionsService.GetAllPresenters(ev);

            Assert.IsNotNull(presenters);

            Assert.IsFalse(presenters.Count == 0);

            Debug.WriteLine($"Found {presenters.Count} presenters\n");

            var guFound = false;

            foreach (var presenter in presenters)
            {
                Debug.WriteLine(presenter);
                if (presenter.ToLowerInvariant().Equals("jordan knight"))
                {
                    guFound = true;
                    break;
                }
            }

            Assert.IsTrue(guFound);
        }
    }
}