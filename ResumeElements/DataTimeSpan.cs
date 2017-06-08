using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ResumeElements
{
    public class DataTimeSpan<T> : Data<T>, INotifyPropertyChanged
    {
        public DataTimeSpan(T value, TimeSpan span, string displayFormat, double level = -1, string description = "", bool isIndependant = false, bool isDefault = true) : this(Index.GetUnusedName(value), value, span, displayFormat, level, description, isIndependant, isDefault)
        {
        }

        public DataTimeSpan(string name, T value, TimeSpan span, string displayFormat, double level = -1, string description = "", bool isIndependant = false, bool isDefault = true) : base(name, value, level, description, isIndependant, isDefault)
        {
            TimeSpan = span;

            if (IsDisplayFormatGood(displayFormat))
            {
                this.displayFormat = displayFormat;
            }
            else
                displayFormat = GenerateDefaultDisplayFormat();
        }

        public string RenderedTimeSpan
        {
            get => RenderTimeSpan();
        }

        private TimeSpan timespan;
        public TimeSpan TimeSpan
        {
            get => timespan;
            set
            {
                timespan = value;
                NotifyPropertyChanged("TimeSpan");
                NotifyPropertyChanged("RenderedTimeSpan");
            }
        }

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
                {
                    displayFormat = value;
                    NotifyPropertyChanged("DisplayFormat");
                    NotifyPropertyChanged("RenderedTimeSpan");
                }
            }
        }

        public static string[] GetDisplayFormatStructure(string displayFormat)
        {
            string[] tab = { "", "", "" };

            var rank = displayFormat.IndexOf("$");

            tab[0] = displayFormat.Substring(0, rank);

            tab[1] = GetDisplayFormatFrom(displayFormat);

            rank = displayFormat.IndexOf(")$");
            tab[2] = displayFormat.Substring(rank + 2);

            return tab;
        }

        /// <summary>
        /// ["foreword","format","endword"]
        /// </summary>
        /// <returns></returns>
        public string[] GetDisplayFormatStructure()
        {
            return GetDisplayFormatStructure(DisplayFormat);
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
            res = format.Substring(rank + 2);
            rank = res.IndexOf(")$");
            res = res.Substring(0, rank);

            return res;
        }

        /// <summary>
        /// Generate the format to display the timespan based on the given one
        /// </summary>
        /// <returns></returns>
        public string GenerateDefaultDisplayFormat()
        {
            return "Pendant $(%d)$ jours";
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            return new DataTimeSpan<T>(Name, Value, TimeSpan, DisplayFormat, Level, Description, true, IsDefault);
        }

        public override void UpdateFromIndex()
        {
            base.UpdateFromIndex();
            if (Index.Find(Name) is DataTimeSpan<T> d)
            {
                TimeSpan = d.TimeSpan;
                DisplayFormat = d.DisplayFormat;
            }
            //else
            //    throw new InvalidCastException("The piece of Data in the Index does not match this one and can't be updated.");
        }

        public static DataTimeSpan<T> Replace(Data<T> data, TimeSpan timeSpan = default(TimeSpan), string displayFormat = "")
        {
            // Mew mew (== Mewtwo)
            var cats = data.Categories;

            if (!data.IsIndependant)
            {
                Index.Erase(data);
            }

            var dest = new DataTimeSpan<T>(data.Name, data.Value, timeSpan, displayFormat, data.Level, data.Description, data.IsIndependant, data.IsDefault);

            foreach (ElementList el in cats)
            {
                dest.AddCategory(el);
            }

            data.ClearCategories();

            return dest;
        }

        public static string GenerateDisplayFormat(string start, string end, string format = "%d")
        {
            return start + "$(" + format + ")$" + end;
        }
    }
}
