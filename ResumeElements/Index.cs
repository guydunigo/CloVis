using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    /// <summary>
    /// The index lists all piece of Data that can be put in a resume and it contains the root of the whole hierarchy of those/this data
    /// </summary>
    public class Index : ElementList<Data>
    {
        public Index(string name, bool isDefault = true) : base(name, isDefault)
        {
            Root = new ElementList<Element>("root");
            throw new NotImplementedException("Base ElementsLists");
        }

        // Add new info
        // Remove info
        
        //find an element by name/value
        //+return list

        // Contains recursive

        // Child in CV ? update to mainIndex ?

        /// <summary>
        /// Root defines the topmost ElementList, mother of all Elements that can be put in a resume
        /// </summary>
        public ElementList<Element> Root { get; set; }
    }
}
