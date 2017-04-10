using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arlo.SDK.Contract
{
    public interface IGenericWebRepo<T> : IArloRepoBase<T>
        where T:class, new()
    {
    }
}
