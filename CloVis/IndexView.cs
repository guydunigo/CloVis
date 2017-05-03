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

// The Templated Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234235

namespace CloVis
{
    public sealed class IndexView : Control
    {
        public IndexView()
        {
            this.DefaultStyleKey = typeof(IndexView);
            this.Loaded += OnLoaded;
            // throw new NotImplementedException("Event handler IndexChanged ?");
            elementsToAdd = new List<UIElement>();

            RenderIndexList();
        }

        private List<UIElement> elementsToAdd;

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("Resume") as Grid).Children.Clear();

            foreach (UIElement el in elementsToAdd)
            {
                (GetTemplateChild("Resume") as Grid).Children.Add(el);
            }
        }
        
        private void RenderIndexList()
        {

        }
    }
}
