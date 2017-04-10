using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Entities;
using Arlo.SDK.Repo;

namespace Arlo.SDK.Contract
{
    public interface IEventSessionRepo : IArloRepoBase<ArloSession>
    {
    }
}
