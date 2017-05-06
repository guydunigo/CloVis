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
    public sealed class IndexElementListView : Control
    {
        public IndexElementListView()
        {
            this.DefaultStyleKey = typeof(IndexElementListView);
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnElementListChanged(this, null);
        }

        public ResumeElements.ElementList ElementList
        {
            get => (ResumeElements.ElementList)(GetValue(ElementListProperty));
            set => SetValue(ElementListProperty, value);
        }

        public static readonly DependencyProperty ElementListProperty =
            DependencyProperty.Register("ElementList", typeof(ResumeElements.ElementList), typeof(IndexElementListView), new PropertyMetadata(null, OnElementListChanged));

        private static void OnElementListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexElementListView instance && instance.ElementList != null)
            {
                if (instance.GetTemplateChild("Name") is TextBlock tb)
                    tb.Text = instance.ElementList.Name;
            }
        }
    }
}
