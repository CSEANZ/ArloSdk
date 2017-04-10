/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

using System.Collections.Generic;
using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;

namespace Arlo.SDK.Entities.Product
{
    

    [XmlRoot(ElementName = "Contact")]
    public class ArloContact : ListOfLinks
    {
        [XmlElement(ElementName = "ContactID")]
        public string ContactID { get; set; }
        [XmlElement(ElementName = "UniqueIdentifier")]
        public string UniqueIdentifier { get; set; }
        [XmlElement(ElementName = "FirstName")]
        public string FirstName { get; set; }
        [XmlElement(ElementName = "LastName")]
        public string LastName { get; set; }
        [XmlElement(ElementName = "Email")]
        public string Email { get; set; }
        [XmlElement(ElementName = "PhoneMobile")]
        public string PhoneMobile { get; set; }
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "CreatedDateTime")]
        public string CreatedDateTime { get; set; }
        [XmlElement(ElementName = "LastModifiedDateTime")]
        public string LastModifiedDateTime { get; set; }

        public string FullName => $"{FirstName} {LastName}";
    }

}
