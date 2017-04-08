using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    public class DataTimeSpan<T>: Data<T>
    {
        public DataTimeSpan(string name, T value, TimeSpan span, string displayFormat, double level = -1, string description = "", bool isDefault = true) : base(name, value, level, description, isDefault)
        {
            TimeSpan = span;

            if (displayFormat == "")
                GenerateDisplayFormat();
            //throw new NotImplementedException("DisplayFormat");
        }

        public TimeSpan TimeSpan { get; set; }

        /// <summary>
        /// The timeSpan display format using the type described at this address :
        /// https://msdn.microsoft.com/en-us/library/ee372286(v=vs.110).aspx
        /// </summary>
        public string DisplayFormat { get; set; }

        /// <summary>
        /// Generate the format to display the date based on the given dates
        /// </summary>
        /// <returns></returns>
        public string GenerateDisplayFormat()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            return new DataTimeSpan<T>(Name, Value, TimeSpan, DisplayFormat, Level, Description, true);
        }

        public override void UpdateFromIndex()
        {
            base.UpdateFromIndex();
            if (Index.Find(Name) is DataTimeSpan<T> d)
            {
                TimeSpan = d.TimeSpan;
            }
            else
                throw new InvalidCastException("The piece of Data in the Index does not match this one and can't be updated.");
        }
    }
}
