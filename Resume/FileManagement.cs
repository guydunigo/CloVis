
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
using Windows.UI;

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
            using (var stream = await folder.OpenStreamForWriteAsync(resumetosave.Name + ".cv", CreationCollisionOption.ReplaceExisting))
            {
                
                //Settings
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.Async = true;
                XmlWriter writer = XmlWriter.Create(stream, settings);

                XmlReaderSettings read_settings = new XmlReaderSettings();
                read_settings.Async = true;
                read_settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(stream, read_settings);

                // effacer tout le fichier !! A FAIRE

                await writer.WriteStartDocumentAsync();

                // écriture des donnees

                //resume (name)
                string name = resumetosave.Name.Replace(" ", "");
                if (reader.LocalName != name) await writer.WriteStartElementAsync("", name, "Resume");

                //Layout
                await writer.WriteStartElementAsync("", "Layout", "Resume");

                //BackBoxes
                int num = 0;
                foreach (var i in resumetosave.Layout.BackBoxes)
                {

                    await writer.WriteStartElementAsync("", "BackBox", "Resume");
                    await writer.WriteElementStringAsync("", "x", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].X));
                    await writer.WriteElementStringAsync("", "y", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Y));
                    await writer.WriteElementStringAsync("", "z", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Z));
                    await writer.WriteElementStringAsync("", "SizeX", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].SizeX));
                    await writer.WriteElementStringAsync("", "SizeY", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].SizeY));
                    await writer.WriteElementStringAsync("", "img", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Image));
                    await writer.WriteElementStringAsync("", "angle", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Angle));
                    await writer.WriteElementStringAsync("", "Color_A", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Color.A));
                    await writer.WriteElementStringAsync("", "Color_R", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Color.R));
                    await writer.WriteElementStringAsync("", "Color_G", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Color.G));
                    await writer.WriteElementStringAsync("", "Color_B", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Color.B));
                    await writer.WriteElementStringAsync("", "Border_Color_A", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderColor.A));
                    await writer.WriteElementStringAsync("", "Border_Color_R", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderColor.R));
                    await writer.WriteElementStringAsync("", "Border_Color_G", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderColor.G));
                    await writer.WriteElementStringAsync("", "Border_Color_B", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderColor.B));
                    await writer.WriteElementStringAsync("", "Border_radius", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderRadius));
                    await writer.WriteEndElementAsync();
                    num += 1;
                }

                //TextesBoxes
                int numT = 0;
                foreach (var i in resumetosave.Layout.TextBoxes)
                {
                    await writer.WriteStartElementAsync("", "TextBox", "Resume");
                    await writer.WriteElementStringAsync("", "x", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].X));
                    await writer.WriteElementStringAsync("", "y", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Y));
                    await writer.WriteElementStringAsync("", "z", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Z));
                    await writer.WriteElementStringAsync("", "SizeX", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].SizeX));
                    await writer.WriteElementStringAsync("", "SizeY", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].SizeY));
                    await writer.WriteElementStringAsync("", "angle", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Angle));
                    //Font
                    Save_Font(resumetosave.Layout.TextBoxes[numT], writer);
                    // await writer.WriteElementStringAsync("", "Default_Element", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].DefaultElement));
                    //element
                    Save_Element(resumetosave, writer, numT);
                    //elementList
                    if (resumetosave.Layout.TextBoxes[numT].Element is ElementList<Element> list) Save_ElementList(list, writer, numT);
                    await writer.WriteEndElementAsync();
                    numT += 1;
                }
                await writer.WriteEndElementAsync(); //Fin Layout
                Save_Font(resumetosave, writer);
                await writer.WriteEndElementAsync(); //Fin resume
                //vider le writer, fin de la sauvegarde
                await writer.WriteEndDocumentAsync();
                await writer.FlushAsync();
            }
        }
        public static async void Save_Font(BoxText box, XmlWriter writer)
        {
            if (box.Fonts != null)
            {
                await writer.WriteStartElementAsync("", "Fonts", "Resume");
                await writer.WriteElementStringAsync("", "Font_Alignment", "Resume", Convert.ToString(box.Fonts.TextAlignment));
                int numF = 0;
                foreach (var k in box.Fonts.List)
                {
                    await writer.WriteStartElementAsync("", "Font", "Resume");

                    await writer.WriteElementStringAsync("", "Font_Name", "Resume", k.Name);
                    await writer.WriteElementStringAsync("", "Font_font", "Resume", k.Font.Source);
                    await writer.WriteElementStringAsync("", "Font_fontSize", "Resume", Convert.ToString(k.FontSize));
                    await writer.WriteElementStringAsync("", "Font_Color", "Resume", Convert.ToString(k.Color));
                    await writer.WriteElementStringAsync("", "Font_Italic", "Resume", Convert.ToString(k.Italic));
                    await writer.WriteElementStringAsync("", "Font_Bold", "Resume", Convert.ToString(k.Bold));
                    await writer.WriteElementStringAsync("", "Font_Underlined", "Resume", Convert.ToString(k.Underlined));
                    await writer.WriteElementStringAsync("", "Font_UpperCase", "Resume", Convert.ToString(k.UpperCase));
                    await writer.WriteEndElementAsync();
                    numF += 1;
                }

                await writer.WriteEndElementAsync();
            }

        }
        public static async void Save_Font(Resume resumetosave, XmlWriter writer)
        {
            await writer.WriteStartElementAsync("", "Fonts", "Resume");
            await writer.WriteElementStringAsync("", "Font_Alignment", "Resume", Convert.ToString(resumetosave.Fonts.TextAlignment));
            int numF = 0;
            foreach (var k in resumetosave.Fonts.List)
            {
                await writer.WriteStartElementAsync("", "Font", "Resume");
                await writer.WriteElementStringAsync("", "Font_Name", "Resume", k.Name);
                await writer.WriteElementStringAsync("", "Font_font", "Resume", k.Font.Source);
                await writer.WriteElementStringAsync("", "Font_fontSize", "Resume", Convert.ToString(k.FontSize));
                await writer.WriteElementStringAsync("", "Font_Color", "Resume", Convert.ToString(k.Color));
                await writer.WriteElementStringAsync("", "Font_Italic", "Resume", Convert.ToString(k.Italic));
                await writer.WriteElementStringAsync("", "Font_Bold", "Resume", Convert.ToString(k.Bold));
                await writer.WriteElementStringAsync("", "Font_Underlined", "Resume", Convert.ToString(k.Underlined));
                await writer.WriteElementStringAsync("", "Font_UpperCase", "Resume", Convert.ToString(k.UpperCase));
                await writer.WriteEndElementAsync();
                numF += 1;
            }
            await writer.WriteEndElementAsync();
        }

        public static async void Save_Element(Resume resumetosave, XmlWriter writer, int numT)
        {
            //Simple Element

            //Data
            if (resumetosave.Layout.TextBoxes[numT].Element is Data data && !(resumetosave.Layout.TextBoxes[numT].Element is Data<string>))
            {
                await writer.WriteStartElementAsync("", "Data_Element", "Resume");

                await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(data.Level));
                await writer.WriteElementStringAsync("", "D_name", "Resume", data.Name);
                await writer.WriteElementStringAsync("", "D_decription", "Resume", data.Description);
                await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(data.IsIndependant));
                await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(data.IsDefault));
                //liste des catégories associé a l'élément
                await writer.WriteStartElementAsync("", "D_categories", "Resume");
                int numcat = 0;
                foreach (var cats in data.Categories)
                {
                    await writer.WriteElementStringAsync("", "categorie", "Resume", cats.Name);
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
                await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(datestr.Level));
                await writer.WriteElementStringAsync("", "D_name", "Resume", datestr.Name);
                await writer.WriteElementStringAsync("", "D_decription", "Resume", datestr.Description);
                await writer.WriteElementStringAsync("", "D_value", "Resume", datestr.Value);
                await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(datestr.IsIndependant));
                await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(datestr.IsDefault));
                //liste des catégories associé a l'élément
                await writer.WriteStartElementAsync("", "D_categories", "Resume");
                int numcat = 0;
                foreach (var cats in datestr.Categories)
                {
                    await writer.WriteElementStringAsync("", "categorie" + numcat, "Resume", cats.Name);
                    numcat += 1;
                }
                await writer.WriteEndElementAsync();

                await writer.WriteEndElementAsync();
            }

            //DataDated<string>
            if (resumetosave.Layout.TextBoxes[numT].Element is DataDated<string> dateD)
            {

                await writer.WriteStartElementAsync("", "DataDated_Element", "Resume");

                await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(dateD.Level));
                await writer.WriteElementStringAsync("", "D_name", "Resume", dateD.Name);
                await writer.WriteElementStringAsync("", "D_decription", "Resume", dateD.Description);
                await writer.WriteElementStringAsync("", "D_value", "Resume", dateD.Value);
                await writer.WriteElementStringAsync("", "D_start", "Resume", Convert.ToString(dateD.StartTime));
                await writer.WriteElementStringAsync("", "D_end", "Resume", Convert.ToString(dateD.EndTime));
                await writer.WriteElementStringAsync("", "D_Format", "Resume", dateD.DisplayFormat);
                await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(dateD.IsIndependant));
                await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(dateD.IsDefault));
                //liste des catégories associé a l'élément
                await writer.WriteStartElementAsync("", "D_categories", "Resume");
                int numcat = 0;
                foreach (var cats in dateD.Categories)
                {
                    await writer.WriteElementStringAsync("", "categorie" + numcat, "Resume", cats.Name);
                    numcat += 1;
                }
                await writer.WriteEndElementAsync();


                await writer.WriteEndElementAsync();
            }
            //  await writer.WriteElementStringAsync("", "Element_Default", "Resume", Convert.ToString(resumetosave.Layout.TextBoxes[numT].Element.IsDefault));

        }

        public static async void Save_ElementList(ElementList<Element> list, XmlWriter writer, int numT)
        {
            //ElementList

            await writer.WriteStartElementAsync("", "ElementList", "Resume");
            // await writer.WriteElementStringAsync("", "E_name", "Resume", Convert.ToString(box.Element.Name));

            await writer.WriteElementStringAsync("", "List_Name", "Resume", list.Name);
            await writer.WriteElementStringAsync("", "List_Default", "Resume", Convert.ToString(list.IsDefault));
            await writer.WriteElementStringAsync("", "List_ReadOnly", "Resume", Convert.ToString(list.IsReadOnly));

            int numj = 0;
            foreach (var j in list.Values)
            {
                await writer.WriteStartElementAsync("", "Element", "Resume");
                if (j is ElementList<Element> sous_list)
                {
                    Save_ElementList(sous_list, writer, numT);
                }

                if (j is Data Ldata && !(j is Data<string>))
                {
                    await writer.WriteStartElementAsync("", "Data_Element", "Resume");
                    await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(Ldata.Level));
                    await writer.WriteElementStringAsync("", "D_name", "Resume", Ldata.Name);
                    await writer.WriteElementStringAsync("", "D_decription", "Resume", Ldata.Description);

                    await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(Ldata.IsIndependant));
                    await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(Ldata.IsDefault));
                    //liste des catégories associé a l'élément de la liste
                    await writer.WriteStartElementAsync("", "D_categories", "Resume");
                    int numcat = 0;
                    foreach (var cats in Ldata.Categories)
                    {
                        await writer.WriteElementStringAsync("", "categorie", "Resume", cats.Name);
                        numcat += 1;
                    }
                    await writer.WriteEndElementAsync();

                    await writer.WriteEndElementAsync();
                }

                if (j is Data<string> dstr)
                {
                    await writer.WriteStartElementAsync("", "DataString_Element", "Resume");
                    await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(dstr.Level));
                    await writer.WriteElementStringAsync("", "D_name", "Resume", dstr.Name);
                    await writer.WriteElementStringAsync("", "D_decription", "Resume", dstr.Description);
                    await writer.WriteElementStringAsync("", "D_value", "Resume", dstr.Value);
                    await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(dstr.IsIndependant));
                    await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(dstr.IsDefault));

                    //liste des catégories associé a l'élément de la liste
                    await writer.WriteStartElementAsync("", "D_catergories", "Resume");
                    int numcat = 0;
                    foreach (var cats in dstr.Categories)
                    {
                        await writer.WriteElementStringAsync("", "catergorie", "Resume", cats.Name);
                        numcat += 1;
                    }
                    await writer.WriteEndElementAsync();

                    //fin de l'element
                    await writer.WriteEndElementAsync();
                }

                if (j is DataDated<string> ddstr)
                {
                    await writer.WriteStartElementAsync("", "DataDated_Element", "Resume");

                    await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(ddstr.Level));
                    await writer.WriteElementStringAsync("", "D_name", "Resume", ddstr.Name);
                    await writer.WriteElementStringAsync("", "D_decription", "Resume", ddstr.Description);
                    await writer.WriteElementStringAsync("", "D_value", "Resume", ddstr.Value);
                    await writer.WriteElementStringAsync("", "D_start", "Resume", Convert.ToString(ddstr.StartTime));
                    await writer.WriteElementStringAsync("", "D_end", "Resume", Convert.ToString(ddstr.EndTime));
                    await writer.WriteElementStringAsync("", "D_Format", "Resume", ddstr.DisplayFormat);
                    await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(ddstr.IsIndependant));
                    await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(ddstr.IsDefault));
                    //liste des catégories associé a l'élément de la liste
                    await writer.WriteStartElementAsync("", "D_catergories", "Resume");
                    int numcat = 0;
                    foreach (var cats in ddstr.Categories)
                    {
                        await writer.WriteElementStringAsync("", "catergorie", "Resume", cats.Name);
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

        public static async Task<Resume> Read_file(string filename)
        {
            StorageFolder folder = null;

            Resume resumetoread = new Resume(filename, false);

            try
            {
                folder = await ApplicationData.Current.LocalFolder.GetFolderAsync("CVs_CloVis");
            }
            catch (FileNotFoundException)
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("CVs_CloVis");
            }

            using (var stream = await folder.OpenStreamForReadAsync(filename + ".cv"))
            {

                XmlReaderSettings read_settings = new XmlReaderSettings();
                read_settings.Async = true;
                read_settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(stream, read_settings);

                string box = "";
                string balise = "";
                string[] D_format = new string[10], D_name = new string[10], D_description = new string[10], D_value = new string[10];
                string[] L_Name = {"", "", "",""};
                bool[] D_dependant = new bool[10], D_def = new bool[10], L_def = new bool[10], L_ReadOnly = new bool[10];
                string[] categorie = new string[10];

                double x = 0, y = 0, z = 0, SizeX = 0, SizeY = 0, angle = 0;
                double[] D_level = new double[10];
                byte Color_A = 0, Color_R = 0, Color_G = 0, Color_B = 0, Color_Border_A = 0, Color_Border_R = 0, Color_Border_G = 0, Color_Border_B = 0;
                int liste = 0, elem = 0, dts = 0, dde = 0, dt = 0;

                var nv = new ElementList<Element>("");

                resumetoread.Layout = new Layout();
                resumetoread.Fonts = new Fonts("Polices_cv");

                while (reader.Read())
                {

                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (reader.Name == "BackBox" || reader.Name == "TextBox") box = reader.Name;
                                if (reader.Name == "ElementList") liste += 1;
                                if (reader.Name == "Element") elem += 1;
                                if (reader.Name == "DataString_Element") dts += 1;
                                if (reader.Name == "DataDated_Element") dde += 1;
                                if (reader.Name == "Data_Element") dt +=1;
                                else balise = reader.Name;
                                break;
                            }
                        case XmlNodeType.Text:
                            {
                                if (box == "TextBox" || box == "BackBox")
                                {
                                    if (balise == "x") x = double.Parse(reader.Value);
                                    if (balise == "y") y = double.Parse(reader.Value);
                                    if (balise == "z") z = double.Parse(reader.Value);
                                    if (balise == "SizeX") SizeX = double.Parse(reader.Value);
                                    if (balise == "SizeY") SizeY = double.Parse(reader.Value);
                                    if (balise == "angle") angle = double.Parse(reader.Value);
                                }
                                if (box == "BackBox")
                                {
                                    if (balise == "Color_A") Color_A = byte.Parse(reader.Value);
                                    if (balise == "Color_R") Color_R = byte.Parse(reader.Value);
                                    if (balise == "Color_G") Color_G = byte.Parse(reader.Value);
                                    if (balise == "Color_B") Color_B = byte.Parse(reader.Value);
                                    if (balise == "Color_Border_A") Color_Border_A = byte.Parse(reader.Value);
                                    if (balise == "Color_Border_R") Color_Border_R = byte.Parse(reader.Value);
                                    if (balise == "Color_Border_G") Color_Border_G = byte.Parse(reader.Value);
                                    if (balise == "Color_Border_B") Color_Border_B = byte.Parse(reader.Value);

                                }
                                if (liste >= 0)
                                {
                                    if (elem >= 0)
                                    {
                                        if (dts >= 1) // si c'est un data string
                                        {
                                            if (balise == "D_level") D_level[dts] = double.Parse(reader.Value);
                                            if (balise == "D_name") D_name[dts] = reader.Value;
                                            if (balise == "D_description") D_description[dts] = reader.Value;
                                            if (balise == "D_value") D_value[dts] = reader.Value;
                                            if (balise == "D_dependant") D_dependant[dts] = bool.Parse(reader.Value);
                                            if (balise == "D_default") D_def[dts] = bool.Parse(reader.Value);
                                            if (balise == "D_categories") categorie[dts] = reader.Value;
                                        }

                                        if (dde >=1)
                                        {
                                            //COMPLETER
                                        }
                                        if (dt >=1)
                                        {
                                            // completer
                                        }
                                    }
                                }
                                if (liste >= 1)
                                {
                                    if (balise == "List_Name")
                                    {
                                        L_Name[liste] = reader.Value;
                                        
                                    }
                                    if (balise == "List_Default")
                                    {
                                        L_def[liste] = bool.Parse(reader.Value);
                                    }
                                    if (balise == "List_ReadOnly") L_ReadOnly[liste] = bool.Parse(reader.Value);

                                    if (elem >= 1) // si un element
                                    {
                                        if (dde >=1) // si c'est un data dated 
                                        {
                                            // completer
                                        }
                                        if (dt>=1) // si c'est un data
                                        {
                                            //completer
                                        }
                                        if (dts >= 1) // si c'est un data string
                                        {
                                            if (balise == "D_level") D_level[dts] = double.Parse(reader.Value);
                                            if (balise == "D_name")  D_name[dts] = reader.Value;
                                            if (balise == "D_description") D_description[dts] = reader.Value;
                                            if (balise == "D_value") D_value[dts] = reader.Value;
                                            if (balise == "D_dependant") D_dependant[dts] = bool.Parse(reader.Value);
                                            if (balise == "D_default") D_def[dts] = bool.Parse(reader.Value);
                                            if (balise == "D_categories") categorie[dts] = reader.Value;
                                        }
                                    }
                                }

                                break;
                            }
                        case XmlNodeType.EndElement: // prend n'importe quel end !! 
                            {
                                if (box == "BackBox" && reader.Name == "BackBox") // fonctionne
                                {
                                    resumetoread.Layout.AddBackBox(new BoxBackground(new Color() { A = Color_A, R = Color_R, G = Color_G, B = Color_B }, new Color() { A = Color_Border_A, R = Color_Border_R, G = Color_Border_G, B = Color_Border_B }, x, y, z, SizeX, SizeY, null, angle));
                                    box = "";
                                    x = 0; y = 0; z = 0; SizeX = 0; SizeY = 0; angle = 0; D_level[elem] = 0;
                                    Color_A = 0; Color_R = 0; Color_G = 0; Color_B = 0; Color_Border_A = 0; Color_Border_R = 0; Color_Border_G = 0; Color_Border_B = 0;
                                }
                                if (liste>= 1 && reader.Name == "ElementList") // si c'est une liste imbriquée -- BEUG
                                {
                                    nv = new ElementList<Element>(L_Name[liste], L_def[liste]);

                                    while (dde >=1) 
                                    {
                                        // completer
                                    }
                                    while (dt >=1)
                                    {
                                        //completer
                                    }

                                    while (dts >= 1 /*&& elem >= 1*/)
                                    {
                                        nv.Add(new Data<string>(D_value[dts], D_level[dts], D_description[dts], D_dependant[dts], D_def[dts]));
                                        D_format[dts] = ""; D_name[dts] = ""; D_description[dts] = ""; D_value[dts] = ""; D_dependant[dts] = false; D_def[dts] = false;
                                        categorie[dts] = "";
                                        elem -= 1; dts -= 1;
                                    }
                                    //réinitialisation 
                                    box = "";
                                    L_Name[liste] = ""; L_def[liste] = false;
                                    L_ReadOnly[liste] = false;
                                    //décrémenter les listes
                                     liste -= 1;
                                }
                                if (liste == 1 && reader.Name == "ElementList")
                                {

                                    var bnv = new BoxText(x, y, z, SizeX, SizeY, angle, L_Name[liste]);
                                    if (nv.Name != "")
                                    {
                                        var souslist = nv;
                                        nv = new ElementList<Element>(L_Name[liste], L_def[liste]); // n'a pas de nom !
                                        nv.Add(souslist);
                                        bnv.Element = nv;
                                        L_Name[liste] = ""; L_def[liste] = false;
                                    }
                                    else
                                    {
                                        nv = new ElementList<Element>(L_Name[liste], L_def[liste]);
                                        while (dde >=1 && elem >=1) 
                                        {
                                            // completer
                                        }
                                        while (dt>=1 && elem >=1)
                                        {
                                            //completer
                                        }
                                        while (dts >= 1 && elem >= 1)
                                        {
                                            nv.Add(new Data<string>(D_value[dts], D_level[dts], D_description[dts], D_dependant[dts], D_def[dts]));
                                            D_format[dts] = ""; D_name[dts] = ""; D_description[dts] = ""; D_value[dts] = ""; D_dependant[dts] = false; D_def[dts] = false; D_level[dts] = 0;
                                            categorie[dts] = "";
                                            elem -= 1; dts -= 1;
                                        }
                                        //réinitialisation 
                                        L_Name[liste] = ""; L_def[liste] = false;
                                        L_ReadOnly[liste] = false; 
                                        bnv.Element = nv;
                                    }
                                    resumetoread.Layout.AddTextBox(bnv);
                                    x = 0; y = 0; z = 0; SizeX = 0; SizeY = 0; angle = 0;  
                                    liste -= 1; elem -= 1;
                                    box = "";
                                }
                                if (liste == 0 && reader.Name == "DataString_Element") // fonctionne
                                {
                                    if (dde == 1 && elem == 0)
                                    {
                                        // completer
                                    }
                                    if (dt ==1 && elem ==0)
                                    {
                                        //completer
                                    }
                                    if (dts == 1 && elem == 0)
                                    {
                                        dts = 0;
                                        var nvdt = new Data<string>(D_value[dts], D_level[dts], D_description[dts], D_dependant[dts], D_def[dts]);
                                        var bnv = new BoxText(x, y, z, SizeX, SizeY, angle, D_name[dts]);
                                        bnv.Element = nvdt;
                                        resumetoread.Layout.AddTextBox(bnv);

                                        //réinitialisation
                                        box = "";
                                        D_name[dts] = ""; D_description[dts] = ""; D_value[dts] = "";
                                        D_dependant[dts] = false; D_def[dts] = false;
                                        x = 0; y = 0; z = 0; SizeX = 0; SizeY = 0; angle = 0; D_level[dts] = 0;
                                    }
                                }
                                break;
                            }
                    }
                }
            }

            return resumetoread;
        }
    }
}

