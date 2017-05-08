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
            DisplayFormat = displayFormat;
        }

        protected string displayFormat;
        /// <summary>
        /// If there is two dates (beginning and ending) : "[words] $1(display format)$ [words] $2(display format)$ [word]" (the two dates can be switched)
        /// If there is just a single date or it is not finished : "[words] $1(display format)$ [words]"
        /// 
        /// The date display format using the type described at this address :
        /// https://msdn.microsoft.com/en-us/library/az4se3k1(v=vs.110).aspx
        /// </summary>
        public string DisplayFormat
        {
            get => displayFormat;
            set
            {
                if (IsDisplayFormatGood(value))
                    displayFormat = value;
                else
                    displayFormat = GenerateDisplayFormat();
            }
        }

        /// <summary>
        /// Get display format structure as follows :
        /// ["foreword","start"/"end","format","middleword"/"endword","start"/"end"/"","format"/"","endword"/""]
        /// </summary>
        /// <returns></returns>
        public string[] GetDisplayFormatStructure()
        {
            string[] tab = { "", "", "", "", "", "", "" };
            
            var temp = "";

            var rank = DisplayFormat.IndexOf("$");

            tab[0] = DisplayFormat.Substring(0, rank);

            if (DisplayFormat[rank + 1] == '1')
            {
                tab[1] = "start";
                tab[2] = StartTime.ToString(GetDisplayFormat(1));
            }
            else if (DisplayFormat[rank + 1] == '2')
            {
                tab[1] = "end";
                tab[2] = EndTime.ToString(GetDisplayFormat(2));
            }

            rank = DisplayFormat.IndexOf(")$");
            temp = DisplayFormat.Substring(rank + 2);

            if (temp.Contains("$"))
            {
                rank = temp.IndexOf("$");
                tab[3] = temp.Substring(0, rank);
                
                if (temp[rank + 1] == '1')
                {
                    tab[4] = "start";
                    tab[5] = StartTime.ToString(GetDisplayFormat(1));
                }
                else if (temp[rank + 1] == '2')
                {
                    tab[4] = "end";
                    tab[5] = EndTime.ToString(GetDisplayFormat(2));
                }

                rank = temp.IndexOf(")$");
                tab[6] = temp.Substring(rank + 2);
            }
            else
            {
                tab[3] = temp;
            }

            return tab;
        }

        public string RenderDates()
        {
            var res = GetDisplayFormatStructure();

            return res[0] + res[2] + res[3] + res[5] + res[6];
        }

        /// <summary>
        /// Useful ?
        /// </summary>
        /// <returns></returns>
        public static bool IsDisplayFormatGood(string format)
        {
            var date = new DateTime(2000, 12, 1);

            if (format == "") return false;

            if (format.Contains("$1") == false)
                return false;
            try
            {
                date.ToString(GetDisplayFormatFrom(format,1));
            }
            catch (Exception)
            {
                return false;
            }

            if (format.Contains("$2") == true)
            {
                try
                {
                    date.ToString(GetDisplayFormatFrom(format,2));
                }
                catch (Exception)
                {
                    return false;
                }
            }
            
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="date">1 for start and 2 for end time</param>
        /// <returns></returns>
        public string GetDisplayFormat(int date)
        {
            return GetDisplayFormatFrom(DisplayFormat, date);
        }

        public static string GetDisplayFormatFrom(string format, int date)
        {
            var res = "";
            var rank = format.IndexOf("$" + date.ToString());
            res = format.Substring(rank+3);
            rank = res.IndexOf(")$");
            res = res.Substring(0, rank);

            return res;
        }

        /// <summary>
        /// Generate the format to display the date based on the given dates
        /// </summary>
        /// <returns></returns>
        public string GenerateDisplayFormat()
        {
            var res = "$1(D)$";

            // If it hasn't finished
            if (EndTime == default(DateTime))
            {
                res = "Depuis " + res;
            }
            // If there's an end date
            else if (EndTime != StartTime)
            {
                res += " - " + "$2(D)$";
            }

            return res;
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
                else
                    endTime = value;
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