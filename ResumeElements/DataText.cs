﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    internal class NonGenericElementListComparer : IComparer<NonGenericElementList>
    {
        public int Compare(NonGenericElementList x, NonGenericElementList y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }

    class DataText : Element, INotifyPropertyChanged
    {
        public DataText(string value, double level = -1, bool isIndependant = false, bool isDefault = true) : this(Index.GetUnusedName(value), value, level, isIndependant, isDefault)
        {
        }

        public DataText(string name, string value, double level = -1, bool isIndependant = false, bool isDefault = true) : base(name, isDefault)
        {
            Value = value;

            Level = level;
            categories = new SortedSet<NonGenericElementList>(new NonGenericElementListComparer());

            IsIndependant = isIndependant;
            //if (!isIndependant)
            //    Index.AddData(this);
            throw new NotImplementedException();
        }

        protected string value;
        public string Value
        {
            get => value;
            set
            {
                Value = value;
                NotifyPropertyChanged("Value");
            }
        }

        protected bool isIndependant;
        public bool IsIndependant
        {
            get => isIndependant;
            set
            {
                isIndependant = value;
                NotifyPropertyChanged("IsIndependant");
            }
        }

        protected double level;
        /// <summary>
        /// Level value between 0 and 5
        /// If it is undefined, Level is -1
        /// </summary>
        public double Level
        {
            get
            {
                return level;
            }
            set
            {
                if (value == -1 || (value >= 0 && value <= 5))
                    level = value;
                else if (value > 5)
                    level = 5;
                else if (value < 0)
                    level = 0;
                NotifyPropertyChanged("Level");
            }
        }

        protected SortedSet<NonGenericElementList> categories;
        /// <summary>
        /// Lists all the categories this element is listed in. The accessor will return a copy of it to prevent unwanted modifications.
        /// </summary>
        public SortedSet<NonGenericElementList> Categories
        {
            get
            {
                return new SortedSet<NonGenericElementList>(categories, new NonGenericElementListComparer());
            }
        }
        public void AddCategory(NonGenericElementList cat, bool AddIntoCatAsWell = true)
        {
            if (!categories.Contains(cat))
            {
                // The misc. category isn't to be listed in Categories
                if (cat.Name != "Divers")
                    categories.Add(cat);

                if (AddIntoCatAsWell && !cat.ContainsKey(Name))
                    cat.Add(this);

                // Remove the misc cat if the element is listed elsewhere and is dependant
                if (!IsIndependant && categories.Count >= 2)
                    (Index.Find("Divers") as NonGenericElementList)?.Remove(this);

                NotifyPropertyChanged("Categories");
            }
        }
        public void RemoveCategory(NonGenericElementList cat, bool RemoveFromCatAsWell = true)
        {
            if (categories.Contains(cat))
            {
                categories.Remove(cat);

                if (RemoveFromCatAsWell && cat.ContainsKey(Name))
                    cat.Remove(this);

                // Add the misc cat if the element isn't listed anywhere else and is dependant
                if (!IsIndependant && categories.Count == 0 && cat.Name != "Divers" && Index.Find("Divers") is NonGenericElementList misc)
                    AddCategory(misc);

                NotifyPropertyChanged("Categories");
            }
        }
        public void ClearCategories()
        {
            var copy = new List<NonGenericElementList>(categories);
            foreach (NonGenericElementList e in copy)
            {
                e.Remove(this);
            }
            categories.Clear();
            NotifyPropertyChanged("Categories");
        }

        public override Element Find(string name)
        {
            if (Name == name)
                return this;
            else
                return null;
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// The return Element is Independant and isn't registered in the Index
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            var temp = new DataText(Name, Value, Level, true, IsDefault);
            foreach (NonGenericElementList el in categories)
            {
                temp.AddCategory(el);
            }

            return temp;
        }

        public override void UpdateFromIndex()
        {
            // + in daughters
            var temp = Index.Find(Name);
            if (temp is DataText d)
            {
                Level = d.Level;
                Value = d.Value;
                IsDefault = d.IsDefault;
            }
            //else if (temp == null)
            //    throw new MissingFieldException("The element can't be found in the Index and can't be updated.");
            //else
            //    throw new InvalidCastException("The type of the Element in the Index does not match this one and therefore, it can't be updated.");
        }
    }
}