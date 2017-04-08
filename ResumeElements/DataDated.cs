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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="start"></param>
        /// <param name="end">If it hasn't finished, EndTime is default(DateTime). If there is just a date, EndTime equals StartTime.</param>
        /// <param name="displayFormat"></param>
        /// <param name="level"></param>
        /// <param name="description"></param>
        /// <param name="isDefault"></param>
        public DataDated(string name, T value, DateTime start, DateTime end = default(DateTime), string displayFormat = "", double level = -1, string description = "", bool isDefault = true) : base(name, value, level, description, isDefault)
        {
            StartTime = start;
            EndTime = end;

            if (displayFormat == "")
                GenerateDisplayFormat();
            //throw new NotImplementedException("DisplayFormat");
        }

        /// <summary>
        /// The timeSpan display format using the type described at this address :
        /// https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
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

        protected DateTime startTime;
        public DateTime StartTime {
            get => startTime;
            set
            {
                if (startTime == endTime)
                {
                    startTime = value;
                    endTime = value;
                }
                else if (value > endTime) // If value is a date after endTime, set dateTime to one day after the new value
                {
                    startTime = value;
                    endTime = startTime + new TimeSpan(1, 0, 0, 0);
                }
            }
        }
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
            return new DataDated<T>(Name, Value, StartTime, EndTime, DisplayFormat, Level, Description, true);
        }

        public override void UpdateFromIndex()
        {
            base.UpdateFromIndex();
            if (Index.Find(Name) is DataDated<T> d)
            {
                StartTime = d.StartTime;
                EndTime = d.EndTime;
            }
            else
                throw new InvalidCastException("The piece of Data in the Index does not match this one and can't be updated.");
        }
    }
}