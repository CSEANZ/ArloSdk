using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Arlo.SDK.Entities.Base;
using Arlo.SDK.Entities.Product;
using Arlo.SDK.Entities.System;

namespace Arlo.SDK.Entities
{

    [XmlRoot(ElementName = "EventSession")]
    public class ArloSession : ListOfLinks
    {
        [XmlElement(ElementName = "SessionID")]
        public string SessionID { get; set; }
        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "StartDateTime")]
        public string StartDateTime { get; set; }
        [XmlElement(ElementName = "FinishDateTime")]
        public string FinishDateTime { get; set; }
        [XmlElement(ElementName = "SessionType")]
        public string SessionType { get; set; }
        [XmlElement(ElementName = "Description")]
        public string Description { get; set; }
        [XmlElement(ElementName = "Status")]
        public string Status { get; set; }
        [XmlElement(ElementName = "CreatedDateTime")]
        public string CreatedDateTime { get; set; }
        [XmlElement(ElementName = "LastModifiedDateTime")]
        public string LastModifiedDateTime { get; set; }

        public DateTime? DateTime_Start
        {
            get
            {
                DateTime result;

                if (DateTime.TryParse(StartDateTime.Replace("+10:00", ""), out result))
                {
                    return result;
                }
                return DateTime.MaxValue;
            }
        }

        public DateTime? DateTime_End
        {
            get
            {
                DateTime result;

                if (DateTime.TryParse(FinishDateTime.Replace("+10:00", ""), out result))
                {
                    return result;
                }
                return DateTime.MaxValue;
            }
        }

        public string Room => VenueDetails?.VenueRoom?.Name ?? "-";

        public string Code => _getField(Constants.Fields.SessionCode);

        public string Objective => _getField(Constants.Fields.Objective);

        public string Theme => _getField(Constants.Fields.Theme);

        public string Audience => _getField(Constants.Fields.Audience);

        public string Track => _getField(Constants.Fields.Track);

        public List<ArloContact> Presenters { get; set; }
        public ArloCustomFields CustomFields { get; set; }

        public EventSessionVenueDetails VenueDetails { get; set; }

        string _getField(string field)
        {
            var fieldObject = CustomFields?.Field?.FirstOrDefault(_ => _.Name == field);

            if (fieldObject == null)
            {
                return null;
            }

            if (fieldObject.Value.StringArray?.Values?.Count > 0)
            {
                return string.Join(", ", fieldObject.Value.StringArray?.Values);
            }

            return fieldObject.Value?.String;
            
        }
    }

}
