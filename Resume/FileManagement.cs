
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Windows.Storage;
using ResumeElements;
using Windows.UI;
using Windows.UI.Xaml;

namespace Resume
{
    public static class FileManagement
    {
        public async static Task<StorageFolder> GetLocalResumeFolder()
        {
            var name = "CVs_Clovis";
            StorageFolder folder;
            try
            {
                folder = await ApplicationData.Current.LocalFolder.GetFolderAsync(name);
            }
            catch (FileNotFoundException)
            {
                folder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(name);
            }
            return folder;
        }

        public static async void Save_File(Resume resumetosave)
        {
            StorageFolder folder = await  GetLocalResumeFolder();

            using (var stream = await folder.OpenStreamForWriteAsync(resumetosave.Name + ".cv", CreationCollisionOption.ReplaceExisting))
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                settings.Async = true;
                XmlWriter writer = XmlWriter.Create(stream, settings);

                XmlReaderSettings read_settings = new XmlReaderSettings();
                read_settings.Async = true;
                read_settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(stream, read_settings);

                await writer.WriteStartDocumentAsync();

                string name = resumetosave.Name.Replace(" ", "");
                if (reader.LocalName != name) await writer.WriteStartElementAsync("", name, "Resume");

                await writer.WriteStartElementAsync("", "Layout", "Resume");

                int num = 0;
                foreach (var i in resumetosave.Layout.BackBoxes)
                {
                    await writer.WriteStartElementAsync("", "BackBox", "Resume");
                    await writer.WriteElementStringAsync("", "x", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].X));
                    await writer.WriteElementStringAsync("", "y", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Y));
                    await writer.WriteElementStringAsync("", "z", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Z));
                    await writer.WriteElementStringAsync("", "SizeX", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].SizeX));
                    await writer.WriteElementStringAsync("", "SizeY", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].SizeY));
                    if (resumetosave.Layout.BackBoxes[num].Image != null)
                    {
                        await writer.WriteElementStringAsync("", "img_name", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Image.Name));
                        await writer.WriteElementStringAsync("", "img_dependant", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Image.IsIndependant));
                    }
                    await writer.WriteElementStringAsync("", "angle", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Angle));
                    await writer.WriteElementStringAsync("","Shape","Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Shape));
                    await writer.WriteElementStringAsync("", "Color_A", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Fill.A));
                    await writer.WriteElementStringAsync("", "Color_R", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Fill.R));
                    await writer.WriteElementStringAsync("", "Color_G", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Fill.G));
                    await writer.WriteElementStringAsync("", "Color_B", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Fill.B));
                    await writer.WriteElementStringAsync("", "StrokeThikness", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].StrokeThickness));
                    await writer.WriteElementStringAsync("", "Border_Color_A", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Stroke.A));
                    await writer.WriteElementStringAsync("", "Border_Color_R", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Stroke.R));
                    await writer.WriteElementStringAsync("", "Border_Color_G", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Stroke.G));
                    await writer.WriteElementStringAsync("", "Border_Color_B", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].Stroke.B));
                    await writer.WriteElementStringAsync("", "Border_radius", "Resume", Convert.ToString(resumetosave.Layout.BackBoxes[num].BorderRadius));
                    await writer.WriteEndElementAsync();
                    num += 1;
                }

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
                    Save_Font(resumetosave.Layout.TextBoxes[numT], writer);
                    Save_Element(resumetosave, writer, numT);
                    if (resumetosave.Layout.TextBoxes[numT].Element is ElementList<Element> list) Save_ElementList(list, writer, numT);
                    await writer.WriteEndElementAsync();
                    numT += 1;
                }
                await writer.WriteEndElementAsync();
                Save_Font(resumetosave, writer);
                await writer.WriteEndElementAsync(); 
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
                    await writer.WriteElementStringAsync("", "Font_font", "Resume", k.FontName);
                    await writer.WriteElementStringAsync("", "Font_fontSize", "Resume", Convert.ToString(k.FontSize));
                    await writer.WriteElementStringAsync("", "Font_Color_A", "Resume", Convert.ToString(k.Color.A));
                    await writer.WriteElementStringAsync("", "Font_Color_R", "Resume", Convert.ToString(k.Color.R));
                    await writer.WriteElementStringAsync("", "Font_Color_G", "Resume", Convert.ToString(k.Color.G));
                    await writer.WriteElementStringAsync("", "Font_Color_B", "Resume", Convert.ToString(k.Color.B));
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
                await writer.WriteElementStringAsync("", "Font_font", "Resume", k.FontName);
                await writer.WriteElementStringAsync("", "Font_fontSize", "Resume", Convert.ToString(k.FontSize));
                await writer.WriteElementStringAsync("", "Font_Color_A", "Resume", Convert.ToString(k.Color.A));
                await writer.WriteElementStringAsync("", "Font_Color_R", "Resume", Convert.ToString(k.Color.R));
                await writer.WriteElementStringAsync("", "Font_Color_G", "Resume", Convert.ToString(k.Color.G));
                await writer.WriteElementStringAsync("", "Font_Color_B", "Resume", Convert.ToString(k.Color.B));
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
            if (resumetosave.Layout.TextBoxes[numT].Element is Data data && !(resumetosave.Layout.TextBoxes[numT].Element is Data<string>))
            {
                await writer.WriteStartElementAsync("", "Data_Element", "Resume");
                await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(data.Level));
                await writer.WriteElementStringAsync("", "D_name", "Resume", data.Name);
                await writer.WriteElementStringAsync("", "D_description", "Resume", data.Description);
                await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(data.IsIndependant));
                await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(data.IsDefault));
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
            if (resumetosave.Layout.TextBoxes[numT].Element is Data<string> datestr)
            {
                var box = resumetosave.Layout.TextBoxes[numT];
                await writer.WriteStartElementAsync("", "DataString_Element", "Resume");
                await writer.WriteElementStringAsync("", "DS_level", "Resume", Convert.ToString(datestr.Level));
                await writer.WriteElementStringAsync("", "DS_name", "Resume", datestr.Name);
                await writer.WriteElementStringAsync("", "DS_description", "Resume", datestr.Description);
                await writer.WriteElementStringAsync("", "DS_value", "Resume", datestr.Value);
                await writer.WriteElementStringAsync("", "DS_dependant", "Resume", Convert.ToString(datestr.IsIndependant));
                await writer.WriteElementStringAsync("", "DS_default", "Resume", Convert.ToString(datestr.IsDefault));
                await writer.WriteStartElementAsync("", "DS_categories", "Resume");
                int numcat = 0;
                foreach (var cats in datestr.Categories)
                {
                    await writer.WriteElementStringAsync("", "categorie" + numcat, "Resume", cats.Name);
                    numcat += 1;
                }
                await writer.WriteEndElementAsync();
                await writer.WriteEndElementAsync();
            }
            if (resumetosave.Layout.TextBoxes[numT].Element is DataDated<string> dateD)
            {
                await writer.WriteStartElementAsync("", "DataDated_Element", "Resume");
                await writer.WriteElementStringAsync("", "DD_level", "Resume", Convert.ToString(dateD.Level));
                await writer.WriteElementStringAsync("", "DD_name", "Resume", dateD.Name);
                await writer.WriteElementStringAsync("", "DD_description", "Resume", dateD.Description);
                await writer.WriteElementStringAsync("", "DD_value", "Resume", dateD.Value);
                await writer.WriteElementStringAsync("", "DD_start", "Resume", Convert.ToString(dateD.StartTime));
                await writer.WriteElementStringAsync("", "DD_end", "Resume", Convert.ToString(dateD.EndTime));
                await writer.WriteElementStringAsync("", "DD_Format", "Resume", dateD.DisplayFormat);
                await writer.WriteElementStringAsync("", "DD_dependant", "Resume", Convert.ToString(dateD.IsIndependant));
                await writer.WriteElementStringAsync("", "DD_default", "Resume", Convert.ToString(dateD.IsDefault));
                await writer.WriteStartElementAsync("", "DD_categories", "Resume");
                int numcat = 0;
                foreach (var cats in dateD.Categories)
                {
                    await writer.WriteElementStringAsync("", "categorie" + numcat, "Resume", cats.Name);
                    numcat += 1;
                }
                await writer.WriteEndElementAsync();
                await writer.WriteEndElementAsync();
            }
        }

        public static async void Save_ElementList(ElementList<Element> list, XmlWriter writer, int numT)
        {
            await writer.WriteStartElementAsync("", "ElementList", "Resume");
            await writer.WriteElementStringAsync("", "List_Name", "Resume", list.Name);
            await writer.WriteElementStringAsync("", "List_Default", "Resume", Convert.ToString(list.IsDefault));
            await writer.WriteElementStringAsync("", "List_ReadOnly", "Resume", Convert.ToString(list.IsReadOnly));
            int numj = 0;
            foreach (var j in list.Values)
            {
                await writer.WriteStartElementAsync("", "Element", "Resume");
                if (j is ElementList<Element> sous_list)  Save_ElementList(sous_list, writer, numT);
                if (j is Data Ldata && !(j is Data<string>))
                {
                    await writer.WriteStartElementAsync("", "Data_Element", "Resume");
                    await writer.WriteElementStringAsync("", "D_level", "Resume", Convert.ToString(Ldata.Level));
                    await writer.WriteElementStringAsync("", "D_name", "Resume", Ldata.Name);
                    await writer.WriteElementStringAsync("", "D_description", "Resume", Ldata.Description);
                    await writer.WriteElementStringAsync("", "D_dependant", "Resume", Convert.ToString(Ldata.IsIndependant));
                    await writer.WriteElementStringAsync("", "D_default", "Resume", Convert.ToString(Ldata.IsDefault));
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
                    await writer.WriteElementStringAsync("", "DS_level", "Resume", Convert.ToString(dstr.Level));
                    await writer.WriteElementStringAsync("", "DS_name", "Resume", dstr.Name);
                    await writer.WriteElementStringAsync("", "DS_description", "Resume", dstr.Description);
                    await writer.WriteElementStringAsync("", "DS_value", "Resume", dstr.Value);
                    await writer.WriteElementStringAsync("", "DS_dependant", "Resume", Convert.ToString(dstr.IsIndependant));
                    await writer.WriteElementStringAsync("", "DS_default", "Resume", Convert.ToString(dstr.IsDefault));
                    await writer.WriteStartElementAsync("", "DS_categories", "Resume");
                    int numcat = 0;
                    foreach (var cats in dstr.Categories)
                    {
                        await writer.WriteElementStringAsync("", "categorie", "Resume", cats.Name);
                        numcat += 1;
                    }
                    await writer.WriteEndElementAsync();
                    await writer.WriteEndElementAsync();
                }

                if (j is DataDated<string> ddstr)
                {
                    await writer.WriteStartElementAsync("", "DataDated_Element", "Resume");
                    await writer.WriteElementStringAsync("", "DD_level", "Resume", Convert.ToString(ddstr.Level));
                    await writer.WriteElementStringAsync("", "DD_name", "Resume", ddstr.Name);
                    await writer.WriteElementStringAsync("", "DD_description", "Resume", ddstr.Description);
                    await writer.WriteElementStringAsync("", "DD_value", "Resume", ddstr.Value);
                    await writer.WriteElementStringAsync("", "DD_start", "Resume", Convert.ToString(ddstr.StartTime));
                    await writer.WriteElementStringAsync("", "DD_end", "Resume", Convert.ToString(ddstr.EndTime));
                    await writer.WriteElementStringAsync("", "DD_Format", "Resume", ddstr.DisplayFormat);
                    await writer.WriteElementStringAsync("", "DD_dependant", "Resume", Convert.ToString(ddstr.IsIndependant));
                    await writer.WriteElementStringAsync("", "DD_default", "Resume", Convert.ToString(ddstr.IsDefault));
                    await writer.WriteStartElementAsync("", "DD_categories", "Resume");
                    int numcat = 0;
                    foreach (var cats in ddstr.Categories)
                    {
                        await writer.WriteElementStringAsync("", "categorie", "Resume", cats.Name);
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
            StorageFolder folder = await GetLocalResumeFolder();
            Resume resumetoread = new Resume(filename);

            using (var stream = await folder.OpenStreamForReadAsync(filename + ".cv"))
            {
                XmlReaderSettings read_settings = new XmlReaderSettings();
                read_settings.Async = true;
                read_settings.IgnoreWhitespace = true;
                XmlReader reader = XmlReader.Create(stream, read_settings);

                //---------variables utiles
                //mise en forme  
                int liste = 0, elem = 0, dts = 0, dde = 0, dt = 0, ft = 0;
                string box = "";
                string balise = "";
                BoxBackgroundShape Shape = BoxBackgroundShape.Rectangle;
                string Img_name = "";
                bool Img_dep = false;
                double strokethikness =0 ;
                double x = 0, y = 0, z = 0, SizeX = 0, SizeY = 0, angle = 0;
                byte Color_A = 0, Color_R = 0, Color_G = 0, Color_B = 0, Color_Border_A = 0, Color_Border_R = 0, Color_Border_G = 0, Color_Border_B = 0;
                //data 

                // data string 
                string[] DS_name = new string[10], DS_description = new string[10], DS_value = new string[10];
                double[] DS_level = new double[10];
                bool[] DS_dependant = new bool[10], DS_def = new bool[10];
                string[,] DS_categorie = new string[10,10];
                int dscatnum = 0;

                //liste 
                bool[] L_def = new bool[10], L_ReadOnly = new bool[10];
                string[] L_Name = new string[10];
                var nv = new ElementList<Element>("");

                //datadated
                string[] DD_format = new string[10], DD_name = new string[10], DD_description = new string[10], DD_value = new string[10];
                double[] DD_level = new double[10];
                bool[] DD_dependant = new bool[10], DD_def = new bool[10];
                string[,] DD_categorie = new string[10,10];
                DateTime[] DD_start = new DateTime[10], DD_end = new DateTime[10];
                for (int i = 0; i < 10; i++) { DD_start[i] = default(DateTime); DD_end[i] = default(DateTime); }
                int ddcatnum = 0;

                //fonts générales
                Windows.UI.Xaml.TextAlignment Fts_alignment = new Windows.UI.Xaml.TextAlignment();
                string[] Ft_name = new string[10], Ft_source = new string[10];
                double[] Ft_size = new double[10];
                byte[] Ft_color_a = new byte[10], Ft_color_r = new byte[10], Ft_color_g = new byte[10], Ft_color_b = new byte[10];
                bool[] Ft_italic = new bool[10], Ft_bold = new bool[10], Ft_underlined = new bool[10], Ft_uppercase = new bool[10];

                // fonts d'une boite
                int Ft_text = 0, ft_text = 0;
                Windows.UI.Xaml.TextAlignment Fts_text_alignment = new Windows.UI.Xaml.TextAlignment();
                string[] Ft_text_name = new string[10], Ft_text_source = new string[10];
                double[] Ft_text_size = new double[10];
                byte[] Ft_text_color_a = new byte[10], Ft_text_color_r = new byte[10], Ft_text_color_g = new byte[10], Ft_text_color_b = new byte[10];
                bool[] Ft_text_italic = new bool[10], Ft_text_bold = new bool[10], Ft_text_underlined = new bool[10], Ft_text_uppercase = new bool[10];

                //---------début lecture
                resumetoread.Layout = new Layout();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            {
                                if (reader.Name == "BackBox" || reader.Name == "TextBox" || (reader.Name == "Fonts" && box != "TextBox")) box = reader.Name;
                                if (reader.Name == "ElementList") liste += 1;
                                if (reader.Name == "Element") elem += 1;
                                if (reader.Name == "DataString_Element") dts += 1;
                                if (reader.Name == "DataDated_Element") dde += 1;
                                if (reader.Name == "Data_Element") dt += 1;
                                if (reader.Name == "Font" && box != "TextBox") ft += 1;
                                if (reader.Name == "Font" && box == "TextBox")
                                {
                                    ft_text += 1;
                                }
                                if (reader.Name == "Fonts" && box == "TextBox") Ft_text = 1;
                                else balise = reader.Name;
                                break;
                            }
                        case XmlNodeType.Text:
                            {
                                if (Ft_text == 1)
                                {
                                    if (balise == "Font_Alignment")
                                    {
                                        if (reader.Value == "Left") Fts_text_alignment = Windows.UI.Xaml.TextAlignment.Left;
                                        if (reader.Value == "Right") Fts_text_alignment = Windows.UI.Xaml.TextAlignment.Right;
                                        if (reader.Value == "Justify") Fts_text_alignment = Windows.UI.Xaml.TextAlignment.Justify;
                                    }
                                }
                                if (ft_text >= 1)
                                {
                                    if (balise == "Font_Name") Ft_text_name[ft_text] = reader.Value;
                                    if (balise == "Font_font")
                                    {
                                        Ft_text_source[ft_text] = reader.Value;
                                    }
                                    if (balise == "Font_fontSize") Ft_text_size[ft_text] = double.Parse(reader.Value);
                                    if (balise == "Font_Color_A") Ft_text_color_a[ft_text] = byte.Parse(reader.Value);
                                    if (balise == "Font_Color_R") Ft_text_color_r[ft_text] = byte.Parse(reader.Value);
                                    if (balise == "Font_Color_G") Ft_text_color_g[ft_text] = byte.Parse(reader.Value);
                                    if (balise == "Font_Color_B") Ft_text_color_b[ft_text] = byte.Parse(reader.Value);
                                    if (balise == "Font_Italic") Ft_text_italic[ft_text] = bool.Parse(reader.Value);
                                    if (balise == "Font_Bold") Ft_text_bold[ft_text] = bool.Parse(reader.Value);
                                    if (balise == "Font_Underlined") Ft_text_underlined[ft_text] = bool.Parse(reader.Value);
                                    if (balise == "Font_UpperCase") Ft_text_uppercase[ft_text] = bool.Parse(reader.Value);
                                }
                                if (box == "Fonts")
                                {
                                    if (balise == "Font_Alignment")
                                    {
                                        if (reader.Value == "Left") Fts_alignment = Windows.UI.Xaml.TextAlignment.Left;
                                        if (reader.Value == "Right") Fts_alignment = Windows.UI.Xaml.TextAlignment.Right;
                                        if (reader.Value == "Justify") Fts_alignment = Windows.UI.Xaml.TextAlignment.Justify;
                                    }
                                }
                                if (ft >= 1)
                                {
                                    if (balise == "Font_Name") Ft_name[ft] = reader.Value;
                                    if (balise == "Font_font") Ft_source[ft] = reader.Value;
                                    if (balise == "Font_fontSize") Ft_size[ft] = double.Parse(reader.Value);
                                    if (balise == "Font_Color_A") Ft_color_a[ft] = byte.Parse(reader.Value);
                                    if (balise == "Font_Color_R") Ft_color_r[ft] = byte.Parse(reader.Value);
                                    if (balise == "Font_Color_G") Ft_color_g[ft] = byte.Parse(reader.Value);
                                    if (balise == "Font_Color_B") Ft_color_b[ft] = byte.Parse(reader.Value);
                                    if (balise == "Font_Italic") Ft_italic[ft] = bool.Parse(reader.Value);
                                    if (balise == "Font_Bold") Ft_bold[ft] = bool.Parse(reader.Value);
                                    if (balise == "Font_Underlined") Ft_underlined[ft] = bool.Parse(reader.Value);
                                    if (balise == "Font_UpperCase") Ft_uppercase[ft] = bool.Parse(reader.Value);
                                }
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
                                    if (balise == "Shape")
                                    {
                                        if (reader.Value == "Rectangle") Shape = BoxBackgroundShape.Rectangle;
                                        else Shape = BoxBackgroundShape.Ellipse;
                                    }
                                    if (balise == "img_name") Img_name = reader.Value;
                                    if (balise == "img_dependant") Img_dep = bool.Parse(reader.Value);
                                    if (balise == "StrokeThikness") strokethikness = double.Parse(reader.Value);
                                    if (balise == "Color_A") Color_A = byte.Parse(reader.Value);
                                    if (balise == "Color_R") Color_R = byte.Parse(reader.Value);
                                    if (balise == "Color_G") Color_G = byte.Parse(reader.Value);
                                    if (balise == "Color_B") Color_B = byte.Parse(reader.Value);
                                    if (balise == "Color_Border_A") Color_Border_A = byte.Parse(reader.Value);
                                    if (balise == "Color_Border_R") Color_Border_R = byte.Parse(reader.Value);
                                    if (balise == "Color_Border_G") Color_Border_G = byte.Parse(reader.Value);
                                    if (balise == "Color_Border_B") Color_Border_B = byte.Parse(reader.Value);

                                }
                                if (dts >= 1)
                                {
                                    if (balise == "DS_level") DS_level[dts] = double.Parse(reader.Value);
                                    if (balise == "DS_name") DS_name[dts] = reader.Value;
                                    if (balise == "DS_description") DS_description[dts] = reader.Value;
                                    if (balise == "DS_value") DS_value[dts] = reader.Value;
                                    if (balise == "DS_dependant") DS_dependant[dts] = bool.Parse(reader.Value);
                                    if (balise == "DS_default") DS_def[dts] = bool.Parse(reader.Value);
                                    if (balise == "DS_categorie") { dscatnum += 1; DS_categorie[dts, dscatnum] = reader.Value; }
                                }
                                if (dde >= 1)
                                {
                                    if (balise == "DD_level") DD_level[dde] = double.Parse(reader.Value);
                                    if (balise == "Dd_name") DD_name[dde] = reader.Value;
                                    if (balise == "DD_description") DD_description[dde] = reader.Value;
                                    if (balise == "DD_value") DD_value[dde] = reader.Value;
                                    if (balise == "DD_dependant") DD_dependant[dde] = bool.Parse(reader.Value);
                                    if (balise == "DD_default") DD_def[dde] = bool.Parse(reader.Value);
                                    if (balise == "DD_categorie") { ddcatnum += 1; DD_categorie[dde, ddcatnum] = reader.Value;  }
                                    if (balise == "DD_Format") DD_format[dde] = reader.Value;
                                    if (balise == "DD_start") DD_start[dde] = Convert.ToDateTime(reader.Value);
                                    if (balise == "DD_end") DD_end[dde] = Convert.ToDateTime(reader.Value);
                                }
                                if (dt >= 1) { }
                                if (liste >= 1)
                                {
                                    if (balise == "List_Name") L_Name[liste] = reader.Value;
                                    if (balise == "List_Default") L_def[liste] = bool.Parse(reader.Value);
                                    if (balise == "List_ReadOnly") L_ReadOnly[liste] = bool.Parse(reader.Value);
                                }
                                break;
                            }
                        case XmlNodeType.EndElement: // prend n'importe quel end !! 
                            {
                                if (box == "Fonts" && reader.Name == "Fonts")
                                {
                                    resumetoread.Fonts = new Fonts("Polices_cv", Fts_alignment);
                                    for (int i = 1; i <= ft; i++)
                                    {
                                        //pbs !( source a changer)
                                        resumetoread.Fonts.List.Add(new FontElement( Ft_source[i], Ft_size[i], new Color() {A= Ft_color_a[i], R= Ft_color_r[i], G=Ft_color_g[i], B=Ft_color_b[i] }, Ft_italic[i], Ft_bold[i], Ft_underlined[i], Ft_uppercase[i], Ft_name[i]));
                                        Ft_name[i] = "";
                                        Ft_size[i] = 0;
                                        Ft_color_a[i] = 0;
                                        Ft_color_r[i] = 0; Ft_color_g[i] = 0; Ft_color_b[i] = 0;
                                        Ft_italic[i] = false; Ft_bold[i] = false; Ft_underlined[i] = false; Ft_uppercase[i] = false;
                                        Ft_source[i] = "";
                                    }
                                    ft = 0;
                                }
                                if (box == "BackBox" && reader.Name == "BackBox")
                                {
                                    var backbox = new BoxBackground(x, y, z, SizeX, SizeY, angle, Shape)
                                    {
                                        Fill = new Color() { A = Color_A, B = Color_B, R = Color_R, G = Color_G },
                                        Stroke = new Color() { A = Color_Border_A, B = Color_Border_B, R = Color_Border_R, G = Color_Border_G },
                                        StrokeThickness = strokethikness
                                    };
                                    if (Img_name != "")
                                    {
                                        backbox.Image = new DataImage(Img_name, Img_dep);
                                    }
                                    resumetoread.Layout.AddBackBox(backbox);
                                    Shape = BoxBackgroundShape.Rectangle; Img_name = ""; Img_dep = false; strokethikness = 0;
                                    box = ""; x = 0; y = 0; z = 0; SizeX = 0; SizeY = 0; angle = 0; 
                                    Color_A = 0; Color_R = 0; Color_G = 0; Color_B = 0; Color_Border_A = 0; Color_Border_R = 0; Color_Border_G = 0; Color_Border_B = 0;
                                }
                                if (liste > 1 && reader.Name == "ElementList")
                                {
                                    if (nv.Name != "")
                                    {
                                        var souslist = nv;
                                        nv = new ElementList<Element>(L_Name[liste], L_def[liste]);
                                        nv.Add(souslist);
                                        L_Name[liste] = ""; L_def[liste] = false;
                                    }
                                    else nv = new ElementList<Element>(L_Name[liste], L_def[liste]);
                                    dde = Creation_DDE(resumetoread, nv, dde, DD_value, DD_start, DD_end, DD_format, DD_level, DD_description, DD_dependant, DD_def,DD_categorie,ddcatnum, DD_name);
                                    while (dt >= 1) { elem -= 1; dt -= 1; }
                                    elem = Creation_DTS(resumetoread, nv, dts, DS_value, DS_level, DS_description, DS_dependant, DS_def, DS_categorie,dscatnum, DS_name, elem);
                                    dts = 0; box = ""; L_Name[liste] = ""; L_def[liste] = false; L_ReadOnly[liste] = false;
                                    liste -= 1; elem -= 1;
                                    break;
                                }
                                if (liste == 1 && reader.Name == "ElementList")
                                {
                                    var bnv = new BoxText(x, y, z, SizeX, SizeY, angle, L_Name[liste]);
                                    if (nv.Name != "")
                                    {
                                        var souslist = nv;
                                        nv = new ElementList<Element>(L_Name[liste], L_def[liste]); // n'a pas de nom !
                                        nv.Add(souslist);
                                        while (dde >= 1 && elem >= 1) { dde = Creation_DDE(resumetoread, nv, dde, DD_value, DD_start, DD_end, DD_format, DD_level, DD_description, DD_dependant, DD_def, DD_categorie, ddcatnum, DD_name); }
                                        while (dt >= 1 && elem >= 1) { elem -= 1; dt -= 1; }
                                        while (dts >= 1 && elem >= 1)
                                        {
                                            elem = Creation_DTS(resumetoread, nv, dts, DS_value, DS_level, DS_description, DS_dependant, DS_def, DS_categorie,dscatnum, DS_name, elem);
                                            dts = 0;
                                        }
                                        if (ft_text >= 1) ft_text = Creation_BoxFonts(bnv, ft_text, Fts_text_alignment, Ft_text_source, Ft_text_size, Ft_text_color_a, Ft_text_color_r, Ft_text_color_g, Ft_text_color_b, Ft_text_italic, Ft_text_bold, Ft_text_underlined, Ft_text_uppercase, Ft_text_name);
                                        bnv.Element = nv;
                                        L_Name[liste] = ""; L_def[liste] = false;
                                        nv = new ElementList<Element>("");
                                    }
                                    else
                                    {
                                        nv = new ElementList<Element>(L_Name[liste], L_def[liste]);
                                        while (dde >= 1 && elem >= 1)
                                        {
                                            for (int i = 1; i <= dde; i++)
                                            {
                                                nv.Add(new DataDated<string>(DD_value[i], DD_start[i], DD_end[i], DD_format[i], DD_level[i], DD_description[i], DD_dependant[i], DD_def[i]));
                                                DD_value[i] = ""; DD_start[i] = default(DateTime); DD_end[i] = default(DateTime); DD_format[i] = ""; DD_level[i] = 0; DD_description[i] = "";
                                                DD_dependant[i] = false; DD_def[i] = false; DD_name[i] = "";
                                                elem -= 1;
                                            }
                                            dde = 0;
                                        }
                                        while (dt >= 1 && elem >= 1) { dt -= 1; elem -= 1; }
                                        while (dts >= 1 && elem >= 1)
                                        {
                                            for (int i = 1; i <= dts; i++)
                                            {
                                                var datastr =new Data<string>(DS_value[i], DS_level[i], DS_description[i], DS_dependant[i], DS_def[i]);
                                                DS_name[i] = ""; DS_description[i] = ""; DS_value[i] = ""; DS_dependant[i] = false; DS_def[i] = false; DS_level[i] = 0;
                                                for (int j = 1; j <= dscatnum; j++)
                                                {
                                                    datastr.AddCategory(new ElementList<Element>(DS_categorie[i, j]));
                                                    DS_categorie[i,j] = "";
                                                }
                                                nv.Add(datastr);
                                                elem -= 1;
                                            }
                                            dts = 0;
                                        }
                                        if (ft_text >= 1) ft_text = Creation_BoxFonts(bnv, ft_text, Fts_text_alignment, Ft_text_source, Ft_text_size, Ft_text_color_a, Ft_text_color_r, Ft_text_color_g, Ft_text_color_b, Ft_text_italic, Ft_text_bold, Ft_text_underlined, Ft_text_uppercase, Ft_text_name);
                                           
                                        //réinitialisation 
                                        L_Name[liste] = ""; L_def[liste] = false;
                                        L_ReadOnly[liste] = false;
                                        bnv.Element = nv;
                                        nv = new ElementList<Element>("");

                                    }
                                    resumetoread.Layout.AddTextBox(bnv);
                                    x = 0; y = 0; z = 0; SizeX = 0; SizeY = 0; angle = 0;
                                    liste -= 1;
                                    box = "";

                                }
                                if (liste == 0 && reader.Name == "DataString_Element")
                                {
                                    if (dde == 1 && elem == 0)
                                    {
                                        var bnv = new BoxText(x, y, z, SizeX, SizeY, angle, DD_name[dde]);
                                        var nvdd = new DataDated<string>(DD_value[dde], DD_start[dde], DD_end[dde], DD_format[dde], DD_level[dde], DD_description[dde], DD_dependant[dde], DD_def[dde]);
                                        DD_name[dde] = ""; DD_value[dde] = ""; DD_format[dde] = ""; DD_level[dde] = 0; DD_description[dde] = ""; DD_dependant[dde] = false; DD_def[dde] = false; DD_start[dde] = default(DateTime); DD_end[dde] = default(DateTime);
                                        DD_categorie[dde,1] = ""; box = ""; x = 0; y = 0; z = 0; SizeX = 0; SizeY = 0; angle = 0; DD_level[dde] = 0;
                                        dde = 0;
                                    }
                                    if (dt == 1 && elem == 0) { }
                                    if (dts == 1 && elem == 0)
                                    {
                                        var nvdt = new Data<string>(DS_value[dts], DS_level[dts], DS_description[dts], DS_dependant[dts], DS_def[dts]);
                                        var bnv = new BoxText(x, y, z, SizeX, SizeY, angle, DS_name[dts]);
                                        for (int j = 1; j <= dscatnum; j++)
                                        {
                                            nvdt.AddCategory(new ElementList<Element>(DS_categorie[1, j]));
                                            DS_categorie[1, j] = "";
                                        }
                                        
                                        bnv.Element = nvdt;
                                        resumetoread.Layout.AddTextBox(bnv);
                                        DS_name[dts] = ""; DS_description[dts] = ""; DS_value[dts] = ""; DS_dependant[dts] = false; DS_def[dts] = false; DS_level[dts] = 0; DS_level[dts] = 0;
                                        DS_categorie[dts,1] = ""; box = ""; x = 0; y = 0; z = 0; SizeX = 0; SizeY = 0; angle = 0;
                                        dts = 0;
                                    }
                                }
                                break;
                            }
                    }
                }
            }

            return resumetoread;
        }

        private static int Creation_BoxFonts(BoxText bnv, int ft_text, TextAlignment Fts_text_alignment, string[] Ft_text_source, double[] Ft_text_size, byte[] Ft_text_color_a, byte[] Ft_text_color_r, byte[] Ft_text_color_g, byte[] Ft_text_color_b, bool[] Ft_text_italic, bool[] Ft_text_bold, bool[] Ft_text_underlined, bool[] Ft_text_uppercase, string[] Ft_text_name)
        {
            bnv.Fonts = new Fonts("Polices_boite", Fts_text_alignment);
            for (int i = 1; i <= ft_text; i++)
            {
                bnv.Fonts.List.Add(new FontElement(Ft_text_source[i], Ft_text_size[i], new Color() { A = Ft_text_color_a[i], R = Ft_text_color_r[i], G = Ft_text_color_g[i], B = Ft_text_color_b[i] }, Ft_text_italic[i], Ft_text_bold[i], Ft_text_underlined[i], Ft_text_uppercase[i], Ft_text_name[i]));
                Ft_text_name[i] = "";
                Ft_text_size[i] = 0;
                Ft_text_color_a[i] = 0;
                Ft_text_color_r[i] = 0; Ft_text_color_g[i] = 0; Ft_text_color_b[i] = 0;
                Ft_text_italic[i] = false; Ft_text_bold[i] = false; Ft_text_underlined[i] = false; Ft_text_uppercase[i] = false;
                Ft_text_source[i] = "";
            }
             return 0;
        }

        private static int Creation_DTS(Resume resumetoread, ElementList<Element> nv, int dts, string[] DS_value, double[] DS_level, string[] DS_description, bool[] DS_dependant, bool[] DS_def, string[,] DS_categorie,int dscatnum, string[] DS_name, int elem)
        {
            for (int i = 1; i <= dts; i++)
            {
                var datastr =new Data<string>(DS_value[i], DS_level[i], DS_description[i], DS_dependant[i], DS_def[i]);
                DS_name[i] = ""; DS_description[i] = ""; DS_value[i] = ""; DS_dependant[i] = false; DS_def[i] = false; DS_level[i] = 0;
                
               for (int j = 1; j <= dscatnum; j++)
                {
                    datastr.AddCategory(new ElementList<Element>(DS_categorie[i, j]));
                    DS_categorie[i, j] = "";
                }
                nv.Add(datastr);
                elem -= 1;
            }
            return elem;
        }

        private static int Creation_DDE(Resume resumetoread, ElementList<Element> nv, int dde, string[] DD_value, DateTime[] DD_start, DateTime[] DD_end, string[] DD_format, double[] DD_level, string[] DD_description, bool[] DD_dependant, bool[] DD_def, string[,] DD_categorie,int ddcatnum, string[] DD_name)
        {
            for (int i = 1; i <= dde; i++)
            {
                var datadd =new DataDated<string>(DD_value[i], DD_start[i], DD_end[i], DD_format[i], DD_level[i], DD_description[i], DD_dependant[i], DD_def[i]);
                DD_value[i] = ""; DD_start[i] = default(DateTime); DD_end[i] = default(DateTime); DD_format[i] = ""; DD_level[i] = 0; DD_description[i] = "";
                DD_dependant[i] = false; DD_def[i] = false; DD_name[i] = "";
                for (int j = 1; j <= ddcatnum; j++)
                {
                    datadd.AddCategory(new ElementList<Element>(DD_categorie[i, j]));
                    DD_categorie[i, j] = "";
                }
                nv.Add(datadd);
            }
            return 0;
        }
    }
}

