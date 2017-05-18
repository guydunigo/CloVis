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
            
            try
            {
                folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("CVs_CloVis");
            }
            catch (FileNotFoundException)
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("CVs_CloVis");
            }
           

            using (var stream = await folder.OpenStreamForWriteAsync(resumetosave.Name + ".xml", CreationCollisionOption.OpenIfExists))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.Async = true;
            
                XmlWriter writer = XmlWriter.Create(stream, settings);
                await writer.WriteStartDocumentAsync();
                await writer.FlushAsync(); 
            }
        }

        public async void Save_File(Resume resumetosave)
        {
            // ouverture / recherche du dossier
            StorageFolder folder = null;

            try
            {
                folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("CVs_CloVis");
            }
            catch (FileNotFoundException)
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("CVs_CloVis");
            }

            // creer le flux , le writer et écriture des donnees
            using (var stream = await folder.OpenStreamForWriteAsync(resumetosave.Name + ".xml", CreationCollisionOption.OpenIfExists))
            {
                //Settinges
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Async = true;
                settings.Indent = true;
                
                settings.NewLineOnAttributes = true;
                settings.OmitXmlDeclaration = true;
                XmlWriter writer = XmlWriter.Create(stream, settings);


                // écriture des donnees

                //resume (name)
                string name = resumetosave.Name.Replace(" ", "");
                await writer.WriteStartElementAsync("", name, "Resume");

                //Layout
                await writer.WriteStartElementAsync("", "Layout", "Resume");

                //BackBoxes
                int num = 0;
                foreach (var i in resumetosave.Layout.BackBoxes) {

                    await writer.WriteStartElementAsync("", "BackBox" + num, "Resume");
                    await writer.WriteElementStringAsync("", "x", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].X));
                    await writer.WriteElementStringAsync("", "y", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Y));
                    await writer.WriteElementStringAsync("", "z", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Z));

                    await writer.WriteElementStringAsync("", "SizeX", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].SizeX));
                    await writer.WriteElementStringAsync("", "SizeY", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].SizeY));

                    await writer.WriteElementStringAsync("", "img", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Image));

                    await writer.WriteElementStringAsync("", "angle", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Angle));

                    //couleurs
                    await writer.WriteElementStringAsync("", "Color", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Color));
                    await writer.WriteElementStringAsync("", "Color_Border", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderColor));

                    await writer.WriteElementStringAsync("", "B.radius", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderRadius));

                    await writer.WriteEndElementAsync();
                    num += 1;
                }

                //TextesBoxes
                int numT = 0;
                foreach (var i in resumetosave.Layout.TextBoxes)
                {

                    await writer.WriteStartElementAsync("", "TextBox" + numT, "Resume");
                    await writer.WriteElementStringAsync("", "x", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].X));
                    await writer.WriteElementStringAsync("", "y", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Y));
                    await writer.WriteElementStringAsync("", "z", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Z));

                    await writer.WriteElementStringAsync("", "SizeX", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].SizeX));
                    await writer.WriteElementStringAsync("", "SizeY", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].SizeY));

                    await writer.WriteElementStringAsync("", "angle", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Angle));

                    
                    await writer.WriteElementStringAsync("", "Default_Element", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].DefaultElement));

                    //Element
                    await writer.WriteElementStringAsync("", "Element_Name", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.Name));
                    await writer.WriteElementStringAsync("", "Element_Default", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.IsDefault));

                    await writer.WriteEndElementAsync();
                    numT += 1;
                }
                //Fin Layout
                await writer.WriteEndElementAsync();

                //Fonts


                //Fin resume
                await writer.WriteEndElementAsync();

                //vider le writer, fin de la sauvegarde
                await writer.WriteEndDocumentAsync();
                await writer.FlushAsync();
            }
        }
    }
}

