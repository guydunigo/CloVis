using System;
using System.IO;
using Windows.Storage;

namespace ResumeElements
{
    public class Index
    {
        public NonGenericElementList DataIndex { get; }
        public NonGenericElementList Images { get; }

        /// <summary>
        /// Root defines the topmost ElementList, mother of all Elements that can be put in a resume
        /// </summary>
        public NonGenericElementList Root { get; set; }

        public Index()
        {
            DataIndex = new NonGenericElementList("DataIndex");
            Images = new NonGenericElementList("Images");

            Root = new NonGenericElementList("root")
            {
                new NonGenericElementList("Coordonnées")
                {
                    new DataText("Nom", ""),
                    new DataText("Téléphone", ""),
                    new DataText("Mél", ""),
                    new DataText("Adresse", ""),
                    new DataText("Profession", "")
                },
                new NonGenericElementList("Compétences"),
                new NonGenericElementList("Langues")
                {
                    new DataText("Langue 1",""),
                    new DataText("Langue 2",""),
                    new DataText("Langue 3",""),
                    new DataText("Langue 4","")
                },
                new NonGenericElementList("Diplômes"),
                new NonGenericElementList("Études"),
                new NonGenericElementList("Expériences professionnelles"),
                new NonGenericElementList("Centres d'intérêts"),
                new NonGenericElementList("Divers"),
            };
        }

        public async void ReloadImagesAsync()
        {
            Images.Clear();

            var imgFolds = await FileManagment.Images.GetFoldersListAsync();
            foreach (StorageFolder f in imgFolds)
            {
                var temp = FileManagment.Images.GetNameWithoutExtension(f.Name);
                if (!Images.ContainsKey(temp))
                    new DataImage(temp);
            }
        }

        protected void AddDataTo(DataText d, NonGenericElementList list)
        {
            list.Add(d, false);
        }
        protected void RemoveDataFrom(DataText d, NonGenericElementList list)
        {
            if (list.Contains(d))
            {
                d.ClearCategories();
                list.Remove(d);
            }
        }
        protected void RemoveDataFrom(string name, NonGenericElementList list)
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

        public NonGenericElementList FindParent(Element e)
        {
            Element prev = null, cur = Root;

            while (cur != e)
            {
                prev = cur;
                if (prev == null)
                    break;

                if (prev is NonGenericElementList i)
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

            return prev as NonGenericElementList;
        }

        /// <summary>
        /// Return a list of all pieces of Data unlisted in any categories other than the index
        /// </summary>
        /// <returns></returns>
        public NonGenericElementList GetUnlistedData()
        {
            var misc = new NonGenericElementList("Non listés");
            foreach (DataText d in DataIndex.Values)
            {
                if (d.Categories.Count == 0)
                {
                    misc.Add(d, false);
                }
            }
            return misc;
        }

        public void Erase(NonGenericElementList e)
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
