using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using ResumeElements;
using Resume;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CloVis
{
    public class IsTimeSpanToVisibilityConverter: IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
           if (value is string str && Index.Find(str) is DataTimeSpan<string> dts)
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
            if (value is string str && Index.Find(str) is DataDated<string> dts)
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
            if (value is string str && (Index.Find(str) is DataTimeSpan<string> dts || Index.Find(str) is DataDated<string> dd))
                return Visibility.Collapsed;
            else
                return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class AddComaConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s)
                return s + ", ";
            else
                return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToVisibilityConverter: IValueConverter
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

    public class TimeSpanDisplayFormatToTextConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string s && parameter is string si && int.TryParse(si, out int i))
                return (DataTimeSpan<string>.GetDisplayFormatStructure(s))[i];
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
                return (DataDated<string>.GetDisplayFormatStructure(s))[i];
            else
                return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
    
    public sealed partial class IndexDataTextView : UserControl, INotifyPropertyChanged
    {
        public IndexDataTextView()
        {
            this.InitializeComponent();
        }

        public ResumeElements.Data<string> Data
        {
            get => (ResumeElements.Data<string>)(GetValue(DataProperty));
            set
            {
                SetValue(DataProperty, value);
                NotifyPropertyChanged("Data");
            }
        }
        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexDataTextView instance && instance.Data != null)
            {
                instance.NotifyPropertyChanged("Data");
            }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ResumeElements.Data<string>), typeof(IndexDataTextView), new PropertyMetadata(null, OnDataChanged));

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SwitchToEditData()
        {
            DataView.Visibility = Visibility.Collapsed;
            DataEdit.Visibility = Visibility.Visible;
            // Update fields if it is a DataDated :
            if (Data is DataDated<string> dd)
            {
                var stru = dd.GetDisplayFormatStructure();
                DateForeword.Text = stru[0];
                DateMiddleword.Text = stru[3];
                DateEndword.Text = stru[6];

                DateFirstFormat.Text = stru[2];
                DateSecondFormat.Text = stru[5];

                if (stru[1] == "start")
                {
                    DateFirst.Date = dd.StartTime;
                    DateFirstField.SelectedIndex = 0;
                }
                else if (stru[1] == "end")
                {
                    DateFirst.Date = dd.EndTime;
                    DateFirstField.SelectedIndex = 1;
                }

                if (stru[4] == "start")
                {
                    DateSecond.Date = dd.StartTime;
                    DateSecondField.SelectedIndex = 1;
                    ShowSecondDate();

                }
                else if (stru[4] == "end")
                {
                    DateSecond.Date = dd.EndTime;
                    DateSecondField.SelectedIndex = 2;
                    ShowSecondDate();
                }
                else
                {
                    DateSecondField.SelectedIndex = 0;
                    HideSecondDate();
                }
            }
        }
        public void SwitchToViewData()
        {
            DataView.Visibility = Visibility.Visible;
            DataEdit.Visibility = Visibility.Collapsed;
        }
        public void AcceptEditChanges()
        {
            Data.Value = ValueEdit.Text;
            if (Data is DataDated<string> dd)
            {
                var temp = DateForeword.Text + "$";

                if (DateFirstField.SelectedIndex == 0)
                {
                    dd.StartTime = DateFirst.Date;
                    temp += "1";
                }
                else
                {
                    dd.EndTime = DateFirst.Date;
                    temp += "2";
                }

                temp += "(" + DateFirstFormat.Text + ")$" + DateMiddleword.Text;

                if (DateSecondField.SelectedIndex != 0)
                {
                    temp += "$";
                    if (DateSecondField.SelectedIndex == 1)
                    {
                        dd.StartTime = DateSecond.Date;
                        temp += "1";
                    }
                    else
                    {
                        dd.EndTime = DateSecond.Date;
                        temp += "2";
                    }

                    temp += "(" + DateSecondFormat.Text + ")$" + DateEndword.Text;
                }

                dd.DisplayFormat = temp;
            }
            else if (Data is DataTimeSpan<string> dts)
            {
                if (int.TryParse(TSMiddleword.Text, out int i))
                    dts.TimeSpan = new TimeSpan(i, 0, 0, 0);

                dts.DisplayFormat = TSForeword.Text + "$(" + dts.GetDisplayFormat() + ")$" + TSEndword.Text;
            }
            
            // Back to View mode :
            SwitchToViewData();

            // Update values in the fields :
            NotifyPropertyChanged("Data");
        }
        public void CancelEditChanges()
        {
            SwitchToViewData();
            NotifyPropertyChanged("Data");
        }
        public void ShowSecondDate()
        {
            DateSecond.Visibility = Visibility.Visible;
            DateSecondFormat.Visibility = Visibility.Visible;
            DateSecondFormatName.Visibility = Visibility.Visible;
            DateEndword.Visibility = Visibility.Visible;
        }
        public void HideSecondDate()
        {
            DateSecond.Visibility = Visibility.Collapsed;
            DateSecondFormat.Visibility = Visibility.Collapsed;
            DateSecondFormatName.Visibility = Visibility.Collapsed;
            DateEndword.Visibility = Visibility.Collapsed;
        }
        public void ShowCategories()
        {
            CategoriesStack.Visibility = Visibility.Visible;
            ToggleCategories.IsChecked = true;
        }
        public void HideCategories()
        {
            CategoriesStack.Visibility = Visibility.Collapsed;
            ToggleCategories.IsChecked = false;
        }

        private void Data_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            SwitchToEditData();
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            SwitchToEditData();
            HideCategories();
        }

        private void AcceptChanges_Click(object sender, RoutedEventArgs e)
        {
            AcceptEditChanges();
        }
        private void CancelChanges_Click(object sender, RoutedEventArgs e)
        {
            CancelEditChanges();
        }
        private void AcceptChanges_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                AcceptEditChanges();
            else if (e.Key == Windows.System.VirtualKey.Escape)
                CancelEditChanges();
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string str && Index.Find(str) is ElementList el)
            {
                Data.RemoveCategory(el);
            }
        }

        private void DateFirst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change the date in the first field :
            if (Data is DataDated<string> dd)
            {
                if (e.AddedItems.Contains((sender as ComboBox).Items[0]))
                {
                    DateFirst.Date = dd.StartTime;
                }
                else if (e.AddedItems.Contains((sender as ComboBox).Items[1]))
                {
                    DateFirst.Date = dd.EndTime;
                }
            }
        }

        private void DateSecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Data is DataDated<string> dd)
            {
                // Hide or show the last fields :
                if (e.AddedItems.Contains((sender as ComboBox).Items[0]))
                {
                    HideSecondDate();
                }
                else
                {
                    ShowSecondDate();
                }

                // Change the date in the second field :
                if (e.AddedItems.Contains((sender as ComboBox).Items[1]))
                {
                    DateSecond.Date = dd.StartTime;
                }
                else if (e.AddedItems.Contains((sender as ComboBox).Items[2]))
                {
                    DateSecond.Date = dd.EndTime;
                }
            }
        }
    }
}
