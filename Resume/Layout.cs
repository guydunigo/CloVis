using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Resume
{
   
    public class Layout
    {
        public Layout()
        {
           
            BackBoxes = new List<BoxBackground>();
            TextBoxes = new List<BoxText>();
        }

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

        /// <summary>
        /// Create a new BackBox and add it to Layout
        /// </summary>
        public void AddBackBox(double x=0, double y=0, double z=10, double SizeX=10, double SizeY=10,Windows.UI.Xaml.Controls.Image img=null)
        {
            var box = new BoxBackground(x, y, z, SizeX, SizeY, img);
            BackBoxes.Add(box);
        }

        /// <summary>
        /// Add a previously created BackBox to Layout
        /// </summary>
        public void AddBackBox(BoxBackground box)
        {
            BackBoxes.Add(box);
        }


        /// <summary>
        /// Create a new TextBox and add it to Layout
        /// </summary>
        public void AddTextBox(double x=0, double y=0, double z=60, double SizeX=10, double SizeY=10, double o=0, string defaultElmt="")
        {
            var box = new BoxText(x,y,z,SizeX,SizeY,o,defaultElmt);
            TextBoxes.Add(box);
        }

        /// <summary>
        /// Add a previously created TextBox to Layout
        /// </summary>
        public void AddTextBox(BoxText box)
        {
            TextBoxes.Add(box);
        }

    }
}
