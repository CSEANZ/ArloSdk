using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;

namespace Arlo.SDK.Entities.Product
{
    [XmlRoot(ElementName = "Events")]
    public class EventList : ListOfLinks
    {
       
    }
}
