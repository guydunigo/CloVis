using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ResumeElements;

namespace Resume
{
    public class Resume
    {
        public Resume()
        {
            // default ? Fonts, ...
        }
        
        /// <summary>
        /// Lists all the boxes (background and text boxes of the resume
        /// It is maybe advisable to separate those two kinds of boxes
        /// ! If you add an TextBox, you have to register its element to le elmts list.
        /// </summary>
        public Layout Layout { get; set; }

        /// <summary>
        /// Defines how each kind of text will be displayed on the resume
        /// </summary>
        public Fonts Fonts { get; set; }

        public string Name { get; set; }

        public void UpdateFromIndex()
        {
            foreach(BoxText b in Layout.TextBoxes)
            {
                b.UpdateFromIndex();
            }
        }

        /// <summary>
        /// Deep copy of the resume
        /// </summary>
        public Resume Copy()
        {
            return new Resume()
            {
                Layout = Layout.Copy(),
                Fonts = Fonts.Copy(),
                Name = Name
            };
        }
    }
}
