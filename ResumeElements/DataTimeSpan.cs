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

            DisplayFormat = displayFormat;
        }

        public TimeSpan TimeSpan { get; set; }

        protected string displayFormat;
        /// <summary>
        /// Must follow this format : "[words] $(Display format) [words]"
        /// 
        /// The timeSpan display format using the type described at this address :
        /// https://msdn.microsoft.com/en-us/library/ee372286(v=vs.110).aspx
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
        /// ["foreword","format","endword"]
        /// </summary>
        /// <returns></returns>
        public string[] GetDisplayFormatStructure()
        {
            string[] tab = { "", "", "" };
            
            var rank = DisplayFormat.IndexOf("$");

            tab[0] = DisplayFormat.Substring(0, rank);

            tab[1] = GetDisplayFormat();

            rank = DisplayFormat.IndexOf(")$");
            tab[2] = DisplayFormat.Substring(rank + 2);

            return tab;
        }

        public string RenderTimeSpan()
        {
            var res = GetDisplayFormatStructure();

            return res[0] + TimeSpan.ToString(res[1]) + res[2];
        }

        /// <summary>
        /// Useful ?
        /// </summary>
        /// <returns></returns>
        public static bool IsDisplayFormatGood(string format)
        {
            var ts = new TimeSpan(2000, 12, 1);

            if (format == "") return false;

            if (format.Contains("$(") == false)
                return false;
            try
            {
                ts.ToString(GetDisplayFormatFrom(format));
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetDisplayFormat()
        {
            return GetDisplayFormatFrom(DisplayFormat);
        }

        public static string GetDisplayFormatFrom(string format)
        {
            var res = "";
            var rank = format.IndexOf("$(");
            res = format.Substring(rank+2);
            rank = res.IndexOf(")$");
            res = res.Substring(0, rank);

            return res;
        }

        /// <summary>
        /// Generate the format to display the timespan based on the given one
        /// </summary>
        /// <returns></returns>
        public string GenerateDisplayFormat()
        {
            return "Pendant $(%d)$ jours";
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
