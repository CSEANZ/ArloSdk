using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.Contract.Repos.Base;

namespace Arlo.SDK.Contract
{
    public interface IArloRepoBase<T> :IWebRepo<T>
        where T : class, new()
    {
    }
}
