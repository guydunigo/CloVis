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


                    // await writer.WriteElementStringAsync("", "Default_Element", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].DefaultElement));

                    //Element
                    //Data
                    /* if (resumetosave.Layout.TextBoxes[numT].Element is Data data)
                      {
                          var box = resumetosave.Layout.TextBoxes[numT];

                          await writer.WriteStartElementAsync("", "Data_Element", "Resume");
                          await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(box.Element.Name));

                          await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(data.Level));
                          await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(data.Name));
                          await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(data.Description));
                          await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(data.IsIndependant));
                          await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(data.IsDefault));
                          await writer.WriteElementStringAsync("", "D_categories", "Resume", Convert.ToString(data.Categories));

                          await writer.WriteEndElementAsync();
                      }*/

                    //Data<string>
                    if (resumetosave.Layout.TextBoxes[numT].Element is Data<string> datestr)
                    {
                        var box = resumetosave.Layout.TextBoxes[numT];

                        await writer.WriteStartElementAsync("", "DataString_Element", "Resume");
                        await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(box.Element.Name));

                        await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(datestr.Level));
                        await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(datestr.Name));
                        await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(datestr.Description));
                        await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(datestr.IsIndependant));
                        await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(datestr.IsDefault));
                        await writer.WriteElementStringAsync("", "D_categories", "Resume", Convert.ToString(datestr.Categories));

                        await writer.WriteEndElementAsync();
                    }

                    if (resumetosave.Layout.TextBoxes[numT].Element is DataDated<string> dateD)
                    {
                        var box = resumetosave.Layout.TextBoxes[numT];

                        await writer.WriteStartElementAsync("", "DataDated_Element", "Resume");
                        await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(box.Element.Name));

                        await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(dateD.Level));
                        await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(dateD.Name));
                        await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(dateD.Description));
                        await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(dateD.IsIndependant));
                        await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(dateD.IsDefault));
                        await writer.WriteElementStringAsync("", "D_categories", "Resume", Convert.ToString(dateD.Categories));

                        await writer.WriteEndElementAsync();
                    }

                    //ElementList

                    if (resumetosave.Layout.TextBoxes[numT].Element is ElementList<Element> list)
                    {
                        var box = resumetosave.Layout.TextBoxes[numT];
                        await writer.WriteStartElementAsync("", "ElementList", "Resume");
                        await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(box.Element.Name));

                        int numj = 0;
                        foreach (var j in list)
                        {

                            await writer.WriteStartElementAsync("", "Elemnt_" + numj, "Resume");
                            await writer.WriteElementStringAsync("", "List_Name", "Resume", list.Name);
                            await writer.WriteElementStringAsync("", "List_Default", "Resume", Convert.ToString(list.IsDefault));
                            await writer.WriteElementStringAsync("", "List_ReadOnly", "Resume", Convert.ToString(list.IsReadOnly));
                           // await writer.WriteElementStringAsync("", "List_keys", "Resume", Convert.ToString(list.Keys)); // System.Collections.Generic.SortedDictionary`2+KeyCollection[System.String,ResumeElements.Element]
                           // await writer.WriteElementStringAsync("", "List_Values", "Resume", Convert.ToString(list.Values)); // System.Collections.Generic.SortedDictionary`2+ValueCollection[System.String,ResumeElements.Element]

                            
                            if (j is Data<string> dstr)
                            {
                    
                                await writer.WriteStartElementAsync("", "DataString_Element", "Resume");
                                await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.Name));

                                await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(dstr.Level));
                                await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(dstr.Name));
                                await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(dstr.Description));
                                await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(dstr.IsIndependant));
                                await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(dstr.IsDefault));
                                await writer.WriteElementStringAsync("", "D_categories", "Resume", Convert.ToString(dstr.Categories));

                                await writer.WriteEndElementAsync();
                            }
                            if (j is DataDated<string> ddstr)
                            {
                                await writer.WriteStartElementAsync("", "DataDated_Element", "Resume");
                                await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.Name));

                                await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(ddstr.Level));
                                await writer.WriteElementStringAsync("", "D_name", "Resume", Convert.ToString(ddstr.Name));
                                await writer.WriteElementStringAsync("", "D_decription", "Resume", Convert.ToString(ddstr.Description));
                                await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(ddstr.IsIndependant));
                                await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(ddstr.IsDefault));
                                await writer.WriteElementStringAsync("", "D_categories", "Resume", Convert.ToString(ddstr.Categories));

                                await writer.WriteEndElementAsync();
                            }
                            await writer.WriteEndElementAsync();
                            numj += 1;

                        }


                    }
                    

                      //  await writer.WriteElementStringAsync("", "Element_Default", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.IsDefault));
                      

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

