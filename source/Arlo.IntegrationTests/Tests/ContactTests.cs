using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Arlo.IntegrationTests.Tests
{
    [TestClass]
    public class ContactTests : TestBase
    {
        [TestMethod]
        public async Task GetAnEventRegistrationFromContactId()
        {
            var eventService = Resolve<IEventService>();
            var contactService = Resolve<IContactService>();

            var ev = await eventService.GetEventById("1881", false);

            var rego = await contactService.MapContactIdToRegistration(ev, "3813");

            Assert.IsNotNull(rego);

            var regoCheck = rego.Contact.FirstName == "Jordan";

            Assert.IsTrue(regoCheck);
        }

        [TestMethod]
        public async Task GetAllEventRegistrations()
        {
            var eventService = Resolve<IEventService>();
            var contactService = Resolve<IContactService>();

            var ev = await eventService.GetEventById("1881", false);

            var rego = await contactService.GetAllEventRegistrations(ev, true);

            Assert.IsNotNull(rego);


            var regoCheck = rego.First(_ => _.Contact.ContactID == "7348");

            Debug.WriteLine(regoCheck.RegistrationID);

            Assert.IsTrue(rego.Count > 100);
            
        }


        [TestMethod]
        public async Task GetAllEventContactsDetail()
        {
            var eventService = Resolve<IEventService>();
            var contactService = Resolve<IContactService>();

            var ev = await eventService.GetEventById("1881", false);

            var contacts = await contactService.GetAllEventRegistrationContacts(ev, true); //don't cache this bit. 

            Assert.IsNotNull(contacts);

            Assert.IsTrue(contacts.Count > 100);


             contacts = await contactService.GetAllEventRegistrationContacts(ev, false);

            Assert.IsNotNull(contacts);

            Assert.IsTrue(contacts.Count > 100);
        }
    }
}
