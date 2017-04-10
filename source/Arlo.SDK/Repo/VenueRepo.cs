using Arlo.SDK.Contract;
using Arlo.SDK.Entities.Product;
using Arlo.SDK.Repo.Base;
using Xamling.Azure.Portable.Contract;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Serialise;

namespace Arlo.SDK.Repo
{
    public class VenueRepo : ArloRepoBase<EventSessionVenueDetails>, IVenueRepo
    {
        public VenueRepo(IHttpTransferrer downloader, IEntitySerialiser entitySerialiser, ILogService logService) : base(downloader, entitySerialiser, logService, "")
        {
        }
    }
}
