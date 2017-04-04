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
        public BoxText(double x = 0, double y = 0, double z = 60, double sizeX = 10, double sizeY = 10, double angle = 0)//0 en the upper left corner - cm
            : base(x, y, z, sizeX, sizeY, 0)
        {}

        /// <summary>
        /// Box depth position, determines whether an element is above another one or below.
        /// For background boxes, it is a number above 50.
        /// </summary>
        public override double Z { get => z; set => z = value < 51 ? 51 : value; }

        public ResumeElements.Element Element { get; set; }
        
        public string DefaultElement { get; set; }
    }
}
