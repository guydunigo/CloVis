using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    class DataTimeSpan: Data<string>
    {
        public DataTimeSpan(string name, string value, TimeSpan span, string description = "", double level = -1, bool isDefault = false) : base(name, value, description, level, isDefault)
        {
            TimeSpan = span;
            throw new NotImplementedException("StringFormat");
        }

        public TimeSpan TimeSpan { get; set; }
    }
}
