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

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=234238

namespace CloVis
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class DetailsEdit : Page
    {
        public DetailsEdit()
        {
            this.InitializeComponent();
        }

        private void SymbolIcon_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var temp = sender as SymbolIcon;
            if (temp.Symbol == Symbol.Add)
                temp.Foreground = Application.Current.Resources["CloVisBlue"] as SolidColorBrush;
        }

        private void SymbolIcon_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var temp = sender as SymbolIcon;
            if (temp.Symbol == Symbol.Add)
                temp.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);
        }

        private void SymbolIcon_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            var temp = sender as SymbolIcon;
            if (temp.Symbol == Symbol.Add)
            {
                temp.Symbol = Symbol.Remove;
                temp.Foreground = Application.Current.Resources["CloVisBlue"] as SolidColorBrush;

                ((temp.Parent as StackPanel).Parent as StackPanel).Children[1].Visibility = Visibility.Visible;
            }
            else if (temp.Symbol == Symbol.Remove)
            {
                temp.Symbol = Symbol.Add;
                temp.Foreground = new SolidColorBrush(Windows.UI.Colors.Black);

                ((temp.Parent as StackPanel).Parent as StackPanel).Children[1].Visibility = Visibility.Collapsed;
            }
            //throw new NotImplementedException("show sublist");
        }

        private void ElementList_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            test.Items.Add(new ListViewItem() { Content = new IndexElementListView() { ElementList = ResumeElements.Index.Root["Coordonnées"] } });
            test.Items.Add(new ListViewItem() { Content = new IndexDataTextView() { Data = ResumeElements.Index.Find("Mél") as ResumeElements.Data<string> } });

            //throw new NotImplementedException();
        }

        private void Data_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Element_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Element_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
