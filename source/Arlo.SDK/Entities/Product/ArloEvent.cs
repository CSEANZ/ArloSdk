
using System.Collections.Generic;
using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;

namespace Arlo.SDK.Entities.Product
{
    
    [XmlRoot(ElementName = "Event")]
    public class ArloEvent : ListOfLinks
    {
        [XmlElement(ElementName = "EventID")]
        public string EventID { get; set; }
        [XmlElement(ElementName = "UniqueIdentifier")]
        public string UniqueIdentifier { get; set; }
        [XmlElement(ElementName = "Code")]
        public string Code { get; set; }
        [XmlElement(ElementName = "StartDateTime")]
        public string StartDateTime { get; set; }
        [XmlElement(ElementName = "FinishDateTime")]
        public string FinishDateTime { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "LocationName")]
        public string LocationName { get; set; }
        [XmlElement(ElementName = "IsPrivate")]
        public string IsPrivate { get; set; }
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "CreatedDateTime")]
        public string CreatedDateTime { get; set; }
        [XmlElement(ElementName = "LastModifiedDateTime")]
        public string LastModifiedDateTime { get; set; }
    }
}
