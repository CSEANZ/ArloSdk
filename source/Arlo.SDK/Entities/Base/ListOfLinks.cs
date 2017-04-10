using System.Collections.Generic;
using System.Xml.Serialization;

namespace Arlo.SDK.Entities.Base
{
    public class ListOfLinks
    {
        [XmlElement(ElementName = "Link")]
        public List<Link> Link { get; set; }
    }
}
