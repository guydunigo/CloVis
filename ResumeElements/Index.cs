using System;
using System.IO;
using Windows.Storage;

namespace ResumeElements
{
    public class Index
    {
        public ElementList DataIndex { get; }
        public ElementList Images { get; }

        /// <summary>
        /// Root defines the topmost ElementList, mother of all Elements that can be put in a resume
        /// </summary>
        public ElementList Root { get; set; }

        public Index()
        {
            DataIndex = new ElementList("DataIndex");
            Images = new ElementList("Images");

            Root = new ElementList("root")
            {
                new ElementList("Coordonnées")
                {
                    new DataText("Nom", ""),
                    new DataText("Téléphone", ""),
                    new DataText("Mél", ""),
                    new DataText("Adresse", ""),
                    new DataText("Profession", "")
                },
                new ElementList("Compétences"),
                new ElementList("Langues")
                {
                    new DataText("Langue 1",""),
                    new DataText("Langue 2",""),
                    new DataText("Langue 3",""),
                    new DataText("Langue 4","")
                },
                new ElementList("Diplômes"),
                new ElementList("Études"),
                new ElementList("Expériences professionnelles"),
                new ElementList("Centres d'intérêts"),
                new ElementList("Divers"),
            };
        }

        public async void ReloadImagesAsync()
        {
            Images.Clear();

            var imgFolds = await FileManagement.Images.GetFoldersListAsync();
            foreach (StorageFolder f in imgFolds)
            {
                var temp = FileManagement.Images.GetNameWithoutExtension(f.Name);
                if (!Images.ContainsKey(temp))
                    new DataImage(temp);
            }
        }

        protected void AddDataTo(DataText d, ElementList list)
        {
            list.Add(d, false);
        }
        protected void RemoveDataFrom(DataText d, ElementList list)
        {
            if (list.Contains(d))
            {
                d.ClearCategories();
                list.Remove(d);
            }
        }
        protected void RemoveDataFrom(string name, ElementList list)
        {
            if (DataIndex.Find(name) is DataText t)
            {
                RemoveDataFrom(t, list);
            }
        }

        public void AddData(DataText d)
        {
            AddDataTo(d, DataIndex);
        }
        public void RemoveData(DataText d)
        {
            RemoveDataFrom(d, DataIndex);
        }
        public void RemoveData(string name)
        {
            RemoveDataFrom(name, DataIndex);
        }

        public void AddImage(DataImage d)
        {
            AddDataTo(d, Images);
        }
        public void RemoveImage(DataImage d)
        {
            RemoveDataFrom(d, Images);
        }
        public void RemoveImage(string name)
        {
            RemoveDataFrom(name, Images);
        }

        public Element Find(string name)
        {
            var temp = DataIndex.Find(name);
            if (temp != null)
                return temp;

            return Root.Find(name);
        }
        public Element FindImage(string name)
        {
            return Images.Find(name);
        }

        public ElementList FindParent(Element e)
        {
            Element prev = null, cur = Root;

            while (cur != e)
            {
                prev = cur;
                if (prev == null)
                    break;

                if (prev is ElementList i)
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

            return prev as ElementList;
        }

        /// <summary>
        /// Return a list of all pieces of Data unlisted in any categories other than the index
        /// </summary>
        /// <returns></returns>
        public ElementList GetUnlistedData()
        {
            var misc = new ElementList("Non listés");
            foreach (DataText d in DataIndex.Values)
            {
                if (d.Categories.Count == 0)
                {
                    misc.Add(d, false);
                }
            }
            return misc;
        }

        public void Erase(ElementList e)
        {
            e.Clear();
            FindParent(e)?.Remove(e);
        }
        public void Erase(DataText d)
        {
            d.ClearCategories();
            (Find("Divers") as Deprecated_ElementList).Remove(d);
            DataIndex.Remove(d);
        }

        /// <summary>
        /// Generate a valid, unused, name for an Element.
        /// </summary>
        /// <param name="baseValue">If given, the name will include this string.
        /// If the Index doesn't contain baseValue, baseValue is returned</param>
        /// <returns></returns>
        public string GetUnusedName(string baseValue = "")
        {
            if (baseValue == "")
            {
                for (int i = 0; Find(baseValue) != null; i++)
                    baseValue = i.ToString();
            }
            else if (Find(baseValue) != null)
            {
                while (Find(baseValue) != null)
                {
                    baseValue = "_" + baseValue;
                }
            }

            return baseValue;
        }
    }
}
