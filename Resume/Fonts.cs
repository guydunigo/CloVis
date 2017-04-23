using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    public class Fonts: ICollection<FontElement>, IList<FontElement>, IEnumerable<FontElement>
    {
        public Fonts(string name)
        {
            Name = name;
            List = new List<FontElement>();
            // throw new NotImplementedException("Integer/Enum/string?");
        }

        public FontElement this[int index] { get => List[index]; set => List[index] = value; }

        public string Name { get; set; }

        /// <summary>
        /// The Default is the last element in the list. If the list is empty, return a basic black test.
        /// </summary>
        public FontElement Default
        {
            get
            {
                if (Count != 0) return this[Count - 1];
                else return new FontElement("Tahoma", 5, Windows.UI.Colors.Black);
            }
        }

        /// <summary>
        /// Store all the FontElements from the most important (eg. Title 1) to least (eg. Body text). The Default value is the last element.
        /// </summary>
        public List<FontElement> List { get; set; }

        public int Count => List.Count;

        public bool IsReadOnly => false;

        public void Add(FontElement item)
        {
            List.Add(item);
        }

        public void Clear()
        {
            List.Clear();
        }

        public bool Contains(FontElement item)
        {
            return List.Contains(item);
        }

        public void CopyTo(FontElement[] array, int arrayIndex)
        {
            List.CopyTo(array, arrayIndex);
        }

        public IEnumerator<FontElement> GetEnumerator()
        {
            return List.GetEnumerator();
        }

        public int IndexOf(FontElement item)
        {
            return List.IndexOf(item);
        }

        public void Insert(int index, FontElement item)
        {
            List.Insert(index, item);
        }

        public bool Remove(FontElement item)
        {
            return List.Remove(item);
        }

        public void RemoveAt(int index)
        {
            List.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Deep copy
        /// </summary>
        /// <returns></returns>
        public Fonts Copy()
        {
            var temp = new Fonts(Name);
            foreach(FontElement e in List)
            {
                temp.List.Add(e.Copy());
            }
            return temp;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="e">The output values the requested element at this index if it exists, the last one if not</param>
        /// <returns>Returns true if there is a FontElement at this index and false otherwise.</returns>
        public bool TryGetValue(int index, out FontElement e)
        {
            if (index < Count)
            {
                e = this[index];
                return true;
            }
            else
            {
                e = Default;
                return false;
            }
        }
    }
}
