using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;

namespace Arlo.SDK.Entities.Product
{
    [XmlRoot(ElementName = "Contacts")]
    public class ContactList : ListOfLinks
    {
    }
}
