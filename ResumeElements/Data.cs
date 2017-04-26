using System;
using System.Collections.Generic;

namespace ResumeElements
{
    public abstract class Data: Element
    {
		public Data(string name, double level = -1, string description = "", bool isIndependant = false, bool isDefault = true) : base(name, isDefault)//appel au constructeur de la classe mere (element)
        {
            Description = description;
            double Level = level;
            categories = new List<ElementList>();
	        if (!isIndependant) Index.AddData(this);
        }

        public string Description { get; set; }

        public override Element Find(string name)
        {
            if (Name == name) return this;
            else return null;
        }

        protected List<ElementList> categories;
        /// <summary>
        /// Lists all the categories this element is listed in. The accessor will return a copy of it to prevent unwanted modifications.
        /// </summary>
        public List<ElementList> Categories
        {
            get
            {
                return new List<ElementList>(categories);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        public void AddCategory(ElementList cat)
        {
            if (!categories.Contains(cat))
            {
                cat.Add(this);
                categories.Add(cat);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        public void RemoveCategory(ElementList cat)
        {
            if (categories.Contains(cat))
            {
                cat.Remove(this);
                categories.Remove(cat);
            }
        }
        
        /// <summary>
        /// Method used by ElementList.AddToElement() (Not a big fan :/ )
        /// </summary>
        /// <param name="cat"></param>
        internal void AddToCategory(ElementList cat)
        {
            if (!categories.Contains(cat))
                categories.Add(cat);
        }
        /// <summary>
        /// Method used by ElementList.RemoveFromElement() (Not a big fan :/ )
        /// </summary>
        /// <param name="cat"></param>
        internal void RemoveFromCategory(ElementList cat)
        {
            if (categories.Contains(cat))
                categories.Remove(cat);
        }

        public void ClearCategories()
        {
            foreach(ElementList e in categories)
            {
                RemoveCategory(e);
            }
        }

        protected double level;
        /// <summary>
        /// Level value between 0 and 5
        /// If it is undefined, Level is -1
        /// </summary>
        public double Level {
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
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Can be a string, or an image (ie. Data<string> or Data<int>)</typeparam>
    public class Data<T> : Data
    {
        public Data(string name, T value, double level = -1, string description = "", bool isIndependant = false, bool isDefault = true) : base(name, level, description, isIndependant, isDefault)
        {
            Value = value;
        }

        /// <summary>
        /// Actual information (Text, image,time,number)
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            return new Data<T>(Name, Value, Level, Description, true, true);
        }

        public override void UpdateFromIndex()
        {
            // + in daughters
            var temp = Index.Find(Name);
            if (temp is Data<T> d)
            {
                Level = d.Level;
                Value = d.Value;
                Description = d.Description;
            }
            //else if (temp == null)
            //    throw new MissingFieldException("The element can't be found in the Index and can't be updated.");
            //else
            //    throw new InvalidCastException("The type of the Element in the Index does not match this one and therefore, it can't be updated.");
        }
    }
}
