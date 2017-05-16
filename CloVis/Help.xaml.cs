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
    public sealed partial class Help : Page
    {
        public Help()
        {
            this.InitializeComponent();
        }

        private void Retour_Click(object sender, RoutedEventArgs e)
        {
            Frame root = Window.Current.Content as Frame;
            if (root == null)
                return;

            // Navigate back if possible, and if the event has not 
            // already been handled .
            if (root.CanGoBack)
            {
                root.GoBack();
            }
            else
            {
                this.Frame.Navigate(typeof(StartPage));
            }
        }
    }
}
