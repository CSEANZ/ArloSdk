using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Entities.Product;
using Arlo.SDK.Entities.System;
using Arlo.SDK.Repo.Base;
using Xamling.Azure.Portable.Contract;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Serialise;

namespace Arlo.SDK.Repo
{
    public class ArloEventRepo : ArloRepoBase<ArloEvent>, IArloEventRepo
    {
        public ArloEventRepo(IHttpTransferrer downloader, IEntitySerialiser entitySerialiser, ILogService logService) : base(downloader, entitySerialiser, logService, Constants.Endpoints.Events)
        {
        }

        public async Task<ArloEvent> GetById(string eventId)
        {
            return await Get($"{eventId}/");
        }
    }
}
