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
    public class SessionRegistrationTests : TestBase
    {
        [TestMethod]
        public async Task GetSessionRegistrations()
        {
            var eventService = Resolve<IEventService>();
            var contactService = Resolve<IContactService>();
            var registrationService = Resolve<IArloSessionRegistrationService>();

            var ev = await eventService.GetEventById("1881", false);

            var rego = await contactService.MapContactIdToRegistration(ev, "3813");

            Assert.IsNotNull(rego);

            var regoCheck = rego.Contact.FirstName == "Jordan";

            Assert.IsTrue(regoCheck);

            var registrations = await registrationService.GetSessionRegistrations(rego, false);

            Assert.IsNotNull(registrations);

            Assert.IsTrue(registrations.Count > 0);
        }

        [TestMethod]
        public async Task GetRegoById()
        {

            //Get rego
            var contactService = Resolve<IContactService>();
            var registrationService = Resolve<IArloSessionRegistrationService>();
            

            var rego = await contactService.GetRegistration("83675");

            Assert.IsNotNull(rego);

            Assert.IsNotNull(rego.Contact);

            var regoCheck = rego.Contact.FirstName == "Jordan";

            Assert.IsTrue(regoCheck);

            var registrations = await registrationService.GetSessionRegistrations(rego, false);

            Assert.IsNotNull(registrations);

            Assert.IsTrue(registrations.Count > 0);
        }

        [TestMethod]
        public async Task RegisterForASession()
        {

            var eventService = Resolve<IEventService>();
            var sessionsService = Resolve<ISessionService>();

            //Get rego
            var contactService = Resolve<IContactService>();
            var registrationService = Resolve<IArloSessionRegistrationService>();
            

            var ev = await eventService.GetEventById("1881", false);

            var rego = await contactService.MapContactIdToRegistration(ev, "3813");

            Assert.IsNotNull(rego);

            var regoCheck = rego.Contact.FirstName == "Jordan";

            Assert.IsTrue(regoCheck);

            var registrations = await registrationService.GetSessionRegistrations(rego, true);

            Assert.IsNotNull(registrations);

            Assert.IsTrue(registrations.Count > 0);

            //Get sessions and things

            //Assert.IsNotNull(ev);
            //Assert.IsTrue(ev.EventID == "1881");
            //Assert.IsTrue(ev.Link?.Count > 0);
            //var sessions = await sessionsService.GetAllEventSessionLinks(ev, false, false);
            //Assert.IsNotNull(sessions);
            //Assert.IsTrue(sessions.Link?.Count > 0);

            //var eventSession = await sessionsService.GetEventSession(sessions.Link[60].Href);

            var regoDelete = registrations.LastOrDefault();
            var eventSession = regoDelete?.Session;

            var isRegistered1 = await registrationService.IsRegistered(rego, regoDelete.Session, false);
            Assert.IsTrue(isRegistered1 != null);
            var isRegistered2 = await registrationService.IsRegistered(rego, regoDelete.Session.Code, false);
            Assert.IsTrue(isRegistered2 != null);

            var result = await registrationService.UnregisterFromSession(rego, regoDelete);
            Assert.IsTrue(result);

            var isRegistered3 = await registrationService.IsRegistered(rego, regoDelete.Session, false);
            Assert.IsFalse(isRegistered3 != null);
            var isRegistered4 = await registrationService.IsRegistered(rego, regoDelete.Session.Code, false);
            Assert.IsFalse(isRegistered4 != null);
            

            var registrations2 = await registrationService.GetSessionRegistrations(rego, false);

            Assert.IsNotNull(registrations);

            Assert.IsTrue(registrations.Count != registrations2.Count);

            Assert.IsNotNull(eventSession);

            var sessionRegisterResult = await registrationService.RegisterForSession(rego, eventSession);

            Assert.IsTrue(sessionRegisterResult);

            var isRegistered5 = await registrationService.IsRegistered(rego, eventSession, false);
            Assert.IsTrue(isRegistered5 != null);
            var isRegistered6 = await registrationService.IsRegistered(rego, eventSession.Code, false);
            Assert.IsTrue(isRegistered6 != null);


            var registrations3 = await registrationService.GetSessionRegistrations(rego, false);

            Assert.IsTrue(registrations3.Count == registrations.Count);
        }
    }
}
