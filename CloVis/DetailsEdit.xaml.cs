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
            this.Loaded += OnLoaded;
        }
        private MyUserControl1 test;
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            // List elements in hierarchy
            foreach(ResumeElements.ElementList el in ResumeElements.Index.Root.Values)
                IndexList.Items.Add(new ListViewItem() { Content = new IndexElementListView() { ElementList = el } });

            // If some data is not listed anywhere, show the miscellaneous category
            var divers = ResumeElements.Index.GetMiscellaneous();
            if (divers.Count > 0)
                IndexList.Items.Add(new ListViewItem() { Content = new IndexElementListView() { ElementList = divers } });

            test = new MyUserControl1() { Data = ResumeElements.Index.Find("Mél") as ResumeElements.Data<string> };
            DataList.Items.Add(new ListViewItem() { Content = test });
            // List data from A to Z
            foreach (ResumeElements.Data d in ResumeElements.Index.DataIndex.Values)
                if (d is ResumeElements.Data<string> el)
                    DataList.Items.Add(new ListViewItem() { Content = new MyUserControl1() { Data = el } });
                    //DataList.Items.Add(new ListViewItem() { Content = new IndexDataTextView() { Data = el } });
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            test.Data = ResumeElements.Index.Find("Nom") as ResumeElements.Data<string>;
        }
    }
}
