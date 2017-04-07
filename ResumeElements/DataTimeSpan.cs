using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    public class DataTimeSpan<T>: Data<T>
    {
        public DataTimeSpan(string name, T value, TimeSpan span, double level = -1, string description = "", bool isDefault = true) : base(name, value, level, description, isDefault)
        {
            TimeSpan = span;
            throw new NotImplementedException("StringFormat");
        }

        public TimeSpan TimeSpan { get; set; }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            return new DataTimeSpan<T>(Name, Value, TimeSpan, Level, Description, true);
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
