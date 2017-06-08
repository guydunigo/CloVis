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

        public Deprecated_DataImage Image
        {
            get => (Deprecated_DataImage)(GetValue(ImageProperty));
            set
            {
                SetValue(ImageProperty, value);
                NotifyPropertyChanged("Image");
            }
        }

        private async static void OnImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is IndexDataImageView instance && instance.Image != null)
            {
                instance.Img.Source = await Deprecated_DataImage.GetImageSource(instance.Image.Value);
                instance.NotifyPropertyChanged("Image");
            }
        }

        public static readonly DependencyProperty ImageProperty =
            DependencyProperty.Register("Image", typeof(Deprecated_DataImage), typeof(IndexDataImageView), new PropertyMetadata(null, OnImageChanged));


        public async void Delete_Click(object sender, RoutedEventArgs e)
        {
            var temp = await Image.Remove();
            if (temp == ImageRemovedOutput.RestoredToDefault)
            {
                Img.Source = await Deprecated_DataImage.GetImageSource(Image.Value);
                NotifyPropertyChanged("Img");
                var f = new Flyout()
                {
                    Content = new TextBlock() { Text = "Cette image a été réinitialisée à celle fournie avec l'application." }
                };
                f.ShowAt(this);
            }
            else if (temp == ImageRemovedOutput.NothingDone)
            {
                var f = new Flyout()
                {
                    Content = new TextBlock()
                    {
                        Text = "Cette image fait partie de l'application et ne peut être supprimée.",
                        Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush
                    }
                };
                f.ShowAt(this);
            }
        }

        public void Rename_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        public async void Replace_Click(object sender, RoutedEventArgs e)
        {
            var temp = await Deprecated_DataImage.GetImagePicker().PickSingleFileAsync();

            if (temp != null)
            {
                await Image.ReplaceImageFile(temp);
                Img.Source = await Deprecated_DataImage.GetImageSource(Image.Value);
                NotifyPropertyChanged("Img");
            }
        }
    }
}
