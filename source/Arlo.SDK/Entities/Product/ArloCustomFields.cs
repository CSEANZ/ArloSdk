/* 
 Licensed under the Apache License, Version 2.0

 http://www.apache.org/licenses/LICENSE-2.0
 */

using System.Collections.Generic;
using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;

namespace Arlo.SDK.Entities.Product
{
    [XmlRoot(ElementName = "Value")]
    public class Value
    {
        [XmlElement(ElementName = "String")]
        public string String { get; set; }
        [XmlElement(ElementName = "StringArray")]
        public StringArray StringArray { get; set; }
    }

    [XmlRoot(ElementName = "Field")]
    public class Field
    {
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "Value")]
        public Value Value { get; set; }
        [XmlElement(ElementName = "Link")]
        public Link Link { get; set; }
    }

    [XmlRoot(ElementName = "StringArray")]
    public class StringArray
    {
        [XmlElement(ElementName = "String")]
        public List<string> Values { get; set; }
    }

    [XmlRoot(ElementName = "CustomFields")]
    public class ArloCustomFields
    {
        [XmlElement(ElementName = "Field")]
        public List<Field> Field { get; set; }
        [XmlElement(ElementName = "Link")]
        public Link Link { get; set; }
    }

}
