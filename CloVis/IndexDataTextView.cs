using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using ResumeElements;

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CloVis
{
    public sealed class IndexDataTextView : Control
    {
        public IndexDataTextView()
        {
            this.DefaultStyleKey = typeof(IndexDataTextView);
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
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

            KeyDown += AcceptChanges_KeyDown;

            if (GetTemplateChild("DateFirstField") is ComboBox cb)
                cb.SelectionChanged += DataFirst_SelectionChanged;
            if (GetTemplateChild("DateSecondField") is ComboBox cb1)
                cb1.SelectionChanged += DataSecond_SelectionChanged;
        }

        public ResumeElements.Data<string> Data
        {
            get => (ResumeElements.Data<string>)(GetValue(DataProperty));
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ResumeElements.Data<string>), typeof(IndexDataTextView), new PropertyMetadata(null, OnDataChanged));

        public void SwitchToEditData()
        {
            if (GetTemplateChild("DataView") is StackPanel sp)
                sp.Visibility = Visibility.Collapsed;
            if (GetTemplateChild("DataEdit") is StackPanel sp1)
                sp1.Visibility = Visibility.Visible;
        }
        public void SwitchToViewData()
        {
            if (GetTemplateChild("DataEdit") is StackPanel sp)
                sp.Visibility = Visibility.Collapsed;
            if (GetTemplateChild("DataView") is StackPanel sp1)
                sp1.Visibility = Visibility.Visible;
        }
        public void AcceptChanges()
        {
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

            // Back to View mode :
            SwitchToViewData();

            // Update values in the fields :
            OnDataChanged(this, null);

            //throw new NotImplementedException("Save Index ?");
        }
        public static void ShowSecondDate(IndexDataTextView instance)
        {
            if (instance.GetTemplateChild("DateSecond") is DatePicker ds)
                ds.Visibility = Visibility.Visible;
            if (instance.GetTemplateChild("DateSecondFormat") is TextBox form)
                form.Visibility = Visibility.Visible;
            if (instance.GetTemplateChild("DateSecondFormatName") is TextBlock txt)
                txt.Visibility = Visibility.Visible;
            if (instance.GetTemplateChild("DateEndword") is TextBox end)
                end.Visibility = Visibility.Visible;
        }
        public static void HideSecondDate(IndexDataTextView instance)
        {
            if (instance.GetTemplateChild("DateSecond") is DatePicker ds)
                ds.Visibility = Visibility.Collapsed;
            if (instance.GetTemplateChild("DateSecondFormat") is TextBox form)
                form.Visibility = Visibility.Collapsed;
            if (instance.GetTemplateChild("DateSecondFormatName") is TextBlock txt)
                txt.Visibility = Visibility.Collapsed;
            if (instance.GetTemplateChild("DateEndword") is TextBox end)
                end.Visibility = Visibility.Collapsed;
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexDataTextView instance && instance.Data != null)
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
            }
        }

        private void Data_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            SwitchToEditData();
        }
        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            SwitchToEditData();
        }

        private void AcceptChanges_Click(object sender, RoutedEventArgs e)
        {
            AcceptChanges();
        }

        private void AcceptChanges_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                AcceptChanges();
        }

        private void DataFirst_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Change the date in the first field :
            if (Data is DataDated<string> dd && GetTemplateChild("DateFirst") is DatePicker dp1)
            {
                if (e.AddedItems.Contains((sender as ComboBox).Items[0]))
                {
                    dp1.Date = dd.StartTime;
                }
                else if (e.AddedItems.Contains((sender as ComboBox).Items[1]))
                {
                    dp1.Date = dd.EndTime;
                }
            }
        }

        private void DataSecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Data is DataDated<string> dd)
            {
                // Hide or show the last fields :
                if (e.RemovedItems.Contains((sender as ComboBox).Items[0]))
                {
                    ShowSecondDate(this);
                }
                else if (e.AddedItems.Contains((sender as ComboBox).Items[0]))
                {
                    HideSecondDate(this);
                }

                // Change the date in the second field :
                if (GetTemplateChild("DateSecond") is DatePicker dp1)
                {
                    if (e.AddedItems.Contains((sender as ComboBox).Items[1]))
                    {
                        dp1.Date = dd.StartTime;
                    }
                    else if (e.AddedItems.Contains((sender as ComboBox).Items[2]))
                    {
                        dp1.Date = dd.EndTime;
                    }
                }
            }
        }
    }
}
