using System;
using System.ComponentModel;

namespace ResumeElements
{
    class DataTextTimeSpan : DataText, INotifyPropertyChanged
    {
        public DataTextTimeSpan(string value, double length, string displayFormat, double level = -1, bool isIndependant = false, bool isDefault = true) : this(Index.GetUnusedName(value), value, length, displayFormat, level, isIndependant, isDefault)
        {
        }

        public DataTextTimeSpan(string name, string value, double length, string displayFormat, double level = -1, bool isIndependant = false, bool isDefault = true) : base(name, value, level, isIndependant, isDefault)
        {
            Length = length;
        }

        public string RenderedTimeSpan
        {
            get => RenderTimeSpan();
        }

        private double length;
        public double Length
        {
            get => length;
            set
            {
                length = value;
                NotifyPropertyChanged("Length");
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
        /// ["foreword","endword"]
        /// </summary>
        /// <returns></returns>
        public string[] GetDisplayFormatStructure()
        {
            return GetDisplayFormatStructure(DisplayFormat);
        }

        public string RenderTimeSpan()
        {
            var res = GetDisplayFormatStructure();

            return res[0] + Length.ToString(res[1]) + res[2];
        }

        /// <summary>
        /// Useful ?
        /// </summary>
        /// <returns></returns>
        public static bool IsDisplayFormatGood(string format)
        {
            double ts = 34.12;

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
            return "Pendant $(G)$ jours";
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            return new DataTextTimeSpan(Name, Value, Length, DisplayFormat, Level, true);
        }

        public override void UpdateFromIndex()
        {
            base.UpdateFromIndex();
            if (Index.Find(Name) is DataTextTimeSpan d)
            {
                Length = d.Length;
                DisplayFormat = d.DisplayFormat;
            }
            //else
            //    throw new InvalidCastException("The piece of Data in the Index does not match this one and can't be updated.");
        }

        public static DataTextTimeSpan Replace(DataText data, double length = 1.0, string displayFormat = "")
        {
            // Mew mew (== Mewtwo)
            var cats = data.Categories;

            if (!data.IsIndependant)
            {
                //Index.Erase(data);
                throw new NotImplementedException();
            }

            var dest = new DataTextTimeSpan(data.Name, data.Value, length, displayFormat, data.Level, data.IsIndependant, data.IsDefault);

            foreach (NonGenericElementList el in cats)
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
