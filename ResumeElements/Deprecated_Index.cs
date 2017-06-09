using System;
using Windows.Storage;

namespace ResumeElements
{
    /// <summary>
    /// The index lists all piece of Data that can be put in a resume and it contains the root of the whole hierarchy of those/this data
    /// </summary>
    public static class Deprecated_Index
    {
        public static Deprecated_ElementList<Deprecated_Data> DataIndex { get; } = new Deprecated_ElementList<Deprecated_Data>("Index");
        public static Deprecated_ElementList<Deprecated_DataImage> Images { get; } = new Deprecated_ElementList<Deprecated_DataImage>("Images");

        public static async void ReloadImagesAsync()
        {
            Deprecated_Index.Images.Clear();

            var imgFolds = await FileManagement.Images.GetFoldersListAsync();
            foreach (StorageFolder imgFold in imgFolds)
            {
                var imgs = await imgFold.GetFilesAsync(Windows.Storage.Search.CommonFileQuery.OrderByName);
                foreach (StorageFile f in imgs)
                {
                    var temp = FileManagement.Images.GetNameWithoutExtension(f.Name);
                    if (!Deprecated_Index.Images.ContainsKey(temp))
                        new Deprecated_DataImage(temp);
                }
            }
        }

        public static void AddData(Deprecated_Data d)
        {
            DataIndex.Add(d, false);
        }
        public static void RemoveData(Deprecated_Data d)
        {
            if (DataIndex.Contains(d))
            {
                d.ClearCategories();
                DataIndex.Remove(d);
            }
        }
        public static void RemoveData(string name)
        {
            if (DataIndex.Find(name) is Deprecated_Data t)
            {
                RemoveData(t);
            }
        }
        public static void AddImage(Deprecated_DataImage d)
        {
            Images.Add(d, false);
        }
        public static void RemoveImage(Deprecated_DataImage d)
        {
            if (Images.Contains(d))
            {
                d.ClearCategories();
                Images.Remove(d);
            }
        }
        public static void RemoveImage(string name)
        {
            if (Images.Find(name) is Deprecated_DataImage t)
            {
                RemoveImage(t);
            }
        }

        public static Element Find(string name)
        {
            var temp = DataIndex.Find(name);
            if (temp != null) return temp;
            temp = Images.Find(name);
            if (temp != null) return temp;
            return Root.Find(name);
        }

        /// <summary>
        /// Return a list of all pieces of Data unlisted in any categories other than the index
        /// </summary>
        /// <returns></returns>
        public static Deprecated_ElementList<Deprecated_Data> GetUnlistedData()
        {
            var misc = new Deprecated_ElementList<Deprecated_Data>("Non listés");
            foreach (Deprecated_Data d in DataIndex.Values)
            {
                if (d.Categories.Count == 0)
                {
                    misc.Add(d, false);
                }
            }
            return misc;
        }

        /// <summary>
        /// Root defines the topmost ElementList, mother of all Elements that can be put in a resume
        /// </summary>
        public static Deprecated_ElementList<Deprecated_ElementList> Root { get; set; } = new Deprecated_ElementList<Deprecated_ElementList>("root")
        {
            new Deprecated_ElementList<Element>("Coordonnées")
            {
                new Deprecated_Data<string>("Nom", ""),
                new Deprecated_Data<string>("Téléphone", ""),
                new Deprecated_Data<string>("Mél", ""),
                new Deprecated_Data<string>("Adresse", ""),
                new Deprecated_Data<string>("Profession", "")

                //...
            },
            new Deprecated_ElementList<Element>("Compétences"),
            new Deprecated_ElementList<Element>("Langues")
            {
                new Deprecated_Data<string>("Langue 1",""),
                new Deprecated_Data<string>("Langue 2",""),
                new Deprecated_Data<string>("Langue 3",""),
                new Deprecated_Data<string>("Langue 4","")
            },
            new Deprecated_ElementList<Element>("Diplômes"),
            new Deprecated_ElementList<Element>("Études"),
            new Deprecated_ElementList<Element>("Expériences professionnelles"),
            new Deprecated_ElementList<Element>("Centres d'intérêts"),
            new Deprecated_ElementList<Element>("Divers"),
            // Remplir
        };

        public static Deprecated_ElementList FindParent(Element e)
        {
            Element prev = null, cur = Root;

            while (cur != e)
            {
                prev = cur;
                if (prev == null)
                    break;

                if (prev is Deprecated_ElementList i)
                {
                    foreach (Element el in i.Values)
                    {
                        cur = el.Find(e.Name);
                        if (cur != null)
                        {
                            cur = el;
                            break;
                        }
                    }
                }
            }

            return prev as Deprecated_ElementList;
        }

        public static void Erase(Deprecated_ElementList e)
        {
            e.Clear();
            FindParent(e).Remove(e);
        }
        public static void Erase(Deprecated_Data d)
        {
            d.ClearCategories();
            (Deprecated_Index.Find("Divers") as Deprecated_ElementList).Remove(d);
            DataIndex.Remove(d);
        }

        /// <summary>
        /// Generate a valid, unused, name for an Element.
        /// </summary>
        /// <param name="baseValue">If given, the name will include this string.
        /// If the Index doesn't contain baseValue, baseValue is returned</param>
        /// <returns></returns>
        public static string GetUnusedName(object baseValue = null)
        {
            string res = baseValue as string;
            if (res == null || res == "")
            {
                for (int i = 0; i >= 0 && Deprecated_Index.Find(res) != null; i++)
                    res = i.ToString();
            }
            else if (Deprecated_Index.Find(res) != null)
            {
                while (Deprecated_Index.Find(res) != null)
                {
                    res = "_" + res;
                }
            }

            return res;
        }
    }
}
