using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace Resume
{
    public class FontElement
    {
        public FontElement(string fontFamilyName, double size, Color color, bool italic = false, bool bold = false, bool underlined = false, bool upperCase = false, string name = "")
            : this(new FontFamily(fontFamilyName), size, color, italic, bold, underlined, upperCase, name)
        { }
        public FontElement(FontFamily font, double size, Color color, bool italic = false, bool bold = false, bool underlined = false, bool upperCase = false, string name = "")
        {
            Name = name;
            Font = font;
            //throw new NotImplementedException("Check ratio with actual size in the view");
            FontSize = size;
            Color = color;
            Italic = italic;
            Bold = bold;
            Underlined = underlined;
            UpperCase = upperCase;
        }

        public string Name { get; }
        public FontFamily Font { get; set; }
        public double FontSize { get; set; }
        public bool Italic { get; set; }
        public bool Bold { get; set; }
        public bool Underlined { get; set; }
        public Color Color { get; set; }
        /// <summary>
        /// Defines wether the text shown with this PoliceElement will be in upper case no matter how the user entered it or not
        /// </summary>
        public bool UpperCase { get; set; }
        
        /// <summary>
        /// Deep copy
        /// </summary>
        /// <returns></returns>
        public FontElement Copy()
        {
            return new FontElement(Font, FontSize, Color, Italic, Bold, Underlined, UpperCase, Name);
        }
    }
}
