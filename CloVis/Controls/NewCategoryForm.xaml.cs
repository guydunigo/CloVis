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
    public sealed partial class NewCategoryForm : UserControl
    {
        public ElementList IndexRoot { get; set; }

        public NewCategoryForm()
        {
            this.InitializeComponent();
            IndexRoot = Index.Root;
        }

        public void Validate(object sender)
        {
            TextBlock txt = new TextBlock();

            if (CatName.Text == "")
            {
                txt.Text = "Veuillez renseigner un nom.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (Index.Find(CatName.Text) != null)
            {
                txt.Text = "Ce nom est déjà utilisé";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (CatList.SelectedIndex == -1)
            {
                txt.Text = "Selectionnez une catégorie de destination.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (CatList.SelectedItem is ElementList el)
            {
                el.Add(new ElementList<Element>(CatName.Text));

                CatName.Text = "";

                txt.Text = "Catégorie ajoutée à " + el.Name + ".";
            }

            var fo = new Flyout()
            {
                Content = txt
            };
            fo.ShowAt(CatName);
        }

        private void CatAdd_Click(object sender, RoutedEventArgs e)
        {
            Validate(sender);
        }

        private void CatName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                Validate(sender);
        }
    }
}
