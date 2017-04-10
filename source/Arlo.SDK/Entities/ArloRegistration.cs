using System.Collections.Generic;
using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;
using Arlo.SDK.Entities.Product;

namespace Arlo.SDK.Entities
{
   

    [XmlRoot(ElementName = "Registration")]
    public class ArloRegistration : ListOfLinks
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
        [XmlElement(ElementName = "LastModifiedDateTime")]
        public string LastModifiedDateTime { get; set; }

        public ArloContact Contact { get; set; }
    }

}
