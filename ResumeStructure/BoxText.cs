using ResumeElements;

namespace ResumeStructure
{

    public class BoxText : Box
    {
        public BoxText(double x = 0, double y = 0, double z = 60, double sizeX = 10, double sizeY = 10, double angle = 0, string defaultElmt = "", Fonts fonts = null)
            : base(x, y, z, sizeX, sizeY, angle)
        {
            DefaultElement = defaultElmt;
            Fonts = fonts;
        }

        /// <summary>
        /// Used to define a boxwise font. Set to null if the default Resume font is used.
        /// </summary>
        public Fonts Fonts { get; set; }

        /// <summary>
        /// Box depth position, determines whether an element is above another one or below.
        /// For background boxes, it is a number above 50.
        /// </summary>
        public override double Z { get => z; set => z = value < 51 ? 51 : value; }

        protected ResumeElements.Element element;
        public ResumeElements.Element Element { get => element; set => element = value?.Copy(); }

        public string DefaultElement { get; set; }

        /// <summary>
        /// Performs a deep copy of the box
        /// </summary>
        /// <returns></returns>
        public override Box Copy()
        {
            return new BoxText(X, Y, Z, SizeX, SizeY, Angle, DefaultElement, Fonts?.Copy()) { Element = Element?.Copy() };
        }

        public void UpdateFromIndex(Index index = null)
        {
            if (Element != null)
                Element.UpdateFromIndex(index);
            else if (index != null)
            {
                // Try to find the default element in the index
                Element = index.Find(DefaultElement)?.Copy();
            }
        }

        /// <summary>
        /// Empty Element property
        /// </summary>
        /// <param name="index"></param>
        public void ClearElement()
        {
            Element = null;
        }
    }
}
