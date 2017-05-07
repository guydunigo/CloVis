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

            if (GetTemplateChild("DataView") is StackPanel sp)
                sp.DoubleTapped += Data_DoubleTapped;


            if (GetTemplateChild("AcceptChanges") is Button btn)
                btn.Click += AcceptChanges_Click;
        }

        public ResumeElements.Data<string> Data
        {
            get => (ResumeElements.Data<string>)(GetValue(DataProperty));
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register("Data", typeof(ResumeElements.Data<string>), typeof(IndexDataTextView), new PropertyMetadata(null, OnDataChanged));

        public void SwitchToEditData()
        {
            if (GetTemplateChild("DataView") is StackPanel sp)
                sp.Visibility = Visibility.Collapsed;
            if (GetTemplateChild("DataEdit") is StackPanel sp1)
                sp1.Visibility = Visibility.Visible;
        }
        public void SwitchToViewData()
        {
            if (GetTemplateChild("DataEdit") is StackPanel sp)
                sp.Visibility = Visibility.Collapsed;
            if (GetTemplateChild("DataView") is StackPanel sp1)
                sp1.Visibility = Visibility.Visible;
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexDataTextView instance && instance.Data != null)
            {
                instance.DataContext = typeof(ResumeElements.Data<string>);
                if (instance.GetTemplateChild("NameView") is TextBlock name)
                    name.Text = instance.Data.Name;
                if (instance.GetTemplateChild("ValueView") is TextBlock val)
                    val.Text = instance.Data.Value;
                if (instance.GetTemplateChild("NameEdit") is TextBlock name1)
                    name1.Text = instance.Data.Name;
                if (instance.GetTemplateChild("ValueEdit") is TextBox val1)
                    val1.Text = instance.Data.Value;
            }
        }

        private void Data_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            SwitchToEditData();
        }

        private void AcceptChanges_Click(object sender, RoutedEventArgs e)
        {
            if (GetTemplateChild("ValueEdit") is TextBox tb)
                Data.Value = tb.Text;

            // Back to View mode :
            SwitchToViewData();

            // Update values in the fields :
            OnDataChanged(this, null);

            //throw new NotImplementedException("Save Index ?");
        }
    }
}
