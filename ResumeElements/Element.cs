using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    public abstract class Element
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name">Elements name, there can't be two elements with the same name</param>
        /// <param name="isDefault">Defines wehter the element is to be shown on a resume by default (may not be used)</param>
        public Element(string name, bool isDefault = true)
        {
            Name = name;
            IsDefault = isDefault;
        }
        
        // throw new NotImplementedException("locked ?")
        public bool IsDefault { get; set; }
        public string Name { get; }
    }
}