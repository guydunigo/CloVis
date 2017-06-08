using ResumeElements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace CloVis.Controls
{
    public sealed partial class NewImageForm : UserControl
    {
        public StorageFile Img { get; set; }

        public NewImageForm()
        {
            this.InitializeComponent();
        }

        public async void Validate(object sender)
        {
            TextBlock txt = new TextBlock();

            if (ImgName.Text == "")
            {
                txt.Text = "Veuillez renseigner un nom.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (Deprecated_Index.Find(ImgName.Text) != null)
            {
                txt.Text = "Ce nom est déjà utilisé pour un autre élément.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (Img == null)
            {
                txt.Text = "Veuillez selectionner une image.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else if (await Deprecated_DataImage.IsImageFilePresent(ImgName.Text))
            {
                txt.Text = "Il existe déjà une image portant ce nom.";
                txt.Foreground = (Application.Current as App).Resources["CloVisOrange"] as SolidColorBrush;
            }
            else
            {
                new Deprecated_DataImage(ImgName.Text, Img);
                ImgName.Text = "";

                txt.Text = "Image ajoutée à Images.";
            }

            var fo = new Flyout()
            {
                Content = txt
            };
            fo.ShowAt(ImgName);
        }

        private void ImgAdd_Click(object sender, RoutedEventArgs e)
        {
            Validate(sender);
        }

        private void ImgName_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
                Validate(sender);
        }

        private async void PickImg_Click(object sender, RoutedEventArgs e)
        {
            var temp = await Deprecated_DataImage.GetImagePicker().PickSingleFileAsync();

            if (temp != null)
            {
                Img = temp;
                ImgFileName.Text = Img.Name;
            }
        }
    }
}
