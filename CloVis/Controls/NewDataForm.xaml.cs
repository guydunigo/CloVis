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
        public NewDataForm()
        {
            this.InitializeComponent();
        }

        public void Validate(object sender)
        {
            TextBlock txt = new TextBlock();

            if (ElmtValue.Text == "")
            {
                txt.Text = "Veuillez renseigner un nom.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (Index.Find(ElmtValue.Text) != null)
            {
                txt.Text = "Ce nom est déjà utilisé";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (Index.Find("Divers") is ElementList el)
            {
                el.Add(new Data<string>(ElmtValue.Text));
                ElmtValue.Text = "";

                txt.Text = "Catégorie ajoutée à Divers.";
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
    }
}
