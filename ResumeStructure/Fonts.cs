using System.Collections;
using System.Collections.Generic;
using Windows.UI.Xaml;

namespace ResumeStructure
{
    public class Fonts : ICollection<FontElement>, IList<FontElement>, IEnumerable<FontElement>
    {
        public Fonts(string name, TextAlignment textAlignment = TextAlignment.Left)
        {
            Name = name;
            List = new List<FontElement>();
            TextAlignment = textAlignment;
        }

        public FontElement this[int index] { get => List[index]; set => List[index] = value; }

        public string Name { get; set; }
        public TextAlignment TextAlignment { get; set; }

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

        public static Fonts GetDefault()
        {
            //throw new NotImplementedException("Create a more generic Fonts");
            return new Fonts("Polices_cv")
            {
                new FontElement("Tahoma", 7, new Windows.UI.Color() { R = 0, G = 0, B = 255, A = 255 },true, true, true, true), //ARGB 0 on voit rien, 255 opaque
                new FontElement("Tahoma", 6, new Windows.UI.Color() { R = 0, G = 0, B = 150, A = 255 },false, true, false, true),
                new FontElement("Tahoma", 5, new Windows.UI.Color() { R = 100, G = 100, B = 200, A = 255 },true, false, false, false),
                new FontElement("Calibri", 5, new Windows.UI.Color() { R = 70, G = 70, B = 200, A = 190 })
            };
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
            var temp = new Fonts(Name, TextAlignment);
            foreach (FontElement e in List)
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
