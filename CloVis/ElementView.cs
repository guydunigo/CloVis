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
    public sealed class ElementView : Control
    {
        public ElementView()
        {
            this.DefaultStyleKey = typeof(ElementView);
            this.Loaded += OnLoaded;

            elementsToAdd = new List<UIElement>();
        }
        
        private List<UIElement> elementsToAdd;

        public void OnLoaded(object sender, RoutedEventArgs e)
        {
            (GetTemplateChild("EList") as Grid).Children.Clear();

            foreach (UIElement el in elementsToAdd)
            {
                (GetTemplateChild("EList") as Grid).Children.Add(el);
            }
        }
        

    }
}
