using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    public class Layout
    {
        public List<BoxBackground> BackBoxes { get; set; }
        public List<BoxText> TextBoxes { get; set; }
        // Element List + Index

        /// <summary>
        /// Performs a deep copy of the two lists
        /// </summary>
        /// <returns></returns>
        public Layout Copy()
        {
            var tempBB = new List<BoxBackground>();
            foreach(BoxBackground b in BackBoxes)
            {
                tempBB.Add(b.Copy());
            }
            var tempTB = new List<BoxText>();
            foreach (BoxText b in TextBoxes)
            {
                tempTB.Add(b.Copy());
            }
            
            return new Layout()
            {
                BackBoxes = tempBB,
                TextBoxes = tempTB
            };
        }
    }
}
