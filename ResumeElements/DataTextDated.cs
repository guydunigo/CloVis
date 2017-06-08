﻿using System;
using System.ComponentModel;

namespace ResumeElements
{
    public class DataTextDated : DataText, INotifyPropertyChanged
    {
        public DataTextDated(NewIndex index, string value, DateTimeOffset start, DateTimeOffset end = default(DateTimeOffset), string displayFormat = "", double level = -1, bool isDefault = true) : this(index.GetUnusedName(value), value, start, end, displayFormat, level, index, isDefault)
        {
        }

        public DataTextDated(string name, string value, DateTimeOffset start, DateTimeOffset end = default(DateTimeOffset), string displayFormat = "", double level = -1, NewIndex index = null, bool isDefault = true) : base(name, value, level, index, isDefault)
        {
            StartTime = start;
            EndTime = end;

            if (IsDisplayFormatGood(displayFormat))
            {
                this.displayFormat = displayFormat;
            }
            else
                displayFormat = GenerateDefaultDisplayFormat();
        }

        protected DateTimeOffset startTime;
        public DateTimeOffset StartTime
        {
            get => startTime;
            set
            {
                if (startTime == EndTime)
                {
                    startTime = value;
                    endTime = value;
                }
                else if (value > EndTime && EndTime != default(DateTimeOffset)) // If value is a date after endTime, set endTime to one day after the new value
                {
                    startTime = value;
                    endTime = startTime + new TimeSpan(1, 0, 0, 0);
                }
                else
                {
                    startTime = value;
                }
                NotifyPropertyChanged("StartTime");
                NotifyPropertyChanged("RenderedDates");
            }
        }

        protected DateTimeOffset endTime;
        /// <summary>
        /// If it hasn't finished, EndTime is default(DateTime). If there is just a date, EndTime equals StartTime.
        /// Also, EndTime cannot be prior to StartTime, in which case it is set to StartTime
        /// </summary>
        public DateTimeOffset EndTime
        {
            get => endTime;
            set
            {
                if (value == default(DateTimeOffset))
                {
                    endTime = default(DateTimeOffset);
                }
                else if (value < StartTime)
                {
                    endTime = StartTime + new TimeSpan(1, 0, 0, 0);
                }
                else
                    endTime = value;
                NotifyPropertyChanged("EndTime");
                NotifyPropertyChanged("RenderedDates");
            }
        }

        public string RenderedDates
        {
            get => RenderDates();
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
                {
                    displayFormat = value;
                    NotifyPropertyChanged("DisplayFormat");
                    NotifyPropertyChanged("RenderedDates");
                }
            }
        }

        public static string[] GetDisplayFormatStructure(string displayFormat)
        {
            string[] tab = { "", "", "", "", "", "", "" };

            var temp = "";

            var rank = displayFormat.IndexOf("$");

            tab[0] = displayFormat.Substring(0, rank);

            if (displayFormat[rank + 1] == '1')
            {
                tab[1] = "start";
                tab[2] = GetDisplayFormatFrom(displayFormat, 1);
            }
            else if (displayFormat[rank + 1] == '2')
            {
                tab[1] = "end";
                tab[2] = GetDisplayFormatFrom(displayFormat, 2);
            }

            rank = displayFormat.IndexOf(")$");
            temp = displayFormat.Substring(rank + 2);

            if (temp.Contains("$"))
            {
                rank = temp.IndexOf("$");
                tab[3] = temp.Substring(0, rank);

                if (temp[rank + 1] == '1')
                {
                    tab[4] = "start";
                    tab[5] = GetDisplayFormatFrom(displayFormat, 1);
                }
                else if (temp[rank + 1] == '2')
                {
                    tab[4] = "end";
                    tab[5] = GetDisplayFormatFrom(displayFormat, 2);
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
        /// <summary>
        /// Get display format structure as follows :
        /// ["foreword","start"/"end","format","middleword"/"endword","start"/"end"/"","format"/"","endword"/""]
        /// </summary>
        /// <returns></returns>
        public string[] GetDisplayFormatStructure()
        {
            return GetDisplayFormatStructure(DisplayFormat);
        }

        public string RenderDates()
        {
            var res = GetDisplayFormatStructure();
            var date1 = res[1] == "start" ? StartTime.ToString(res[2]) : EndTime.ToString(res[2]);
            var date2 = "";
            if (res[4] != "")
                date2 = res[4] == "start" ? StartTime.ToString(res[5]) : EndTime.ToString(res[5]);
            return res[0] + date1 + res[3] + date2 + res[6];
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
                date.ToString(GetDisplayFormatFrom(format, 1));
            }
            catch (Exception)
            {
                return false;
            }

            if (format.Contains("$2") == true)
            {
                try
                {
                    date.ToString(GetDisplayFormatFrom(format, 2));
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
            res = format.Substring(rank + 3);
            rank = res.IndexOf(")$");
            res = res.Substring(0, rank);

            return res;
        }

        /// <summary>
        /// Generate the format to display the date based on the given dates
        /// </summary>
        /// <returns></returns>
        public string GenerateDefaultDisplayFormat()
        {
            var res = "$1(D)$";

            // If it hasn't finished
            if (EndTime == default(DateTimeOffset))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="first">Foreword</param>
        /// <param name="firstStartEnd">1 : start date, 2 : end date</param>
        /// <param name="firstFormat"></param>
        /// <param name="middle"></param>
        /// <param name="secondStartEnd">0 : disable the following args and the display of the second date, 1 start & 2 end</param>
        /// <param name="secondFormat"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string GenerateDisplayFormat(string first, int firstStartEnd, string firstFormat, string middle, int secondStartEnd = 0, string secondFormat = "y", string end = "")
        {
            string temp = first + "$" + ((firstStartEnd == 1 || firstStartEnd == 2) ? firstStartEnd.ToString() : "1") + "(" + firstFormat + ")$" + middle;

            if (secondStartEnd > 0)
                temp += "$" + ((secondStartEnd == 1 || secondStartEnd == 2) ? secondStartEnd.ToString() : "2") + ")$" + end;

            return temp;
        }

        /// <summary>
        /// Provides a deep copy of every Elements and sub-Elements.
        /// </summary>
        /// <returns></returns>
        public override Element Copy()
        {
            return new DataTextDated(Name, Value, StartTime, EndTime, DisplayFormat, Level, null, IsDefault);
        }

        public override void UpdateFromIndex(NewIndex indexToUse = null)
        {
            base.UpdateFromIndex(indexToUse);

            if (indexToUse == null)
                indexToUse = Index;

            if (indexToUse != null)
            {
                if (indexToUse.Find(Name) is DataTextDated d)
                {
                    StartTime = d.StartTime;
                    EndTime = d.EndTime;
                    DisplayFormat = d.DisplayFormat;
                }
                //else
                //    throw new InvalidCastException("The piece of Data in the Index does not match this one and can't be updated.");
            }
        }

        /// <summary>
        /// Used when you want to transform any DataText to DataTextDated
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="displayFormat"></param>
        /// <returns></returns>
        public static DataTextDated Replace(DataText data, DateTimeOffset start = default(DateTimeOffset), DateTimeOffset end = default(DateTimeOffset), string displayFormat = "")
        {
            // Mew mew (== Mewtwo)
            var cats = data.Categories;

            if (data.Index != null)
            {
                data.Index.Erase(data);
            }

            var dest = new DataTextDated(data.Name, data.Value, start, end, displayFormat, data.Level, data.Index, data.IsDefault);

            foreach (NonGenericElementList el in cats)
            {
                dest.AddCategory(el);
            }

            data.ClearCategories();

            return dest;
        }
    }
}