using System.Collections.Generic;
using System.ComponentModel;

namespace ResumeElements
{
    internal class NonGenericElementListComparer : IComparer<NonGenericElementList>
    {
        public int Compare(NonGenericElementList x, NonGenericElementList y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }

    public class DataText : Element, INotifyPropertyChanged
    {
        public DataText(NewIndex index, string value, double level = -1, bool isDefault = true) : this(index.GetUnusedName(value), value, level, index, isDefault)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="level"></param>
        /// <param name="index">If index is null, the DataText is considered independant.</param>
        /// <param name="isDefault"></param>
        public DataText(string name, string value, double level = -1, NewIndex index = null, bool isDefault = true) : base(name, isDefault)
        {
            Value = value;

            Level = level;
            categories = new SortedSet<NonGenericElementList>(new NonGenericElementListComparer());

            Index = index;
            if (index != null)
                index.AddData(this);
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
        
        public NewIndex Index { get; }

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
                if (Index != null && categories.Count >= 2)
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
                if (Index != null && categories.Count == 0 && cat.Name != "Divers" && Index.Find("Divers") is NonGenericElementList misc)
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
        /// The return Element is independant and isn't registered in the Index
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            return new DataText(Name, Value, Level, null, IsDefault);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index">If the index param is defined, use this one instead of the private property.</param>
        public override void UpdateFromIndex(NewIndex indexToUse = null)
        {
            if (indexToUse == null)
                indexToUse = Index;

            if (indexToUse != null)
            {
                if (indexToUse.Find(Name) is DataText d)
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
}
