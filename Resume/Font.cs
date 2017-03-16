using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    public enum FontElementCategory { Title1, Title2, Title3, Body, Reference };

    public class Font
    {
        public Font(string name)
        {
            Name = name;
            Fonts = new Dictionary<FontElementCategory, FontElement>();
            // Default ?
        }

        public string Name { get; set; }
        public Dictionary<FontElementCategory,FontElement> Fonts { get; set; }
    }
}
