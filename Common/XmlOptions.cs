using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Comon.XmlUtilities
{
    public class XmlOptions
    {
        public static StringWriter ObjectSerialize<T>(T pObjetoConvertir)
        {
            XmlSerializer serializadorEntrada = new XmlSerializer(pObjetoConvertir.GetType());
            StringWriter writerEntrada = new StringWriter();
            serializadorEntrada.Serialize(writerEntrada, pObjetoConvertir);

            return writerEntrada; 
        }

        public static T ObjectUnSerialize<T>(string pXmlSerializado)
        {
            T t = default(T);
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(pXmlSerializado);
            // Deserializa el modelo a partir de un lector de XML
            XmlNodeReader reader = new XmlNodeReader(xmlDocument);
            XmlSerializer ser = new XmlSerializer(typeof(T));
            t = (T)ser.Deserialize(reader);
            reader.Close();
            return t;
        }
    }
}