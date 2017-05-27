using ResumeElements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Resume
{
    public enum BoxBackgroundShape
    {
        Rectangle,
        Ellipse
    }

    public class BoxBackground : Box
    {
        public BoxBackground(double x = 0, double y = 0, double z = 10, double sizeX = 10, double sizeY = 10, double angle = 0, BoxBackgroundShape shape = BoxBackgroundShape.Rectangle)
            :base(x,y,z,sizeX,sizeY,angle)
        {
            Shape = shape;
            Fill = Colors.Orange;
            Stroke = Colors.Transparent;
            StrokeThickness = 0;
            BorderRadius = 0;
            Image = null;
        }

        /// <summary>
        /// Only works when shape is Rectangle
        /// </summary>
        public double BorderRadius { get; set; }
        public double StrokeThickness { get; set; }
        public BoxBackgroundShape Shape { get; set; }

        /// <summary>
        /// Box depth position, determines whether an element is above another one or below.
        /// For background boxes, it is a number below 50.
        /// </summary>
        public override double Z { get => z; set => z = value > 50 ? 50 : value; }
        /// <summary>
        /// Color defines the background color of the box and the transparancy of it.
        /// </summary>
        public Color Fill { get; set; }
        public Color Stroke { get; set; }
        /// <summary>
        /// Defines the background image of the box
        /// </summary>
        public DataImage Image { get; set; }

        /// <summary>
        /// Performs a deep copy of the box
        /// </summary>
        /// <returns></returns>
        public BoxBackground Copy()
        {
            return new BoxBackground(X, Y, Z, SizeX, SizeY, Angle, Shape)
            {
                Image = Image,
                Fill = Fill,
                Stroke = Stroke,
                BorderRadius = BorderRadius,
                StrokeThickness = StrokeThickness
            };
        }
    }
}
