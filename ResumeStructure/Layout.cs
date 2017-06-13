using System.Collections.Generic;

namespace ResumeStructure
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

        /// <summary>
        /// Performs a deep copy of the two lists
        /// </summary>
        /// <returns></returns>
        public Layout Copy()
        {
            var tempBB = new List<BoxBackground>();
            foreach (BoxBackground b in BackBoxes)
            {
                tempBB.Add(b.Copy() as BoxBackground);
            }
            var tempTB = new List<BoxText>();
            foreach (BoxText b in TextBoxes)
            {
                tempTB.Add(b.Copy() as BoxText);
            }

            return new Layout()
            {
                BackBoxes = tempBB,
                TextBoxes = tempTB
            };
        }

        /// <summary>
        /// Add a previously created BackBox to Layout
        /// </summary>
        public void AddBackBox(BoxBackground box)
        {
            BackBoxes.Add(box);
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
