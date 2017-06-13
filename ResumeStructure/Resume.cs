using ResumeElements;

namespace ResumeStructure
{
    public class Resume
    {
        public Resume(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Lists all the boxes (background and text boxes of the resume
        /// </summary>
        public Layout Layout { get; set; }

        /// <summary>
        /// Defines how each kind of text will be displayed on the resume
        /// </summary>
        public Fonts Fonts { get; set; }


        public string Name { get; set; }

        public virtual void UpdateFromIndex(Index index = null)
        {
            foreach (BoxText b in Layout.TextBoxes)
            {
                b.UpdateFromIndex(index);
            }
        }

        /// <summary>
        /// Deep copy of the resume
        /// </summary>
        public Resume Copy()
        {
            return new Resume(Name)
            {
                Layout = Layout.Copy(),
                Fonts = Fonts.Copy(),
                Name = Name
            };
        }

        public ElementList LocalIndex
        {
            get
            {
                var res = new ElementList("Root");

                foreach (BoxText bt in Layout.TextBoxes)
                {
                    if (bt.Element != null)
                        res.Add(bt.Element);
                }

                return res;
            }
        }
    }
}
