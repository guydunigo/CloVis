using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    class NonGenericElementList : Element, ICollection<Element>, IEnumerable<Element>, IDictionary<string, Element>, INotifyPropertyChanged
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
            throw new NotImplementedException("All properties");
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
                    NotifyListChanged();
                    throw new NotImplementedException("NotifyListChanged called thrice :/ (do it manually)");
                }
                else
                    throw new ArgumentException("The new value hasn't a name corresponding to the key"); // Or rename it ?
            }
        }

        protected void AddToElements(Element e)
        {
            throw new NotImplementedException();
        }
        protected void RemoveFromElements(Element e)
        {
            throw new NotImplementedException();
        }

        public void Add(Element item)
        {
            throw new NotImplementedException();
        }
        public void Add(string key, Element value)
        {
            ((IDictionary<string, Element>)elements).Add(key, value);
        }
        public void Add(KeyValuePair<string, Element> item)
        {
            ((IDictionary<string, Element>)elements).Add(item);
        }

        public bool Remove(string key)
        {
            return ((IDictionary<string, Element>)elements).Remove(key);
        }
        public bool Remove(Element item)
        {
            throw new NotImplementedException();
        }
        public bool Remove(KeyValuePair<string, Element> item)
        {
            return ((IDictionary<string, Element>)elements).Remove(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
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
    }
}
