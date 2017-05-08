﻿using System;
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

            if (GetTemplateChild("AcceptChanges") is Button btn)
                btn.Click += AcceptChanges_Click;
            //if (GetTemplateChild("ValueEdit") is TextBox tb)
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

            // Back to View mode :
            SwitchToViewData();

            // Update values in the fields :
            OnDataChanged(this, null);

            //throw new NotImplementedException("Save Index ?");
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
                    if (instance.GetTemplateChild("DateMiddleword") is TextBox mid)
                        mid.Text = tab[3];
                    if (instance.GetTemplateChild("DateSecondField") is ComboBox dsf)
                        if (tab[4] == "")
                        {
                            dsf.SelectedIndex = 0;
                            if (instance.GetTemplateChild("DateSecond") is DatePicker ds1)
                                ds1.Visibility = Visibility.Collapsed;
                            if (instance.GetTemplateChild("DateEndword") is TextBox end1)
                                end1.Visibility = Visibility.Collapsed;
                        }
                        else
                        {
                            dsf.SelectedIndex = tab[4] == "start" ? 1 : 2;
                            if (instance.GetTemplateChild("DateSecond") is DatePicker ds)
                            {
                                ds.Visibility = Visibility.Visible;
                                ds.Date = tab[4] == "start" ? dd.StartTime : dd.EndTime;
                            }
                            if (instance.GetTemplateChild("DateEndword") is TextBox end)
                            {
                                end.Visibility = Visibility.Visible;
                                end.Text = tab[6];
                            }
                        }
                }
                else if (instance.Data is DataTimeSpan<string> dts)
                {
                    string[] tab = dts.GetDisplayFormatStructure();

                    if (instance.GetTemplateChild("TSForeword") is TextBox fore)
                        fore.Text = tab[0];
                    if (instance.GetTemplateChild("TSMiddleword") is TextBox mid)
                        mid.Text = tab[1];
                    if (instance.GetTemplateChild("TSEndword") is TextBox end)
                        end.Text = tab[2];
                }
            }
        }

        private void Data_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
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
                if (e.AddedItems.Contains("Début"))
                {
                    dp1.Date = dd.StartTime;
                }
                else if (e.AddedItems.Contains("Fin"))
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
                var visi = new Visibility();
                if (e.RemovedItems.Contains("Pas de deuxième date"))
                {
                    visi = Visibility.Visible;
                }
                else if (e.AddedItems.Contains("Pas de deuxième date"))
                {
                    visi = Visibility.Collapsed;
                }

                if (GetTemplateChild("DateEndword") is TextBox tb)
                    tb.Visibility = visi;
                if (GetTemplateChild("DateSecond") is DatePicker dp)
                    dp.Visibility = visi;

                // Change the date in the second field :
                if (GetTemplateChild("DateSecond") is DatePicker dp1)
                {
                    if (e.AddedItems.Contains("Début"))
                    {
                        dp1.Date = dd.StartTime;
                    }
                    else if (e.AddedItems.Contains("Fin"))
                    {
                        dp1.Date = dd.EndTime;
                    }
                }
            }
        }
    }
}
