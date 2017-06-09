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
using ResumeStructure;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CloVis.Controls
{
    public sealed partial class IndexDataTextView : UserControl, INotifyPropertyChanged
    {
        public IndexDataTextView()
        {
            this.InitializeComponent();
        }

        public ResumeElements.Deprecated_Data<string> Data
        {
            get => (ResumeElements.Deprecated_Data<string>)(GetValue(DataProperty));
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
                if (instance.Data.Value == "")
                    instance.SwitchToEditData();
                instance.NotifyPropertyChanged("Data");
            }
        }
        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ResumeElements.Deprecated_Data<string>), typeof(IndexDataTextView), new PropertyMetadata(null, OnDataChanged));

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
            if (Data is Deprecated_DataDated<string> dd)
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

        public void AlertEmptyField(FrameworkElement fe)
        {
            ValueEdit.BorderBrush = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;

            var fo = new Flyout()
            {
                Content = new TextBlock()
                {
                    Text = "Veuillez renseigner une valeur.",
                    Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush
                }
            };
            fo.ShowAt(fe);
        }
        public void ResetAlertEmptyField()
        {
            ValueEdit.BorderBrush = new SolidColorBrush(Windows.UI.Colors.Gray);
        }

        public void AcceptEditChanges(object sender = null)
        {
            if (ValueEdit.Text != "")
            {
                Data.Value = ValueEdit.Text;
                if (Data is Deprecated_DataDated<string> dd)
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
                else if (Data is Deprecated_DataTimeSpan<string> dts)
                {
                    if (int.TryParse(TSMiddleword.Text, out int i))
                        dts.TimeSpan = new TimeSpan(i, 0, 0, 0);

                    dts.DisplayFormat = Deprecated_DataTimeSpan<string>.GenerateDisplayFormat(TSForeword.Text, TSEndword.Text, dts.GetDisplayFormat());
                }

                // Back to View mode :
                SwitchToViewData();

                ResetAlertEmptyField();

                // Update values in the fields :
                NotifyPropertyChanged("Data");
            }
            else if (sender is FrameworkElement fe)
            {
                AlertEmptyField(fe);
            }
        }
        public void CancelEditChanges(object sender = null)
        {
            if (Data.Value != "")
            {
                SwitchToViewData();

                ResetAlertEmptyField();

                // Even if we didn't change anything, notify :
                NotifyPropertyChanged("Data");
            }
            else if (sender is FrameworkElement fe)
                AlertEmptyField(fe);
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
            AcceptEditChanges(sender);
        }
        private void CancelChanges_Click(object sender, RoutedEventArgs e)
        {
            CancelEditChanges(sender);
        }
        private void AcceptChanges_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                AcceptEditChanges(sender);
            else if (e.Key == Windows.System.VirtualKey.Escape)
                CancelEditChanges(sender);
        }

        private void RemoveCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string str && Deprecated_Index.Find(str) is Deprecated_ElementList el)
            {
                Data.RemoveCategory(el);
            }
        }

        private void DateFirst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change the date in the first field :
            if (Data is Deprecated_DataDated<string> dd)
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
            if (Data is Deprecated_DataDated<string> dd)
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

        private void RemoveBtn_Click(object sender, RoutedEventArgs e)
        {
            Deprecated_Index.Erase(Data);
        }

        private void AddDate_Click(object sender, RoutedEventArgs e)
        {
            if (!(Data is Deprecated_DataDated<string>) && !(Data is Deprecated_DataTimeSpan<string>))
            {
                Data = Deprecated_DataDated<string>.Replace(Data);
            }
        }

        private void AddTimeSpan_Click(object sender, RoutedEventArgs e)
        {
            if (!(Data is Deprecated_DataDated<string>) && !(Data is Deprecated_DataTimeSpan<string>))
            {
                Data = Deprecated_DataTimeSpan<string>.Replace(Data);
            }
        }
    }
}
