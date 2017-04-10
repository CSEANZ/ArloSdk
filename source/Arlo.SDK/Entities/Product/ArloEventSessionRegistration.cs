/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

using System.Collections.Generic;
using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;

namespace Arlo.SDK.Entities.Product
{
    

    [XmlRoot(ElementName = "EventSessionRegistration")]
    public class ArloEventSessionRegistration : ListOfLinks
    {
        [XmlElement(ElementName = "RegistrationID")]
        public string RegistrationID { get; set; }
        [XmlElement(ElementName = "UniqueIdentifier")]
        public string UniqueIdentifier { get; set; }
        [XmlElement(ElementName = "Attendance")]
        public string Attendance { get; set; }
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "CreatedDateTime")]
        public string CreatedDateTime { get; set; }

        public ArloSession Session { get; set; }
    }

}
