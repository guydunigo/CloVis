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
    public sealed partial class IndexView : UserControl
    {
        public IndexView()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;

            this.DataContext = this;

            Root = Index.Root;
            DataIndex = Index.DataIndex;
        }

        public ElementList Root { get; set; }
        public ElementList DataIndex { get; set; }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            /*
            // List elements in hierarchy
            foreach (ResumeElements.ElementList el in ResumeElements.Index.Root.Values)
                IndexList.Items.Add(new ListViewItem() { Content = new IndexElementListView() { ElementList = el } });

            // If some data is not listed anywhere, show the unlisted category
            var unlisted = ResumeElements.Index.GetUnlistedData();
            if (unlisted.Count > 0)
                IndexList.Items.Add(new ListViewItem() { Content = new IndexElementListView() { ElementList = unlisted } });
                */
        }
    }
}
