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
    public class DataDated: Data<string>
    {
        public DataDated(string name, string value, DateTime start, DateTime end = default(DateTime), string description = "", double level = -1, bool isDefault = false) : base(name, value, description, level, isDefault)
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
    }
}