using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace ResumeElements
{
    public class NonGenericElementList : Element, ICollection<Element>, IEnumerable<Element>, IDictionary<string, Element>, INotifyPropertyChanged
    {
        public NonGenericElementList(string name, bool isDefault = true) : base(name, isDefault)
        {
            elements = new Dictionary<string, Element>();
        }

        protected Dictionary<string, Element> elements;

        public NonGenericElementList SubLists
        {
            get
            {
                NonGenericElementList res = new NonGenericElementList("Root");

                NonGenericElementList temp = null;

                foreach (Element e in Values)
                {
                    if (e is NonGenericElementList el)
                    {
                        res.Add(el);
                        temp = el.SubLists;
                        foreach (NonGenericElementList ee in temp.Values)
                        {
                            res.Add(ee);
                        }
                    }
                }

                return res;
            }
        }

        public void NotifyListChanged()
        {
            NotifyPropertyChanged("Values");
            NotifyPropertyChanged("Keys");
            NotifyPropertyChanged("SubLists");
            NotifyPropertyChanged("Count");
        }

        public int Count => elements.Count;

        public bool IsReadOnly => false;

        public ICollection<string> Keys => ((IDictionary<string, Element>)elements).Keys;

        public ICollection<Element> Values => ((IDictionary<string, Element>)elements).Values;

        public Element this[string key]
        {
            get => ((IDictionary<string, Element>)elements)[key];
            set
            {
                // If the given value's name matches the key
                if (value.Name == key)
                {
                    // Removing previous Element before (because of Data.Categories list)
                    Remove(key);
                    Add(key, value);
                    //NotifyListChanged();
                    //throw new NotImplementedException("NotifyListChanged called thrice :/ (do it manually)");
                }
                else
                    throw new ArgumentException("The new value hasn't a name corresponding to the key"); // Or rename it ?
            }
        }

        public void Add(Element item, bool AddToCategoriesAsWell)
        {
            // Preventing self-containing lists
            if (item.Find(Name) != null)
            {
                throw new ArgumentException("A list can't contain itself or another list containing it.");
            }
            else if (!ContainsKey(item.Name))
            {
                elements.Add(item.Name, item);

                if (AddToCategoriesAsWell && item is DataText dt && !(dt.Categories.Contains(this)))
                    dt.AddCategory(this, false);

                NotifyListChanged();
            }
        }
        public void Add(Element item)
        {
            Add(item, true);
        }
        public void Add(string key, Element value)
        {
            if (key == null)
                throw new ArgumentNullException("Key is null");
            else if (elements.ContainsKey(key))
                throw new ArgumentException("An Element with the given key already exists in the dictionary");
            else if (value.Name != key)
                throw new ArgumentException("The Elements name must be the same as the key");
            else
                Add(value);
        }
        public void Add(KeyValuePair<string, Element> item)
        {
            Add(item.Key, item.Value);
        }

        public bool Remove(string key, bool RemoveFromCategoriesAsWell)
        {
            if (ContainsKey(key))
            {
                var temp = elements.Remove(key);
                if (RemoveFromCategoriesAsWell && this[key] is DataText dt && dt.Categories.Contains(this))
                    dt.RemoveCategory(this, false);
                NotifyListChanged();

                return temp;
            }
            else
                return false;
        }
        public bool Remove(string key)
        {
            return Remove(key, true);
        }
        public bool Remove(Element item)
        {
            return Remove(item.Name);
        }
        public bool Remove(KeyValuePair<string, Element> item)
        {
            return Remove(item.Key);
        }

        public void Clear()
        {
            var copy = new List<Element>(elements.Values);
            foreach (Element e in copy)
            {
                if (e is DataText dt)
                    dt.RemoveCategory(this, false);
            }
            elements.Clear();
            NotifyListChanged();
        }

        public bool ContainsKey(string key)
        {
            return ((IDictionary<string, Element>)elements).ContainsKey(key);
        }
        public bool Contains(Element item)
        {
            return elements.ContainsValue(item);
        }
        public bool Contains(KeyValuePair<string, Element> item)
        {
            return ((IDictionary<string, Element>)elements).Contains(item);
        }

        public void CopyTo(Element[] array, int arrayIndex)
        {
            if (array.Length - arrayIndex < elements.Count)
                throw new ArgumentException("Array is too small");
            else if (arrayIndex < 0)
                throw new IndexOutOfRangeException("Index must be positive");
            else if (array == null)
                throw new ArgumentNullException("Undefined array");
            else
                elements.Values.CopyTo(array, arrayIndex);
        }
        public void CopyTo(KeyValuePair<string, Element>[] array, int arrayIndex)
        {
            ((IDictionary<string, Element>)elements).CopyTo(array, arrayIndex);
        }

        public bool TryGetValue(string key, out Element value)
        {
            return ((IDictionary<string, Element>)elements).TryGetValue(key, out value);
        }

        public IEnumerator<Element> GetEnumerator()
        {
            return ((IDictionary<string, Element>)elements).Values.GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<string, Element>)elements).GetEnumerator();
        }
        IEnumerator<KeyValuePair<string, Element>> IEnumerable<KeyValuePair<string, Element>>.GetEnumerator()
        {
            return ((IDictionary<string, Element>)elements).GetEnumerator();
        }

        /// <summary>
        /// Returns an Element matching the given name if it exists or null if it doesn't.
        /// As every Element should have a different name, there shouldn't be any problems
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The first Element matching the given name or null if not found.</returns>
        public override Element Find(string name)
        {
            // Copy the dictionnary to avoid problems with it being modified elsewhere while in the loop
            Element[] temp = new Element[elements.Count];
            elements.Values.CopyTo(temp, 0);

            Element res = null;

            if (Name == name)
            {
                return this;
            }
            else if (elements.Keys.Contains(name))
            {
                return this[name];
            }
            else
            {
                foreach (Element e in temp)
                {
                    res = e.Find(name);
                    if (res != null) return res;
                }
            }

            return null;
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            // Copy the dictionnary to avoid problems with it being modified elsewhere while in the loop
            Element[] temp = new Element[elements.Count];
            elements.Values.CopyTo(temp, 0);

            var res = new NonGenericElementList(Name, IsDefault);
            foreach (Element e in temp)
            {
                res.Add(e.Copy());
            }

            return res;
        }

        public override void UpdateFromIndex(NewIndex index)
        {
            // Copy the dictionnary to avoid problems with it being modified elsewhere while in the loop
            Element[] tempList = new Element[elements.Count];
            elements.Values.CopyTo(tempList, 0);

            var temp = index.Find(Name);

            if (temp is NonGenericElementList l)
            {
                foreach (Element t in tempList)
                {
                    t.UpdateFromIndex(index);
                }
            }
            /*
            else if (temp == null)
                throw new MissingFieldException("The element can't be found in the Index and can't be updated.");
            else
                throw new InvalidCastException("The Element in the Index does not match this one and can't be updated.");
            */
        }
    }
}
