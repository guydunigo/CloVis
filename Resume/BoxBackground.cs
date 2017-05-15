using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI;

namespace Resume
{
   
    public class BoxBackground : Box
    {
       
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Resume.BoxBackground"/> class.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="z">The z coordinate.</param>
        /// <param name="sizeX">Size x.</param>
        /// <param name="sizeY">Size y.</param>
        /// <param name="img">Image.</param>
        /// <param name="angle">Angle: clockwise & degrees</param>
        public BoxBackground(double x = 0, double y = 0, double z = 10, double sizeX = 10, double sizeY = 10, Windows.UI.Xaml.Controls.Image img = null, double angle = 0) //deg? trigo ?
            : this(Colors.Transparent,Colors.Transparent , x, y, z, sizeX, sizeY, img, angle)
        {}
        public BoxBackground(Color color, Color borderColor, double x = 0, double y = 0, double z = 10, double sizeX = 10, double sizeY = 10, Windows.UI.Xaml.Controls.Image img = null, double angle = 0, double borderRadius = 0)
            : base(x, y, z, sizeX, sizeY, angle)
        {
            // Default ?
            Color = color;
            BorderColor = borderColor;
            Image = img;
            BorderRadius = borderRadius;
        }
        
        public double BorderRadius;

        /// <summary>
        /// Box depth position, determines whether an element is above another one or below.
        /// For background boxes, it is a number below 50.
        /// </summary>
        public override double Z { get => z; set => z = value > 50 ? 50: value; }
        /// <summary>
        /// Color defines the background color of the box and the transparancy of it.
        /// </summary>
        public Color Color { get; set; }
        public Color BorderColor { get; set; }
        /// <summary>
        /// Defines the background image of the box
        /// </summary>
        public Windows.UI.Xaml.Controls.Image Image { get; set; }

        /// <summary>
        /// Performs a deep copy of the box
        /// </summary>
        /// <returns></returns>
        public BoxBackground Copy()
        {
            return new BoxBackground(X, Y, Z, SizeX, SizeY, Image, Angle);
        }
    }
}
