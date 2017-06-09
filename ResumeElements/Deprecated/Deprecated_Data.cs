using System.Collections.Generic;
using System.ComponentModel;

namespace ResumeElements
{
    internal class Deprecated_CategoriesComparer : IComparer<Deprecated_ElementList>
    {
        public int Compare(Deprecated_ElementList x, Deprecated_ElementList y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }

    public abstract class Deprecated_Data : Element, INotifyPropertyChanged
    {
        public Deprecated_Data(string name, double level = -1, string description = "", bool isIndependant = false, bool isDefault = true) : base(name, isDefault)//appel au constructeur de la classe mere (element)
        {
            Description = description;
            Level = level;
            categories = new SortedSet<Deprecated_ElementList>(new Deprecated_CategoriesComparer());

            this.IsIndependant = isIndependant;
            if (!isIndependant) Deprecated_Index.AddData(this);
        }

        protected string description;
        public string Description
        {
            get => description;
            set
            {
                description = value;
                NotifyPropertyChanged("Description");
            }
        }

        public override Element Find(string name)
        {
            if (Name == name) return this;
            else return null;
        }

        public bool IsIndependant { get; set; }

        protected SortedSet<Deprecated_ElementList> categories;
        /// <summary>
        /// Lists all the categories this element is listed in. The accessor will return a copy of it to prevent unwanted modifications.
        /// </summary>
        public SortedSet<Deprecated_ElementList> Categories
        {
            get
            {
                return new SortedSet<Deprecated_ElementList>(categories, new Deprecated_CategoriesComparer());
            }
        }
        public void AddCategory(Deprecated_ElementList cat)
        {
            if (!categories.Contains(cat))
            {
                if (!cat.ContainsKey(Name))
                    cat.Add(this);

                // The misc. category isn't to be listed in Categories
                if (cat.Name != "Divers")
                    categories.Add(cat);

                if (!IsIndependant && categories.Count == 2)
                    (Deprecated_Index.Find("Divers") as Deprecated_ElementList).Remove(this);

                NotifyPropertyChanged("Categories");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        public void RemoveCategory(Deprecated_ElementList cat)
        {
            if (categories.Contains(cat))
            {
                if (cat.ContainsKey(Name))
                    cat.Remove(this);
                categories.Remove(cat);

                if (!IsIndependant && categories.Count == 0 && cat.Name != "Divers" && Deprecated_Index.Find("Divers") is Deprecated_ElementList misc)
                    AddCategory(misc);

                NotifyPropertyChanged("Categories");
            }
        }

        public void ClearCategories()
        {
            var copy = new List<Deprecated_ElementList>(categories);
            foreach (Deprecated_ElementList e in copy)
            {
                e.Remove(this);
            }
            categories.Clear();
            NotifyPropertyChanged("Categories");
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
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Can be a string, or an image (ie. Data<string> or Data<int>)</typeparam>
    public class Deprecated_Data<T> : Deprecated_Data, INotifyPropertyChanged
    {
        public Deprecated_Data(T value, double level = -1, string description = "", bool isIndependant = false, bool isDefault = true) : this(Deprecated_Index.GetUnusedName(value), value, level, description, isIndependant, isDefault)
        {
        }

        public Deprecated_Data(string name, T value, double level = -1, string description = "", bool isIndependant = false, bool isDefault = true) : base(name, level, description, isIndependant, isDefault)
        {
            Value = value;
        }

        protected T value;
        /// <summary>
        /// Actual information (Text, image,time,number)
        /// </summary>
        public T Value
        {
            get => value;
            set
            {
                this.value = value;
                NotifyPropertyChanged("Value");
            }
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            var temp = new Deprecated_Data<T>(Name, Value, Level, Description, true, IsDefault);
            foreach (Deprecated_ElementList el in categories)
            {
                temp.AddCategory(el);
            }

            return temp;
        }

        public override void UpdateFromIndex(Index indexToUse = null)
        {
            // + in daughters
            if (Deprecated_Index.Find(Name) is Deprecated_Data<T> d)
            {
                Level = d.Level;
                Value = d.Value;
                Description = d.Description;
                IsDefault = d.IsDefault;
            }
            //else if (temp == null)
            //    throw new MissingFieldException("The element can't be found in the Index and can't be updated.");
            //else
            //    throw new InvalidCastException("The type of the Element in the Index does not match this one and therefore, it can't be updated.");
        }
    }
}
