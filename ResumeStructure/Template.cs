namespace ResumeStructure
{
    public class Template : Resume
    {
        public Template(string name) : base(name)
        {
            //throw new NotImplementedException("Useless ?");
        }

        public override void UpdateFromIndex(ResumeElements.Index index = null)
        {
            foreach (BoxText b in Layout.TextBoxes)
            {
                b.ClearElement();
            }
            base.UpdateFromIndex();
        }
    }
}
