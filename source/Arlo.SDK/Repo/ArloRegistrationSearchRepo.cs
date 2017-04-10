using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities;
using Arlo.SDK.Entities.Product;
using Arlo.SDK.Entities.System;
using Arlo.SDK.Repo.Base;
using Arlo.SDK.Util;
using Xamling.Azure.Portable.Contract;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Serialise;

namespace Arlo.SDK.Repo
{
    public class ArloRegistrationSearchRepo : ArloRepoBase<EventRegistrationList>, IArloRegistrationSearchRepo
    {
        public ArloRegistrationSearchRepo(IHttpTransferrer downloader, IEntitySerialiser entitySerialiser, ILogService logService) : base(downloader, entitySerialiser, logService, "")
        {
        }

        public async Task<EventRegistrationList> Search(ArloEvent ev, string contactId)
        {
            var baseUrl = new Uri(ev.FindLink(Constants.Rel.Registrations));

            var actualUrl = new Uri(baseUrl, $"?filter=Contact/ContactId eq {contactId}");

            return await Get(actualUrl.ToString());
        }
    }
}
