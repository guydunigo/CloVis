using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Security;
using Windows.Storage;
using System.Xml.Serialization;

namespace Resume
{
   
    public class FileManagement
    {
        public FileManagement()
        {
            // a faire
        }
        public async void Create_File(Resume resumetosave)
        {

            StorageFolder folder = null;
            StorageFile file = null;
            try
            {
                folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("CVs_CloVis");
            }
            catch (FileNotFoundException)
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("CVs_CloVis");
            }
           
            
            // Writes simply in the file :
            file = await folder.CreateFileAsync(resumetosave.Name + ".xml", CreationCollisionOption.OpenIfExists);

            // With streams (manage where to write, ...) :
            using (var stream = await folder.OpenStreamForWriteAsync(resumetosave.Name + ".xml", CreationCollisionOption.OpenIfExists))
            {

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Async = true;

                XmlWriter writer = XmlWriter.Create(stream, settings);
                
                await writer.FlushAsync(); // vide tout
                
            }
        }
    }
}

/*  Sérialisation d'une collection ! 
        private void SerializeCollection(string filename){
        Employees Emps = new Employees();
        // Note that only the collection is serialized -- not the 
        // CollectionName or any other public property of the class.
        Emps.CollectionName = "Employees";
        Employee John100 = new Employee("John", "100xxx");
        Emps.Add(John100);
        XmlSerializer x = new XmlSerializer(typeof(Employees));
        TextWriter writer = new StreamWriter(filename);
        x.Serialize(writer, Emps);
    }
    */

//await writer.WriteStartElementAsync("pf", "root", "http://ns");



//await writer.WriteStartElementAsync(null, "sub", null);
/* await writer.WriteAttributeStringAsync(null, "att", null, "val"); await writer.WriteStringAsync("text");
 await writer.WriteEndElementAsync(); */

//await writer.WriteProcessingInstructionAsync("pName", "pValue"); // instruction simple

// await writer.WriteCommentAsync("cValue"); // commentaire

// await writer.WriteCDataAsync("cdata value");  Quelle utilité ?
// await writer.WriteEndElementAsync();  fin de la "permiere " balise