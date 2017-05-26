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
using System.ComponentModel;
using System.Runtime.CompilerServices;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CloVis.Controls
{
    public sealed partial class IndexDataImageView : UserControl, INotifyPropertyChanged
    {
        public IndexDataImageView()
        {
            this.InitializeComponent();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public DataImage Image
        {
            get => (DataImage)(GetValue(ImageProperty));
            set
            {
                SetValue(ImageProperty, value);
                NotifyPropertyChanged("Image");
            }
        }

        private async static void  OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexDataImageView instance && instance.Image != null)
            {
                instance.Img.Source = await DataImage.GetImageSource(instance.Image.Value);
                instance.NotifyPropertyChanged("Image");
            }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(DataImage), typeof(IndexDataImageView), new PropertyMetadata(null,OnImageChanged));
        

        public void Delete_Click(object sender, RoutedEventArgs e)
        {
            Image.Remove();
        }

        public void Rename_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
