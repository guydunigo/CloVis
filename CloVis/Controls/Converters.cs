
using ResumeElements;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace CloVis.Controls
{

    public class IsTimeSpanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str && Deprecated_Index.Find(str) is Deprecated_DataTimeSpan<string> dts)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class IsDatedToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str && Deprecated_Index.Find(str) is Deprecated_DataDated<string> dts)
                return Visibility.Visible;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class IsSimpleToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string str && (Deprecated_Index.Find(str) is Deprecated_DataTimeSpan<string> dts || Deprecated_Index.Find(str) is Deprecated_DataDated<string> dd))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class AddSeparatorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s)
                return " - " + s;
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b)
                return b ? Visibility.Visible : Visibility.Collapsed;
            else
                return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class TimeSpanDisplayFormatToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s && parameter is string si && int.TryParse(si, out int i))
                return (Deprecated_DataTimeSpan<string>.GetDisplayFormatStructure(s))[i];
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class TimeSpanValueToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is TimeSpan ts)
                return ts.Days;
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class DatedDisplayFormatToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s && parameter is string si && int.TryParse(si, out int i))
                return (Deprecated_DataDated<string>.GetDisplayFormatStructure(s))[i];
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class CollectionToObservableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IEnumerable<Element> ic)
                return new ObservableCollection<Element>(ic);
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    public class ResumeCollectionToObservableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IEnumerable<Resume.Resume> ic)
                return new ObservableCollection<Resume.Resume>(ic);
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    internal class ElementComparer : IComparer<Element>
    {
        public int Compare(Element x, Element y)
        {
            return x.Name.CompareTo(y.Name);
        }
    }
    public class CollectionToSortedObservableConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is IEnumerable<Element> ic)
            {
                var temp = new SortedSet<Element>(new ElementComparer());
                foreach (Element e in ic)
                {
                    temp.Add(e);
                }

                return new ObservableCollection<Element>(temp);
            }
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class SubElementTemplateSelector : DataTemplateSelector
    {
        public DataTemplate TemplateForData { get; set; }
        public DataTemplate TemplateForElementList { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            if (item is Deprecated_Data<string> d)
                return TemplateForData;
            else if (item is Deprecated_ElementList el)
                return TemplateForElementList;

            return null;
        }
    }

    public class RootCategoryRemoveToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s && Deprecated_Index.Root.ContainsKey(s))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class IsElmtNameInCVToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s && (Window.Current.Content as Frame).Content is EditionMode page)
            {
                var cv = page.Resume.LocalIndex;
                if (cv.Find(s) != null)
                {
                    return true;
                }
                else
                    return false;
            }
            else
                return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}