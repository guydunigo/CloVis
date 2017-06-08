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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CloVis.Controls
{
    public sealed partial class NewDataForm : UserControl
    {
        public Deprecated_ElementList IndexRoot { get; set; }

        public NewDataForm()
        {
            this.InitializeComponent();
            this.Loaded += NewDataForm_Loaded;
            IndexRoot = Deprecated_Index.Root;
        }

        private void NewDataForm_Loaded(object sender, RoutedEventArgs e)
        {
            HideSecondDate();
        }

        public void Validate(object sender)
        {
            TextBlock txt = new TextBlock();

            if (ElmtValue.Text == "")
            {
                txt.Text = "Veuillez renseigner un nom.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (Deprecated_Index.Find(ElmtValue.Text) != null)
            {
                txt.Text = "Ce nom est déjà utilisé";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (CatList.SelectedIndex == -1)
            {
                txt.Text = "Selectionnez une catégorie de destination.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (CatList.SelectedItem is Deprecated_ElementList el)
            {
                if (DateBtn.IsChecked == true)
                    el.Add(new Deprecated_DataDated<string>(ElmtValue.Text, DateFirst.Date, DateSecond.Date,
                        Deprecated_DataDated<string>.GenerateDisplayFormat(DateForeword.Text, DateFirstField.SelectedIndex + 1, DateFirstFormat.Text, DateMiddleword.Text, DateSecondField.SelectedIndex, DateSecondFormat.Text, DateEndword.Text)));
                else if (TimeSpanBtn.IsChecked == true)
                {
                    el.Add(new Deprecated_DataTimeSpan<string>(ElmtValue.Text, new TimeSpan(int.Parse(TSMiddleword.Text), 0, 0, 0), Deprecated_DataTimeSpan<string>.GenerateDisplayFormat(TSForeword.Text, TSEndword.Text)));
                }
                else
                    el.Add(new Deprecated_Data<string>(ElmtValue.Text));
                ElmtValue.Text = "";
                DateBtn.IsChecked = false;
                TimeSpanBtn.IsChecked = false;

                txt.Text = "Catégorie ajoutée à " + el.Name + ".";
            }

            var fo = new Flyout()
            {
                Content = txt
            };
            fo.ShowAt(ElmtValue);
        }

        private void ElmtAdd_Click(object sender, RoutedEventArgs e)
        {
            Validate(sender);
        }

        private void ElmtValue_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                Validate(sender);
        }

        private void AddDates_Click(object sender, RoutedEventArgs e)
        {
            Validate(sender);
        }

        private void AddTimeSpan_Click(object sender, RoutedEventArgs e)
        {
            Validate(sender);
        }

        private void DateSecond_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            if (DateSecond != null && DateSecondFormat != null && DateSecondFormatName != null && DateEndword != null)
            {
                DateSecond.Visibility = Visibility.Collapsed;
                DateSecondFormat.Visibility = Visibility.Collapsed;
                DateSecondFormatName.Visibility = Visibility.Collapsed;
                DateEndword.Visibility = Visibility.Collapsed;
            }
        }

        public void DateTimeSpanToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (sender == DateBtn)
                TimeSpanBtn.IsChecked = false;
            else if (sender == TimeSpanBtn)
                DateBtn.IsChecked = false;
        }
    }
}
