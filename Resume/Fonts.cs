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
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(FontElement[] array, int arrayIndex)
        {
            Dict.Values.CopyTo(array, arrayIndex);
        }

        public void CopyTo(KeyValuePair<string, FontElement>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, FontElement> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out FontElement value)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Dict.Values.GetEnumerator();
        }

        IEnumerator<KeyValuePair<string, FontElement>> IEnumerable<KeyValuePair<string, FontElement>>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
