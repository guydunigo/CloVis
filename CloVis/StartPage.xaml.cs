using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class StartPage : Page
    {
        private ObservableCollection<NavLink> topNavLinks = new ObservableCollection<NavLink>()
        {
            new NavLink() { Symbol= Windows.UI.Xaml.Controls.Symbol.OpenFile , Label="Ouvrir / Importer", Tag="OpenFile" },
            new NavLink() { Symbol= Windows.UI.Xaml.Controls.Symbol.Edit , Label="Modifiez vos données", Tag="DetailsEdit" },
        };
        public ObservableCollection<NavLink> TopNavLinks { get => topNavLinks; }

        private ObservableCollection<NavLink> bottomNavLinks = new ObservableCollection<NavLink>()
        {
            new NavLink() { Symbol= Windows.UI.Xaml.Controls.Symbol.Setting , Label="Paramètres", Tag="Settings" },
            new NavLink() { Symbol= Windows.UI.Xaml.Controls.Symbol.Help , Label="Aide", Tag="Help" },
        };
        public ObservableCollection<NavLink> BottomNavLinks { get => bottomNavLinks; }

        public StartPage()
        {
            this.InitializeComponent();
        }

        private void NavLinkList_ItemClick(object sender, ItemClickEventArgs e)
        {
            throw new NotImplementedException();
        }
    }

    public class NavLink
    {
        public string Label { get; set; }
        public Symbol Symbol { get; set; }
        public string Tag { get; set; }
    }
}
