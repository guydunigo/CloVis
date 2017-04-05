using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    /// <summary>
    /// Specifies a timespan with set starting and ending dates
    /// </summary>
    public class DataDated<T>: Data<T>
    {
        public DataDated(string name, T value, DateTime start, DateTime end = default(DateTime), double level = -1, string description = "", bool isDefault = true) : base(name, value, level, description, isDefault)
        {
            StartTime = start;
            EndTime = end;
            throw new NotImplementedException("StringFormat");
        }

        public DateTime StartTime { get; set; }
        protected DateTime endTime;
        /// <summary>
        /// If it hasn't finished, EndTime is default(DateTime). If there is just a date, EndTime equals StartTime.
        /// Also, EndTime cannot be prior to StartTime, in which case it is set to StartTime
        /// </summary>
        public DateTime EndTime
        {
            get
            {
                return endTime;
            }
            set
            {
                if (value == default(DateTime))
                {
                    endTime = default(DateTime);
                }
                else if (value < StartTime)
                {
                    endTime = StartTime;
                }
            }
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            return new DataDated<T>(Name, Value, StartTime, EndTime, Level, Description, true);
        }
    }
}