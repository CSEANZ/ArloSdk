using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Contract;
using Arlo.SDK.Repo.Base;
using Xamling.Azure.Portable.Contract;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Contract.Serialise;

namespace Arlo.SDK.Repo
{
    public class GenericWebRepo<T> : ArloRepoBase<T>, IGenericWebRepo<T>
        where T: class, new()
    {
        public GenericWebRepo(IHttpTransferrer downloader, IEntitySerialiser entitySerialiser, ILogService logService) : base(downloader, entitySerialiser, logService, "")
        {
        }
    }
}
