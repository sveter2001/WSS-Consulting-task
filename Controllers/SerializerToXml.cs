using HierarchicalCatalog.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace HierarchicalCatalog.Controllers
{
    public class SerializerToXml
    {
        private XmlSerializer _xmlSerializer;

        public SerializerToXml()
        {
            _xmlSerializer = new XmlSerializer(typeof(List<MenuItem>));
        }

        public void SerializeToXml(List<MenuItem> list, MemoryStream memoryStream)
        {
            try
            {
                using (var xmlWriter = XmlWriter.Create(memoryStream, new XmlWriterSettings { Indent = true }))
                {
                    _xmlSerializer.Serialize(xmlWriter, list);
                }
                Console.WriteLine("Object has been Serialized");
            }
            catch
            {
                Console.WriteLine("Someting went wrong while Serializing");
            }
        }

        public List<MenuItem> DeserializeFromXml(MemoryStream ms)
        {
            List<MenuItem> list = new List<MenuItem>();
            try
            {
                using (var xmlReader = XmlReader.Create(ms, new XmlReaderSettings()))
                {
                    list = _xmlSerializer.Deserialize(xmlReader) as List<MenuItem>;
                }
                Console.WriteLine("Object has been deserialized");
            }
            catch
            {
                Console.WriteLine("Someting went wrong while deserializing");
            }
            return list;
        }
    }
}