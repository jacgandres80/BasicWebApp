using System;
using System.Collections.Generic;
using Server.Models;

namespace Common
{
    public class XmlFileOperation
    {
        public static string ReadXmlContent(string path)
        {
            try
            { 
                string xml = System.IO.File.ReadAllText(path);
                return xml;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void WriteXmlContent(string xml, string path)
        {
            System.IO.StreamWriter stringWriter = new System.IO.StreamWriter(path);
            stringWriter.Write(xml);
            stringWriter.Close();
        }
    }
}
