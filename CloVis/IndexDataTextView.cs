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
    public sealed class IndexDataTextView : Control
    {
        public IndexDataTextView()
        {
            this.DefaultStyleKey = typeof(IndexDataTextView);
            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnDataChanged(this, null);
        }

        public ResumeElements.Data<string> Data
        {
            get => (ResumeElements.Data<string>)(GetValue(DataProperty));
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ResumeElements.Data<string>), typeof(IndexDataTextView), new PropertyMetadata(null, OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexDataTextView instance && instance.Data != null)
            {
                instance.DataContext = typeof(ResumeElements.Data<string>);
                if (instance.GetTemplateChild("Name") is TextBlock name)
                    name.Text = instance.Data.Name;
                if (instance.GetTemplateChild("Value") is TextBlock val)
                    val.Text = instance.Data.Value;
            }
        }
    }
}
