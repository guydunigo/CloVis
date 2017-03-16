using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    public class ElementList<T> : Element, IList<Element>, ICollection<Element>, IEnumerable<Element> where T : Element
    {
        // On autorise toutes les modifs sur la liste ou on veut les contrôler ?
        public List<Element> Elements { get; set; }

    }
}
