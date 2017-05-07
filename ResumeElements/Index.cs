﻿using System;
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
            DataIndex.Add(d);
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
        public static ElementList<Data> GetMiscellaneous()
        {
            var misc = new ElementList<Data>("Divers");
            foreach(Data d in DataIndex.Values)
            {
                if (d.Categories.Count == 1 && d.Categories[0] == DataIndex)
                {
                    misc.Add(d);
                }
            }
            return misc;
        }

        // Child : update to mainIndex ?

        /// <summary>
        /// Root defines the topmost ElementList, mother of all Elements that can be put in a resume
        /// </summary>
        public static ElementList<ElementList> Root { get; set; } = new ElementList<ElementList>("root")
        {
            new ElementList<Element>("Coordonnées")
            {
                new Data<string>("Nom",""),
                new Data<string>("Téléphone",""),
                new Data<string>("Mél",""),
		new Data<string>("Adresse",""),

                //...
            },
            new ElementList<Element>("Compétences"),
            new ElementList<Element>("Langues"),
            new ElementList<Element>("Diplômes"),
            // Remplir
        };
    }
}
