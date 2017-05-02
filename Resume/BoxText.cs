using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace Resume
{
    public class BoxText: Box
    {
        public BoxText(double x = 0, double y = 0, double z = 60, double sizeX = 10, double sizeY = 10, double angle = 0, string defaultElmt = "")//0 on the upper left corner - cm
            : base(x, y, z, sizeX, sizeY, angle)
        {
            DefaultElement = defaultElmt;
        }

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
        public BoxText Copy()
        {
            return new BoxText(X, Y, Z, SizeX, SizeY, Angle, DefaultElement) { Element = Element.Copy() };
        }

        public void UpdateFromIndex()
        {
            if (Element != null)
                Element.UpdateFromIndex();
            else
            {
                // Try to find the default element in the index
                Element = ResumeElements.Index.Find(DefaultElement);
            }
        }
    }
}
