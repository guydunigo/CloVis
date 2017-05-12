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
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            /*
            OnDataChanged(this, null);
            if (Data is DataDated<string> dts)
            {
                if (GetTemplateChild("TimeView") is StackPanel sp0)
                    sp0.Visibility = Visibility.Visible;
                if (GetTemplateChild("DateEdit") is StackPanel sp1)
                    sp1.Visibility = Visibility.Visible;
            }
            else if (Data is DataTimeSpan<string> dts1)
            {
                if (GetTemplateChild("TimeView") is StackPanel sp2)
                    sp2.Visibility = Visibility.Visible;
                if (GetTemplateChild("TimeSpanEdit") is StackPanel sp3)
                    sp3.Visibility = Visibility.Visible;
            }

            if (GetTemplateChild("DataView") is StackPanel sp)
                sp.DoubleTapped += Data_DoubleTapped;
            if (GetTemplateChild("EditBtn") is Button ebtn)
                ebtn.Click += EditBtn_Click;

            if (GetTemplateChild("AcceptChanges") is Button btn)
                btn.Click += AcceptChanges_Click;
            if (GetTemplateChild("CancelChanges") is Button btnc)
                btnc.Click += CancelChanges_Click;

            KeyDown += AcceptChanges_KeyDown;

            if (GetTemplateChild("DateFirstField") is ComboBox cb)
                cb.SelectionChanged += DataFirst_SelectionChanged;
            if (GetTemplateChild("DateSecondField") is ComboBox cb1)
                cb1.SelectionChanged += DataSecond_SelectionChanged;

            if (GetTemplateChild("ToggleCategories") is Windows.UI.Xaml.Controls.Primitives.ToggleButton tb)
            {
                tb.Checked += ToggleCategories_Checked;
                tb.Unchecked += ToggleCategories_Unchecked;
            }

            if (GetTemplateChild("Categories") is ListView lv)
            {
            }
            */
        }

        public ResumeElements.Data<string> Data
        {
            get => (ResumeElements.Data<string>)(GetValue(DataProperty));
            set
            {
                SetValue(DataProperty, value);
                NotifyPropertyChanged();
            }
        }
        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexDataTextView instance && instance.Data != null)
            {
                instance.NotifyPropertyChanged("Data");
            }
            /*
            if (d is MyUserControl1 instance && instance.Data != null)
            {
                instance.DataContext = typeof(ResumeElements.Data<string>);
                if (instance.GetTemplateChild("NameView") is TextBlock name)
                    name.Text = instance.Data.Name;
                if (instance.GetTemplateChild("ValueView") is TextBlock val)
                    val.Text = instance.Data.Value;
                if (instance.GetTemplateChild("TimeView") is TextBlock time)
                {
                    if (instance.Data is DataDated<string> dd1)
                    {
                        time.Visibility = Visibility.Visible;
                        time.Text = dd1.RenderDates() + ", ";
                    }
                    else if (instance.Data is DataTimeSpan<string> dts1)
                    {
                        time.Visibility = Visibility.Visible;
                        time.Text = dts1.RenderTimeSpan() + ", ";
                    }
                    else
                        time.Visibility = Visibility.Collapsed;
                }
                if (instance.GetTemplateChild("NameEdit") is TextBlock name1)
                    name1.Text = instance.Data.Name;
                if (instance.GetTemplateChild("ValueEdit") is TextBox val1)
                    val1.Text = instance.Data.Value;
                if (instance.Data is DataDated<string> dd)
                {
                    string[] tab = dd.GetDisplayFormatStructure();

                    if (instance.GetTemplateChild("DateForeword") is TextBox fore)
                        fore.Text = tab[0];
                    if (instance.GetTemplateChild("DateFirstField") is ComboBox dff)
                        dff.SelectedIndex = tab[1] == "start" ? 0 : 1;
                    if (instance.GetTemplateChild("DateFirst") is DatePicker df)
                        df.Date = tab[1] == "start" ? dd.StartTime : dd.EndTime;
                    if (instance.GetTemplateChild("DateFirstFormat") is TextBox form)
                        form.Text = tab[2];
                    if (instance.GetTemplateChild("DateMiddleword") is TextBox mid)
                        mid.Text = tab[3];
                    if (instance.GetTemplateChild("DateSecondField") is ComboBox dsf)
                        if (tab[4] == "")
                        {
                            dsf.SelectedIndex = 0;
                            HideSecondDate(instance);
                            if (instance.GetTemplateChild("DateSecondFormat") is TextBox form1)
                                form1.Text = tab[2]; // Completes automatically the field with the same format as the first one
                        }
                        else
                        {
                            dsf.SelectedIndex = tab[4] == "start" ? 1 : 2;
                            ShowSecondDate(instance);
                            if (instance.GetTemplateChild("DateSecond") is DatePicker ds)
                                ds.Date = tab[4] == "start" ? dd.StartTime : dd.EndTime;
                            if (instance.GetTemplateChild("DateSecondFormat") is TextBox form1)
                                form1.Text = tab[5];
                            if (instance.GetTemplateChild("DateEndword") is TextBox end)
                                end.Text = tab[6];
                        }
                }
                else if (instance.Data is DataTimeSpan<string> dts)
                {
                    string[] tab = dts.GetDisplayFormatStructure();

                    if (instance.GetTemplateChild("TSForeword") is TextBox fore)
                        fore.Text = tab[0];
                    if (instance.GetTemplateChild("TSMiddleword") is TextBox mid)
                        mid.Text = dts.TimeSpan.ToString(tab[1]);
                    if (instance.GetTemplateChild("TSEndword") is TextBox end)
                        end.Text = tab[2];
                }

                if (instance.GetTemplateChild("Categories") is ListView lv)
                {
                    lv.ItemsSource = instance.Data.Categories;
                }
            }
            */
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ResumeElements.Data<string>), typeof(IndexDataTextView_old), new PropertyMetadata(null, OnDataChanged));

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void SwitchToEditData()
        {
            DataView.Visibility = Visibility.Collapsed;
            DataEdit.Visibility = Visibility.Visible;

            /*if (GetTemplateChild("DataView") is StackPanel sp)
                sp.Visibility = Visibility.Collapsed;
            if (GetTemplateChild("DataEdit") is StackPanel sp1)
                sp1.Visibility = Visibility.Visible;*/
        }
        public void SwitchToViewData()
        {
            DataView.Visibility = Visibility.Visible;
            DataEdit.Visibility = Visibility.Collapsed;

            /*if (GetTemplateChild("DataEdit") is StackPanel sp)
                sp.Visibility = Visibility.Collapsed;
            if (GetTemplateChild("DataView") is StackPanel sp1)
                sp1.Visibility = Visibility.Visible;*/
        }
        public void AcceptEditChanges()
        {
            Data.Value = ValueEdit.Text;
            if (Data is DataDated<string> dd)
            {
                var temp = DateForeword.Text + "$";

                if ((DateFirstField.SelectedItem as String) == (DateFirstField.Items[0] as string))
                {
                    dd.StartTime = DateFirst.Date;
                    temp += "1";
                }
                else
                {
                    dd.EndTime = DateFirst.Date;
                    temp += "2";
                }

                temp += "(" + DateFirstFormat.Text + ")$" + DateMiddleword;

                if ((DateSecondField.SelectedItem as String) == (DateSecondField.Items[0] as string))
                {
                    temp += "$";
                    if ((DateSecondField.SelectedItem as String) == (DateSecondField.Items[1] as string))
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

            //throw new NotImplementedException("Save Index ?");

            /*
            if (GetTemplateChild("ValueEdit") is TextBox tb)
                Data.Value = tb.Text;
            if (Data is DataDated<string> dd)
            {
                if (GetTemplateChild("DateFirstField") is ComboBox cb && GetTemplateChild("DateFirst") is DatePicker dp)
                {
                    if ((cb.SelectedItem as String) == (cb.Items[0] as string))
                        dd.StartTime = dp.Date;
                    else
                        dd.EndTime = dp.Date;
                }
                if (GetTemplateChild("DateSecondField") is ComboBox cb1 && GetTemplateChild("DateSecond") is DatePicker dp1)
                {
                    if ((cb1.SelectedItem as String) == (cb1.Items[1] as string))
                        dd.StartTime = dp1.Date;
                    else
                        dd.EndTime = dp1.Date;
                }
                if (GetTemplateChild("DateForeword") is TextBox fore &&
                        GetTemplateChild("DateFirstField") is ComboBox first &&
                        GetTemplateChild("DateFirstFormat") is TextBox fform &&
                        GetTemplateChild("DateMiddleword") is TextBox mid &&
                        GetTemplateChild("DateSecondField") is ComboBox second &&
                        GetTemplateChild("DateSecondFormat") is TextBox sform &&
                        GetTemplateChild("DateEndword") is TextBox end)
                {
                    var temp = fore.Text
                        + "$" + ((first.SelectedItem as string) == (first.Items[0] as string) ? "1" : "2") + "("
                        + fform.Text + ")$"
                        + mid.Text;
                    if (second.SelectedItem is string s && s != (second.Items[0] as string))
                    {
                        temp += "$" + ((second.SelectedItem as string) == (second.Items[1] as string) ? "1" : "2") + "("
                            + sform.Text + ")$"
                            + end.Text;
                    }

                    dd.DisplayFormat = temp;
                }
            }
            else if (Data is DataTimeSpan<string> dts)
            {
                if (GetTemplateChild("TSMiddleword") is TextBox tsmid)
                {
                    try
                    {
                        dts.TimeSpan = new TimeSpan(int.Parse(tsmid.Text), 0, 0, 0);
                    }
                    catch (FormatException)
                    {
                        // If it can't convert the Int, the user didn't enter a number so we don't updat ethe value.
                    }
                }
                if (GetTemplateChild("TSForeword") is TextBox tsfore &&
                        GetTemplateChild("TSEndword") is TextBox tsend)
                {
                    dts.DisplayFormat = tsfore.Text + "$(" + dts.GetDisplayFormat() + ")$" + tsend.Text;
                }
            }

            // Back to View mode :
            SwitchToViewData();

            // Update values in the fields :
            OnDataChanged(this, null);

            //throw new NotImplementedException("Save Index ?");
            */
        }
        public void CancelEditChanges()
        {
            SwitchToViewData();
            NotifyPropertyChanged("Data");
        }
        public /*static*/ void ShowSecondDate(/*MyUserControl1 instance*/)
        {
            DateSecond.Visibility = Visibility.Visible;
            DateSecondFormat.Visibility = Visibility.Visible;
            DateSecondFormatName.Visibility = Visibility.Visible;
            DateEndword.Visibility = Visibility.Visible;
            /*
            if (instance.GetTemplateChild("DateSecond") is DatePicker ds)
                ds.Visibility = Visibility.Visible;
            if (instance.GetTemplateChild("DateSecondFormat") is TextBox form)
                form.Visibility = Visibility.Visible;
            if (instance.GetTemplateChild("DateSecondFormatName") is TextBlock txt)
                txt.Visibility = Visibility.Visible;
            if (instance.GetTemplateChild("DateEndword") is TextBox end)
                end.Visibility = Visibility.Visible;
                */
        }
        public /*static*/ void HideSecondDate(/*MyUserControl1 instance*/)
        {
            DateSecond.Visibility = Visibility.Collapsed;
            DateSecondFormat.Visibility = Visibility.Collapsed;
            DateSecondFormatName.Visibility = Visibility.Collapsed;
            DateEndword.Visibility = Visibility.Collapsed;
            /*
            if (instance.GetTemplateChild("DateSecond") is DatePicker ds)
                ds.Visibility = Visibility.Collapsed;
            if (instance.GetTemplateChild("DateSecondFormat") is TextBox form)
                form.Visibility = Visibility.Collapsed;
            if (instance.GetTemplateChild("DateSecondFormatName") is TextBlock txt)
                txt.Visibility = Visibility.Collapsed;
            if (instance.GetTemplateChild("DateEndword") is TextBox end)
                end.Visibility = Visibility.Collapsed;
                */
        }
        public void ShowCategories()
        {
            CategoriesStack.Visibility = Visibility.Visible;
            ToggleCategories.IsChecked = true;
            /*
            if (GetTemplateChild("CategoriesStack") is StackPanel sp)
                sp.Visibility = Visibility.Visible;
            if (GetTemplateChild("ToggleCategories") is Windows.UI.Xaml.Controls.Primitives.ToggleButton tb)
                tb.IsChecked = true;
                */
        }
        public void HideCategories()
        {
            CategoriesStack.Visibility = Visibility.Collapsed;
            ToggleCategories.IsChecked = false;
            /*
            if (GetTemplateChild("CategoriesStack") is StackPanel sp)
                sp.Visibility = Visibility.Collapsed;
            if (GetTemplateChild("ToggleCategories") is Windows.UI.Xaml.Controls.Primitives.ToggleButton tb)
                tb.IsChecked = false;
                */
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
                Data.RemoveCategory(el);
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
                if (e.RemovedItems.Contains((sender as ComboBox).Items[0]))
                {
                    ShowSecondDate();
                }
                else if (e.AddedItems.Contains((sender as ComboBox).Items[0]))
                {
                    HideSecondDate();
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
