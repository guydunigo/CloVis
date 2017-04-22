using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    //public enum FontElementCategory { Title1, Title2, Title3, Body, Reference };

    public class Fonts: ICollection<FontElement>, IDictionary<string, FontElement>, IEnumerable<FontElement>
    {
        public Fonts(string name)
        {
            Name = name;
            Dict = new Dictionary<string, FontElement>();
            // throw new NotImplementedException("Integer/Enum/string?");
        }

        public FontElement this[string key] { get => Dict[key]; set => Dict[key] = value; } // => signifie fonction rapide qui retourne ce qui suit

        public string Name { get; set; }
        public Dictionary<string,FontElement> Dict { get; set; }

        public int Count => Dict.Count;

        public bool IsReadOnly => false;

        public ICollection<string> Keys => Dict.Keys;

        public ICollection<FontElement> Values => Dict.Values;

        public void Add(string key, FontElement value)
        {
            Dict.Add(key, value);
        }

        public void Add(FontElement item)
        {
            Dict.Add(item.Name, item);
        }

        public void Add(KeyValuePair<string, FontElement> item)
        {
            Dict.Add(item.Key,item.Value);
        }

        public void Clear()
        {
            Dict.Clear();
        }

        public bool Contains(FontElement item)
        {
            return Dict.ContainsValue(item);
        }

        public bool Contains(KeyValuePair<string, FontElement> item)
        {
            return Dict.Contains(item);
        }

        public bool ContainsKey(string key)
        {
            return Dict.ContainsKey(key);
        }

        public void CopyTo(FontElement[] array, int arrayIndex)
        {
            Dict.Values.CopyTo(array, arrayIndex);
        }

        public void CopyTo(KeyValuePair<string, FontElement>[] array, int arrayIndex)
        {
            foreach(KeyValuePair<string, FontElement> p in Dict)
            {
                array[arrayIndex] = p;
                arrayIndex++;
            }
        }

        public IEnumerator<FontElement> GetEnumerator()
        {
            return Dict.Values.GetEnumerator();
        }

        public bool Remove(FontElement item)
        {
            foreach(string name in Dict.Keys)
            {
                if (Dict[name] == item)
                    return Dict.Remove(name);
            }
            return false;
        }

        public bool Remove(string key)
        {
            return Dict.Remove(key);
        }

        public bool Remove(KeyValuePair<string, FontElement> item)
        {
            return Dict.Remove(item.Key);
        }

        public bool TryGetValue(string key, out FontElement value)
        {
            return Dict.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dict.Values.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, FontElement>> IEnumerable<KeyValuePair<string, FontElement>>.GetEnumerator()
        {
            return Dict.GetEnumerator();
        }

        /// <summary>
        /// Deep copy
        /// </summary>
        /// <returns></returns>
        public Fonts Copy()
        {
            var temp = new Fonts(Name);
            foreach(string f in Dict.Keys)
            {
                temp.Dict.Add(f,Dict[f].Copy());
            }
            return temp;
        }
    }
}
