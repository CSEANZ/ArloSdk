using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using XamlingCore.Portable.Contract.Serialise;

namespace Arlo.SDK.Impl
{
    public class XmlEntitySerialiser : IEntitySerialiser
    {
        public T Deserialise<T>(string entity) where T : class
        {
            var document = XDocument.Parse(entity);
            var serializer = new XmlSerializer(typeof(T));
            var result = (T)serializer.Deserialize(document.CreateReader());
            return result;
        }

        public string Serialise<T>(T entity)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var textWriter = new StringWriter())
            {
                serializer.Serialize(textWriter, entity);
                var t = textWriter.ToString();
                t = t.Replace("encoding=\"utf-16\"", "");

                return t;
            }
        }

        public T BinaryDeserialise<T>(byte[] entity) where T : class
        {
            throw new NotImplementedException();
        }

        public byte[] BinarySerialise<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
