using System.Xml.Serialization;

namespace Arlo.SDK.Entities.Base
{
    [XmlRoot(ElementName = "Link")]
    public class Link
    {
       
       
            [XmlAttribute(AttributeName = "rel")]
            public string Rel { get; set; }
            [XmlAttribute(AttributeName = "type")]
            public string Type { get; set; }
            [XmlAttribute(AttributeName = "title")]
            public string Title { get; set; }
            [XmlAttribute(AttributeName = "href")]
            public string Href { get; set; }
        
    }
}
