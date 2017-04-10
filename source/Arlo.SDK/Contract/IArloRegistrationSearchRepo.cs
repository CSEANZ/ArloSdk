using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arlo.SDK.Entities.Product;

namespace Arlo.SDK.Contract
{
    public interface IArloRegistrationSearchRepo : IArloRepoBase<EventRegistrationList>
    {
        Task<EventRegistrationList> Search(ArloEvent ev, string contactId);
    }
}
