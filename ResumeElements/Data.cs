using System;
using System.Collections.Generic;

namespace ResumeElements
{
    public abstract class Data: Element
    {
        public Data(string name, string description = "", double level = -1, bool isDefault = false) : base(name, isDefault)
        {
            Description = description;
            double Level = level;
            categories = new List<ElementList<Element>>();
        }

        public string Description { get; set; }

        protected List<ElementList<Element>> categories;
        /// <summary>
        /// Lists all the categories this element is listed in. The accessor will return a copy of it to prevent unwanted modifications.
        /// </summary>
        public List<ElementList<Element>> Categories
        {
            get
            {
                return new List<ElementList<Element>>(categories);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        public void AddCategory(ElementList<Element> cat)
        {
            categories.Remove(cat);
            cat.Add(this);
            // Add in the category too
            throw new NotImplementedException();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cat"></param>
        public void RemoveCategory(ElementList<Element> cat)
        {
            // Remove from the category too
            categories.Remove(cat);
            cat.Remove(this);
            throw new NotImplementedException();
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
    /// <typeparam name="T">Can be a string, or an image </typeparam>
    public class Data<T> : Data
    {
        public Data(string name, T value, string description = "", double level = -1, bool isDefault = false) : base(name, description, level, isDefault)
        {
            Value = value;
        }

        /// <summary>
        /// Actual information (Text, image,time,number)
        /// </summary>
        public T Value { get; set; }

    }
}
