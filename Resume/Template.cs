using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resume
{
    public class Template : Resume
    {
        public Template(string name) : base(name) // Useless ?
        {
            //throw new NotImplementedException("Useless ?");
        }

        public override void UpdateFromIndex()
        {
            foreach(BoxText b in Layout.TextBoxes)
            {
                b.Element = null;
            }
            base.UpdateFromIndex();
        }
    }
}
