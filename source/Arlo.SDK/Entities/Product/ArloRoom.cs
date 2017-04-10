/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

using System.Collections.Generic;
using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;

namespace Arlo.SDK.Entities.Product
{
    public class EventSessionVenueDetails : ListOfLinks
    {
        public VenueRoom VenueRoom { get; set; }
    }

    [XmlRoot(ElementName = "VenueRoom")]
    public class VenueRoom : ListOfLinks
    {
        [XmlElement(ElementName = "VenueRoomID")]
        public string VenueRoomID { get; set; }

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Capacity")]
        public string Capacity { get; set; }

        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
    }
}
