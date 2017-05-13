using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Security;


namespace CloVis
{
    public class FileManagement
    {
        public FileManagement()
        {
            // a faire
        }
        public void Create_File()
        {
            
            //String filename = "fond.xml";
           
            //FileStream myFileStream = new FileStream(Path.GetFileName(filename), FileMode.OpenOrCreate);

            /*XmlWriter xmlWriter = XmlWriter.Create( new StringBuilder(filename));

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("users");

            xmlWriter.WriteStartElement("user");
            xmlWriter.WriteAttributeString("age", "42");
            xmlWriter.WriteString("John Doe");
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("user");
            xmlWriter.WriteAttributeString("age", "39");
            xmlWriter.WriteString("Jane Doe");

            xmlWriter.WriteEndDocument();
            //xmlWriter.Close();
            */
        }
    }
}