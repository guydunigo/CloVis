using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    /// <summary>
    /// The index lists all piece of Data that can be put in a resume and it contains the root of the whole hierarchy of those/this data
    /// </summary>
    public static class Index
    {
        public static ElementList<Data> DataIndex { get; } = new ElementList<Data>("Index");
        
        public static void AddData(Data d)
        {
            DataIndex.Add(d, false);
        }
        public static void RemoveData(Data d)
        {
            if (DataIndex.Contains(d))
            {
                DataIndex.Remove(d);
                d.ClearCategories();
            }
        }
        public static void RemoveData(string name)
        {
            if (DataIndex.Find(name) is Data t)
            {
                RemoveData(t);
            }
        }

        public static Element Find(string name)
        {
            var temp = DataIndex.Find(name);
            if (temp != null) return temp;
            return Root.Find(name);
        }

        /// <summary>
        /// Return a list of all pieces of Data unlisted in any categories other than the index
        /// </summary>
        /// <returns></returns>
        public static ElementList<Data> GetUnlistedData()
        {
            var misc = new ElementList<Data>("Non listés");
            foreach(Data d in DataIndex.Values)
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
        public static ElementList<ElementList> Root { get; set; } = new ElementList<ElementList>("root")
        {
            new ElementList<Element>("Coordonnées")
            {
                new Data<string>("Nom", ""),
                new Data<string>("Téléphone", ""),
                new Data<string>("Mél", ""),
                new Data<string>("Adresse", ""),

                //...
            },
            new ElementList<Element>("Compétences"),
            new ElementList<Element>("Langues"),
            new ElementList<Element>("Diplômes"),
            new ElementList<Element>("Études"),
            new ElementList<Element>("Expériences professionnelles"),
            new ElementList<Element>("Centres d'intérêts"),
            new ElementList<Element>("Divers"),
            // Remplir
        };

        public static ElementList FindParent(Element e)
        {
            Element prev = null, cur = Root;
            
            while(cur != e)
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

        public static void Erase(ElementList e)
        {
            e.Clear();
            FindParent(e).Remove(e);
        }
        public static void Erase(Data d)
        {
            d.ClearCategories();
            (Index.Find("Divers") as ElementList).Remove(d);
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
                for (int i = 0; i >= 0 && Index.Find(res) != null; i++)
                    res = i.ToString();
            }
            else if (Index.Find(res) != null)
            {
                while (Index.Find(res) != null)
                {
                    res = "_" + res;
                }
            }

            return res;
        }
    }
}
