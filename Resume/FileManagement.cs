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
using ResumeElements;

namespace Resume
{
   
    public static class FileManagement
    {
        public static async void Create_File(Resume resumetosave)
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
           

            using (var stream = await folder.OpenStreamForWriteAsync(resumetosave.Name + ".cv", CreationCollisionOption.OpenIfExists))
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


        public static async void Save_File(Resume resumetosave)
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
            using (var stream = await folder.OpenStreamForWriteAsync(resumetosave.Name + ".cv", CreationCollisionOption.OpenIfExists))
            {
                //Settinges
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Async = true;
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.OmitXmlDeclaration = true;
                XmlWriter writer = XmlWriter.Create(stream, settings);


                XmlReaderSettings read_settings = new XmlReaderSettings();
                read_settings.Async = true;
                read_settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(stream, read_settings);

                // écriture des donnees

                //resume (name)
                string name = resumetosave.Name.Replace(" ", "");
                if (reader.LocalName != name)await writer.WriteStartElementAsync("", name, "Resume");

                //Layout
                await writer.WriteStartElementAsync("", "Layout", "Resume");

                //BackBoxes
                int num = 0;
                foreach (var i in resumetosave.Layout.BackBoxes) {

                    await writer.WriteStartElementAsync("", "BackBox_" + num, "Resume");

                    await writer.WriteElementStringAsync("", "x", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].X));
                    await writer.WriteElementStringAsync("", "y", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Y));
                    await writer.WriteElementStringAsync("", "z", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Z));
                    await writer.WriteElementStringAsync("", "SizeX", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].SizeX));
                    await writer.WriteElementStringAsync("", "SizeY", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].SizeY));
                    await writer.WriteElementStringAsync("", "img", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Image));
                    await writer.WriteElementStringAsync("", "angle", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Angle));
                    await writer.WriteElementStringAsync("", "Color", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Color));
                    await writer.WriteElementStringAsync("", "Border_Color", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderColor));
                    await writer.WriteElementStringAsync("", "Border_radius", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderRadius));

                    await writer.WriteEndElementAsync();
                    num += 1;
                }

                //TextesBoxes
                int numT = 0;
                foreach (var i in resumetosave.Layout.TextBoxes)
                {
                    await writer.WriteStartElementAsync("", "TextBox_" + numT, "Resume");

                    await writer.WriteElementStringAsync("", "x", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].X));
                    await writer.WriteElementStringAsync("", "y", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Y));
                    await writer.WriteElementStringAsync("", "z", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Z));
                    await writer.WriteElementStringAsync("", "SizeX", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].SizeX));
                    await writer.WriteElementStringAsync("", "SizeY", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].SizeY));
                    await writer.WriteElementStringAsync("", "angle", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Angle));
                    // await writer.WriteElementStringAsync("", "Default_Element", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].DefaultElement));

                    //element
                    Save_Element(resumetosave, writer, numT);

                    //elementList
                    Save_ElementList(resumetosave, writer, numT);


                    await writer.WriteEndElementAsync();
                   // await writer.WriteEndElementAsync();
                    numT += 1;
                }
                //Fin Layout
                await writer.WriteEndElementAsync();

                //Fonts

                await writer.WriteStartElementAsync("",resumetosave.Fonts.Name,"Resume");
                int numF = 0;
                foreach (var k in resumetosave.Fonts.List )
                {
                    await writer.WriteStartElementAsync("","Font_"+numF,"Resume");

                    await writer.WriteElementStringAsync("","Font_Name","Resume",Convert.ToString(k.Name));
                    await writer.WriteElementStringAsync("","Font_font","Resume",Convert.ToString(k.Font));
                    await writer.WriteElementStringAsync("","Font_fontSize","Resume",Convert.ToString(k.FontSize));
                    await writer.WriteElementStringAsync("","Font_Color","Resume",Convert.ToString(k.Color));
                    await writer.WriteElementStringAsync("","Font_Italic","Resume",Convert.ToString(k.Italic));
                    await writer.WriteElementStringAsync("","Font_Bold","Resume",Convert.ToString(k.Bold));
                    await writer.WriteElementStringAsync("","Font_Underlined","Resume",Convert.ToString(k.Underlined));
                    await writer.WriteElementStringAsync("","Font_UpperCase","Resume",Convert.ToString(k.UpperCase));
                    await writer.WriteEndElementAsync();
                    numF+=1;
                }

                await writer.WriteEndElementAsync();
                //Fin resume
                await writer.WriteEndElementAsync();

                //vider le writer, fin de la sauvegarde
                await writer.WriteEndDocumentAsync();
                await writer.FlushAsync();
            }
        }

        public static async void Save_Element(Resume resumetosave, XmlWriter writer, int numT)
        {
                //Simple Element
               
                //Data
                if (resumetosave.Layout.TextBoxes[numT].Element is Data data && !(resumetosave.Layout.TextBoxes[numT].Element is Data<string>))
                {
                    await writer.WriteStartElementAsync("", "Data_Element", "Resume");

                    await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.Name));
                    await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(data.Level));
                    await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(data.Name));
                    await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(data.Description));
                    await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(data.IsIndependant));
                    await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(data.IsDefault));
                    //liste des catégories associé a l'élément
                    await writer.WriteStartElementAsync("", "D_catergories", "Resume");
                    int numcat = 0;
                    foreach (var cats in data.Categories)
                    {
                        await writer.WriteElementStringAsync("", "catergorie" + numcat, "Resume", cats.Name);
                        numcat += 1;
                    }
                    await writer.WriteEndElementAsync();


                    await writer.WriteEndElementAsync();
                }

                //Data<string>
                if (resumetosave.Layout.TextBoxes[numT].Element is Data<string> datestr)
                {
                    var box = resumetosave.Layout.TextBoxes[numT];

                    await writer.WriteStartElementAsync("", "DataString_Element", "Resume");
                    await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(box.Element.Name));

                    await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(datestr.Level));
                    await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(datestr.Name));
                    await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(datestr.Description));
                    await writer.WriteElementStringAsync("", "D_value", "Resume", datestr.Value);
                    await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(datestr.IsIndependant));
                    await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(datestr.IsDefault));
                    //liste des catégories associé a l'élément
                    await writer.WriteStartElementAsync("", "D_catergories", "Resume");
                    int numcat = 0;
                    foreach (var cats in datestr.Categories)
                    {
                        await writer.WriteElementStringAsync("", "catergorie" + numcat, "Resume", cats.Name);
                        numcat += 1;
                    }
                    await writer.WriteEndElementAsync();

                    await writer.WriteEndElementAsync();
                }

                //DataDated<string>
                if (resumetosave.Layout.TextBoxes[numT].Element is DataDated<string> dateD)
                {

                    await writer.WriteStartElementAsync("", "DataDated_Element", "Resume");

                    await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.Name));
                    await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(dateD.Level));
                    await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(dateD.Name));
                    await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(dateD.Description));
                    await writer.WriteElementStringAsync("", "D_value", "Resume", dateD.Value);
                    await writer.WriteElementStringAsync("", "D_start", "Resume", Convert.ToString(dateD.StartTime));
                    await writer.WriteElementStringAsync("", "D_end", "Resume", Convert.ToString(dateD.EndTime));
                    await writer.WriteElementStringAsync("", "D_Format", "Resume", Convert.ToString(dateD.DisplayFormat));
                    await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(dateD.IsIndependant));
                    await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(dateD.IsDefault));
                    //liste des catégories associé a l'élément
                    await writer.WriteStartElementAsync("", "D_catergories", "Resume");
                    int numcat = 0;
                    foreach (var cats in dateD.Categories)
                    {
                        await writer.WriteElementStringAsync("", "catergorie" + numcat, "Resume", cats.Name);
                        numcat += 1;
                    }
                    await writer.WriteEndElementAsync();


                    await writer.WriteEndElementAsync();
                }
                //  await writer.WriteElementStringAsync("", "Element_Default", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.IsDefault));
            
        }

        public static async void Save_ElementList(Resume resumetosave, XmlWriter writer, int numT)
        {
            //ElementList
            if (resumetosave.Layout.TextBoxes[numT].Element is ElementList<Element> list)
            {
                await writer.WriteStartElementAsync("", "ElementList", "Resume");
                await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.Name));

                int numj = 0;
                foreach (var j in list.Values)
                {

                    await writer.WriteStartElementAsync("", "Element_" + numj, "Resume");
                    await writer.WriteElementStringAsync("", "List_Name", "Resume", list.Name);
                    await writer.WriteElementStringAsync("", "List_Default", "Resume", Convert.ToString(list.IsDefault));
                    await writer.WriteElementStringAsync("", "List_ReadOnly", "Resume", Convert.ToString(list.IsReadOnly));

                    if (j is Data Ldata && !(j is Data<string>))
                    {
                        await writer.WriteStartElementAsync("", "Data_Element", "Resume");

                        await writer.WriteElementStringAsync("", "E_name", "Resume", resumetosave.Layout.TextBoxes[numT].Element.Name);
                        await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(Ldata.Level));
                        await writer.WriteElementStringAsync("", "D_name", "Resume", Ldata.Name);
                        await writer.WriteElementStringAsync("", "D_decription", "Resume", Ldata.Description);

                        await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(Ldata.IsIndependant));
                        await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(Ldata.IsDefault));
                        //liste des catégories associé a l'élément de la liste
                        await writer.WriteStartElementAsync("", "D_catergories", "Resume");
                        int numcat = 0;
                        foreach (var cats in Ldata.Categories)
                        {
                            await writer.WriteElementStringAsync("", "catergorie" + numcat, "Resume", cats.Name);
                            numcat += 1;
                        }
                        await writer.WriteEndElementAsync();

                        await writer.WriteEndElementAsync();
                    }

                    if (j is Data<string> dstr)
                    {
                        await writer.WriteStartElementAsync("", "DataString_Element", "Resume");
                        await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.Name));
                        await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(dstr.Level));
                        await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(dstr.Name));
                        await writer.WriteElementStringAsync("", "D_decription", "Resume", dstr.Description);
                        await writer.WriteElementStringAsync("", "D_value", "Resume", dstr.Value);
                        await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(dstr.IsIndependant));
                        await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(dstr.IsDefault));

                        //liste des catégories associé a l'élément de la liste
                        await writer.WriteStartElementAsync("", "D_catergories", "Resume");
                        int numcat = 0;
                        foreach (var cats in dstr.Categories)
                        {
                            await writer.WriteElementStringAsync("", "catergorie" + numcat, "Resume", cats.Name);
                            numcat += 1;
                        }
                        await writer.WriteEndElementAsync();

                        //fin de l'element
                        await writer.WriteEndElementAsync();
                    }

                    if (j is DataDated<string> ddstr)
                    {
                        await writer.WriteStartElementAsync("", "DataDated_Element", "Resume");

                        await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.Name));
                        await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(ddstr.Level));
                        await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(ddstr.Name));
                        await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(ddstr.Description));
                        await writer.WriteElementStringAsync("", "D_value", "Resume", ddstr.Value);
                        await writer.WriteElementStringAsync("", "D_start", "Resume", Convert.ToString(ddstr.StartTime));
                        await writer.WriteElementStringAsync("", "D_end", "Resume", Convert.ToString(ddstr.EndTime));
                        await writer.WriteElementStringAsync("", "D_Format", "Resume", Convert.ToString(ddstr.DisplayFormat));
                        await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(ddstr.IsIndependant));
                        await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(ddstr.IsDefault));
                        //liste des catégories associé a l'élément de la liste
                        await writer.WriteStartElementAsync("", "D_catergories", "Resume");
                        int numcat = 0;
                        foreach (var cats in ddstr.Categories)
                        {
                            await writer.WriteElementStringAsync("", "catergorie" + numcat, "Resume", cats.Name);
                            numcat += 1;
                        }
                        await writer.WriteEndElementAsync();

                        await writer.WriteEndElementAsync();
                    }
                    await writer.WriteEndElementAsync();
                    numj += 1;
                }
                await writer.WriteEndElementAsync();
            }

        }

        public static async void Read_file(Resume resumetoread)
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


            using (var stream = await folder.OpenStreamForWriteAsync(resumetoread.Name + ".cv", CreationCollisionOption.OpenIfExists))
            {

                XmlReaderSettings read_settings = new XmlReaderSettings();
                read_settings.Async = true;
                read_settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(stream, read_settings);




            }



        }
    }
}

